using Aveva.Core.Database;
using Aveva.Core.Utilities.CommandLine;
using COUPWELD.Other;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace COUPWELD
{
    using static Aveva.C.Sharp.Training.Utilites;
    public class PMLFunction
    {
        public void ReturnDistFirstMemtoLastMem(string itemHref, string itemName, out double dDistconvert)
        {
            Command.CreateCommand($"{itemHref}").RunInPdms();                                           //Переходим на указанный элемент 
            Command comReqLastMem = Command.CreateCommand($"!!LAST = !!pmlcommand(|last mem|)");        //Запрос последнего элемента
            comReqLastMem.Run();                                                                        //Запуск запроса
            string respLastMem = comReqLastMem.GetPMLVariableString($"LAST");                           //Получаем последний элемент в переменную

            Command.CreateCommand($"{itemName}").RunInPdms();                                           //Переходим на анализируемый бранч

            Command comReqFirstmem = Command.CreateCommand($"!!FIRS = !!pmlcommand(|first mem|)");      //Запрос первого элемента после того как встали на анализируемый бранч
            comReqFirstmem.Run();                                                                       //Запуск запроса
            string respFirstMem = comReqFirstmem.GetPMLVariableString($"FIRS");                         //Получаем первый элемент в переменную

            Command.CreateCommand($"{respFirstMem}").RunInPdms();                                       //Переходим на первый элемент
            Command getDistofFitsAndLAST = Command.CreateCommand($"!!RESDIST = !!pmlcommand(|const dist pa to pl of {respLastMem}|)");//Запрос дистанции до предыдущего weld стоя на первом элементе
            getDistofFitsAndLAST.Run();                                                                 //Запуск запроса
            string getDist = getDistofFitsAndLAST.GetPMLVariableString($"RESDIST");
            string message = "Ошибка: ReturnDistFirstMemtoLastMem";
            dDistconvert = ConvertToDouble(getDist, message);

            Command.CreateCommand("!!LAST.DELETE()").RunInPdms();
            Command.CreateCommand("!!FIRS.DELETE()").RunInPdms();
            Command.CreateCommand("!!RESDIST.DELETE()").RunInPdms();
        }

        public void ConstDistofPrefWeld(string itemHref, out double distofpref)
        {
            var test = DbElement.GetElement(itemHref);                                                  //Получаем href в виде DbElement
            string dbElement = AttributeStringByName(test, "ref");
            string dbElementType = AttributeStringByName(test, "type");
            string abore = AttributeStringByName(test, "abore");
            string boreCref = AttributeStringByName(test, "hbore of cref");

            if (dbElementType == "NOZZ" || (dbElementType == "OLET" && abore != boreCref))
            {
                distofpref = 0;
                return;
            }

            Command.CreateCommand($"{dbElement}").RunInPdms();                                              //Переходим на указанный элемент 
            Command com = Command.CreateCommand($"!!PREV = !!pmlcommand(|const dist pa to pl of prev weld|)");//Запрос дистанции до предыдущего weld
            com.Run();                                                                                  //Запуск запроса
            string distOfPREV = com.GetPMLVariableString($"PREV");                                      //Получение дистанции до предыдущего weld
            string message = "Ошибка: ConstDistofPrefWeld";
            distofpref = ConvertToDouble(distOfPREV, message);
            Command.CreateCommand("!!PREV.DELETE()").RunInPdms();
        }

        public void ReturnDistElementtoNameLastMemHrefOwn(out double distofpref2)
        {
            //Запрос дистанции стоя на анализируемом элементе
            distofpref2 = 0;
            try
            {
                Command lastElemOfHref = Command.CreateCommand($"!!LASTELOFHREF = !!pmlcommand(|name of last weld of href of owner|)");
                lastElemOfHref.Run();                                                                       //Запуск запроса
                string resLastElHref = lastElemOfHref.GetPMLVariableString("LASTELOFHREF");                 //Получаем дистанцию в переменную
                Command coms = Command.CreateCommand($"!!PREVET2 = !!pmlcommand(|const dist pa to pl of {resLastElHref}|)");//Дистанция от текущего элемента, до указанного
                coms.Run();                                                                                 //Запускаем макрос
                string distOfPREV2 = coms.GetPMLVariableString($"PREVET2");                                 //Получаем дистанцию
                string message = "Ошибка: ReturnDistElementtoNameLastMemHrefOwn";
                distofpref2 = ConvertToDouble(distOfPREV2, message);
            }
            catch (Exception)
            {
                Command.CreateCommand("$P Растояние между сварными стыками могут быть не верными, так как в присоединённом трубопроводе отсутствуют сварные стыки. Необходима проверка!").RunInPdms();
                Command lastElemOfHref = Command.CreateCommand($"!!LASTELOFHREF = !!pmlcommand(|name of last mem of href of owner|)"); //Тут предыдущая версия. Изменён мем на велд 
                lastElemOfHref.Run();                                                                       //Запуск запроса
                string resLastElHref = lastElemOfHref.GetPMLVariableString("LASTELOFHREF");                 //Получаем дистанцию в переменную
                Command coms = Command.CreateCommand($"!!PREVET2 = !!pmlcommand(|const dist pa to pl of {resLastElHref}|)");//Дистанция от текущего элемента, до указанного
                coms.Run();                                                                                 //Запускаем макрос
                string distOfPREV2 = coms.GetPMLVariableString($"PREVET2");                                 //Получаем дистанцию
            }

            Command.CreateCommand("!!LASTELOFHREF.DELETE()").RunInPdms();
            Command.CreateCommand("!!PREVET2.DELETE()").RunInPdms();
        }

        public void RetrunDistElemntToHref(string itemHref, out double distofpref2)
        {
            if (itemHref == "null" || itemHref == "unset" || itemHref == "nulref" || itemHref == null)
            {
                distofpref2 = 0.0;
                return;
            }

            Command getDistOfFirstElement = Command.CreateCommand($"!!FIRSTDIST = !!pmlcommand(|const dist pa to p2 of {itemHref}|)");
            getDistOfFirstElement.Run();
            string resFirstDist = getDistOfFirstElement.GetPMLVariableString("FIRSTDIST");
            string message = "Ошибка: RetrunDistElemntToHref";
            distofpref2 = ConvertToDouble(resFirstDist, message);
            Command.CreateCommand("!!FIRSTDIST .DELETE()").RunInPdms();
        }
        public void ReturnPosTpos(string refno, out double distofpref2)
        {
            Command asd = Command.CreateCommand($"!!LILTLE = !!POSTPOS(|{refno}|)");
            asd.Run();
            string res = asd.GetPMLVariableString("LILTLE");
            string message = "Ошибка: ReturnPosTpos";
            distofpref2 = ConvertToDouble(res, message);
            Command.CreateCommand("!!LILTLE .DELETE()").RunInPdms();
        }

        public void CreateWeldOneTubi(string refno, string dist, out double distofpref2)
        {
            string message = "Ошибка: CreateWeldOneTubi";
            double distForTubi = ConvertToDouble(dist, message);

            Command asd = Command.CreateCommand($"!!ONETUBI = !!POSTPOSONETUBI(|{refno}|, |{distForTubi}|)");
            asd.Run();
            string res = asd.GetPMLVariableString("ONETUBI");
            distofpref2 = ConvertToDouble(res, message);
            Command.CreateCommand("!!ONETUBI .DELETE()").RunInPdms();
        }

        public void DeleteWeldSamePos(string refno)
        {
            Command.CreateCommand($"!!newweld(|check|, {refno}, ||)").RunInPdms();            //Установка WELD. Устанавливается на начале и в конце elbo, тее и tube
        }

        public void CreateElement(string weld, double getDist, string PE)
        {
            Command commands = Command.CreateCommand($"{weld}");                                //Переходим на указанный элемент
            commands.Run();
            string dist = getDist.ToString();                                                   //Получаем дистанцию и заменяем точки на запятые. В этот раз для передачи
            dist = dist.Replace(",", ".");
            Command command = Command.CreateCommand($"!!newweld(|{dist}|, {weld}, |{PE}|)");            //Установка weld
            command.Run();                                                                      //Запускаем макрос
            if (getDist >= -100)                                                                //Вывод сообщения если дистанция меньше 101
            {
                Command.CreateCommand($"$P Элемент {weld} находится близко к coup/weld").RunInPdms();
                NotificationClass notificationClass = new NotificationClass();
                notificationClass.Notification("Ошибка построения!", "Дистанция меньше 100mm!");
            }
        }

        public static double ConvertToDouble(string query, string errorMesage)
        {
            query = query.Trim(new char[] { 'm' });
            double result = 0;
            try
            {
                result = double.Parse(query, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MessageBox.Show(errorMesage, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            result = Convert.ToDouble(query);
            return result;
        }

        public static void PrintMessage(string message)
        {
            Command.CreateCommand($"$P {message}").RunInPdms();
        }
    }
}