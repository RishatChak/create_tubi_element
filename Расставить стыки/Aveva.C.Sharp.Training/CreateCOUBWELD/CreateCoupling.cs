using Aveva.Core.Database.Filters;
using Aveva.Core.Database;
using PipeCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aveva.Core.Utilities.CommandLine;
using System.IO;
using System.Globalization;

namespace COUPWELD
{
    internal class CreateCoupling
    {
        ProjCode ProjCode = new ProjCode();
        PMLFunction pMLFunction = new PMLFunction();
        private IEnumerable<DbElement> GetCollection(string cemem)
        {
            var command = Command.CreateCommand($"!!LISTFILE = !!getclashcollections(|{cemem}|)");
            command.Run();
            string listfile = command.GetPMLVariableString("LISTFILE");
            Command.CreateCommand("!LISTFILE.Delete()").Run();
            return File.ReadAllLines(listfile, Encoding.Default).
                    Select(DbElement.GetElement).ToList();
        }

        public void CreateCoupStart(string NamePipe, string collect)
        {
            DbElement ce = DbElement.GetElement("/" + NamePipe);
            if (ce.ToString() == "Null Element")
            {
                if (NamePipe != "Collection")
                {
                    MessageBox.Show("Проверьте имя, либо выберете элемент повторно");
                    return;
                }
            }
            if (collect == "CE")
                ChechBoxResult(NamePipe, ce, null);
            else
                ChechBoxResult(NamePipe, null, GetCollection(collect));
        }

        private void ChechBoxResult(string NamePipe, DbElement elementCreate, IEnumerable<DbElement> dbElements)
        {
            if (elementCreate != null)
                Create(NamePipe, elementCreate);
            else
                foreach (var item in dbElements)
                    Create(NamePipe, item);
        }

        private void Create(string NamePipe, DbElement elementCreate)
        {
            try
            {
                DbElementType bran = DbElementTypeInstance.BRANCH;                                          //Выбор только бранчей
                string havePE = "";

                TypeFilter filtBran = new TypeFilter(bran);                                                 //Настройка фильтра
                DBElementCollection CollectBran = new DBElementCollection(elementCreate, filtBran);         //Собираем в коллекцию
                var OutBran = CollectBran.Cast<DbElement>()
                           .Where(element => element.ElementType == DbElementTypeInstance.BRANCH).ToList(); //Получение из коллекции всех бранчей в трубе в цикле
                var BranList = OutBran.Select(dbElement => new ItemForCheck(dbElement)).ToList();           //Подключаем класс ItemForCheck для работы с pml запросами

                var result = BranList                                                                       //result хранит все имена бранчей для получения в виде DbElement
                .GroupBy(
                    element =>
                    new
                    {
                        element.NamnBran,
                        element.Refno,
                    })
                    .Select(group => new
                    {
                        name = group.Key.NamnBran,
                        Ref = group.Key.Refno,
                    }).ToList();
                var listSum = new List<double>();                           //Объявления листа для сбора и расчёта дистанции
                var listNameElement = new List<Tuple<string, double>>();    //Объявления листа для сбора данных для макроса по построению муфт

                foreach (var item in result)                                
                {
                    DbElement getDbElement;
                    if (item.name.StartsWith("/"))
                        getDbElement = DbElement.GetElement(item.name);
                    else
                        getDbElement = DbElement.GetElement("/" + item.name);

                    DbElementType[] All = ElementTypeList.Types;                                        //Выбор всех элементов
                    TypeFilter filtAll = new TypeFilter(All);                                           //Настройка фильтра
                    DBElementCollection collectAll = new DBElementCollection(getDbElement, filtAll);    //Собираем коллекцию

                    var outAll = collectAll.Cast<DbElement>()                                           //Собираем все элементы внутри бранча
                    .Where(element => element.ElementType == DbElementTypeInstance.TUBING ||
                    element.ElementType == DbElementTypeInstance.SUPPORT ||
                    element.ElementType == DbElementTypeInstance.STRAIGHT ||
                    element.ElementType == DbElementTypeInstance.DIAMOND ||
                    element.ElementType == DbElementTypeInstance.SHU ||
                    element.ElementType == DbElementTypeInstance.ELBOW ||
                    element.ElementType == DbElementTypeInstance.FTUBE ||
                    element.ElementType == DbElementTypeInstance.BEND ||
                    element.ElementType == DbElementTypeInstance.REDUCER ||
                    element.ElementType == DbElementTypeInstance.TEE ||
                    element.ElementType == DbElementTypeInstance.CROSS ||
                    element.ElementType == DbElementTypeInstance.CAP ||
                    element.ElementType == DbElementTypeInstance.CLOSURE ||
                    element.ElementType == DbElementTypeInstance.OLET ||
                    element.ElementType == DbElementTypeInstance.COUPLING ||
                    element.ElementType == DbElementTypeInstance.UNION ||
                    element.ElementType == DbElementTypeInstance.VALVE ||
                    element.ElementType == DbElementTypeInstance.VTWAY ||
                    element.ElementType == DbElementTypeInstance.VENT ||
                    element.ElementType == DbElementTypeInstance.FILTER ||
                    element.ElementType == DbElementTypeInstance.TRAP ||
                    element.ElementType == DbElementTypeInstance.INSTRUMENT ||
                    element.ElementType == DbElementTypeInstance.FLANGE ||
                    element.ElementType == DbElementTypeInstance.LJSE ||
                    element.ElementType == DbElementTypeInstance.FBLIND ||
                    element.ElementType == DbElementTypeInstance.PCOMPONENT ||
                    element.ElementType == DbElementTypeInstance.SPCOMPONENT ||
                    element.ElementType == DbElementTypeInstance.GASKET ||
                    element.ElementType == DbElementTypeInstance.BBOLT ||
                    element.ElementType == DbElementTypeInstance.WELD ||
                    element.ElementType == DbElementTypeInstance.ATTACHMENT).ToList();

                    var AllElementList = outAll.Select(dbElement => new ItemForCheck(dbElement)).ToList();  //Подключаем класс ItemForCheck для работы с pml запросами

                    var resultAllList = AllElementList                                                      //Параметры для расчётов и передачи в макрос
                        .GroupBy(
                    element =>
                    new
                    {
                        element.Dist,
                        element.Refno,
                        element.CE,
                        element.Type,
                        element.Name,
                        element.PStLenghth,
                        element.Dtxr,
                    })
                    .Select(group => new
                    {
                        dist = group.Key.Dist,
                        refno = group.Key.Refno,
                        ce = group.Key.CE,
                        type = group.Key.Type,
                        name = group.Key.Name,
                        lenghth = group.Key.PStLenghth,
                        dtxr = group.Key.Dtxr,
                    }).ToList();

                    string distPML;                                                 //Переменная для макроса созданный Даниилом
                    double sumres = 0;                                              //Объявление вне цикла для сброса до нуля. sumres - сумма всех элементов
                    double calculation = 0;
                    for (int i = 0; i < resultAllList.Count; i++)
                    {
                        string str = resultAllList[i].dist.ToString();              //Получение дистанции и конвертирование в стринг для последующей обработки
                        str = str.Trim(new char[] { 'm' });                         //Удаление не нужного
                        str = str.Replace(".", ",");                                //С точкой не работает, заменил на запятую
                        if (resultAllList[i].type == "ELBO" || resultAllList[i].type == "TEE" || resultAllList[i].type == "VALV")
                            listSum.Clear();                                        //Очистка листа если условия подходят

                        listSum.Add(Convert.ToDouble(str, new CultureInfo("ru-RU")));
                        sumres = listSum.Sum();                                     //Суммируем

                        if ((resultAllList[i].type == "TUBI"
                            || resultAllList[i].type == "REDU")
                            && ProjCode.Projcode == "GCC")                          //Анализирует текущий элемент. Если это REDU или TUBI анализирует dtxr следующего туби
                        {                                                           //При наличии "PE" в атрибуте расставляет: стык муфта стык
                            havePE = "";                                            //Если стоим на элементе TUBI или REDU сбрасываем атрибут dtxr для проверки наличии "РЕ"
                            if (resultAllList[i].dtxr == "true")                    //Если приходит true значит есть "PE" и расставляем: Муфта стык муфта
                                havePE = "PE";                                      //HavePE становиться "РЕ" для передачи PML функцию. При передачи "РЕ" расставляет: Муфта стык муфта
                        }                                                           //Так как это не универсальный код и требуется только "РАЗОВО" код можно удалить.

                        try { Convert.ToDouble(resultAllList[i].lenghth); }
                        catch (Exception) { MessageBox.Show("Ошибка при получении pstLenghth"); return; }

                        if (sumres >= Convert.ToDouble(resultAllList[i].lenghth))   //Если больше установленной дистанции
                        {
                            double resultSum = Convert.ToDouble(resultAllList[i].lenghth) - sumres;         //Получаем остаток в double
                            if (i < resultAllList.Count - 1)                                                // Проверка, что следующий элемент существует
                            {
                                Command commandS = Command.CreateCommand($"!!DISTMBL = !!KOSTIL(|{resultAllList[i].ce}|)");     //Передаём текущий элемент в макрос "Спросить Даниила"
                                commandS.Run();                                                                                 //Запуск макроса
                                distPML = commandS.GetPMLVariableString("DISTMBL");                                             //Получение дистанции до следующего элемента
                                distPML = distPML.Trim(new char[] { 'm' });                                                     //Вырезаем не нужные символы
                                distPML = distPML.Replace(".", ",");                                                //Заменяем на запятую так как C# не работает с дробной частью через ","
                                calculation = resultSum + Convert.ToDouble(distPML, new CultureInfo("ru-RU"));      //В resultSum общая сумма расстояния
                                if (resultAllList[i + 1].ce.Contains("ileave tube of"))                             //Если ce элемент имеет ileave tube of значит это =>
                                {                                                                                   //tubi и игнорируем
                                    var tuple = Tuple.Create(resultAllList[i + 2].refno, resultSum);                //Создание Tuple из двух элементов для макроса
                                    listNameElement.Add(tuple);                                                     //Добавление Tuple
                                }
                                else
                                {
                                    var tuple = Tuple.Create(resultAllList[i + 1].refno, resultSum);        //Создание Tuple из двух элементов для макроса
                                    listNameElement.Add(tuple);                                             //Добавление Tuple
                                }
                            }

                            if (resultSum > 0)
                            {
                                for (int j = 0; resultSum > Convert.ToDouble(resultAllList[i].lenghth); j++)
                                {
                                    Command commandS = Command.CreateCommand($"!!DISTMBL = !!KOSTIL(|{resultAllList[i].ce}|)");     //Передаём текущий элемент в макрос "Спросить Даниила"
                                    commandS.Run();                                                                                 //Запуск макроса
                                    distPML = commandS.GetPMLVariableString("DISTMBL");                                             //Получение дистанции до следующего элемента
                                    distPML = distPML.Trim(new char[] { 'm' });                                                     //Вырезаем не нужные символы
                                    distPML = distPML.Replace(".", ",");
                                    resultSum = resultSum - Convert.ToDouble(resultAllList[i].lenghth) - Convert.ToDouble(distPML);
                                    var tuple = Tuple.Create(resultAllList[i + 2].refno, resultSum);        //Создание Tuple из двух элементов для макроса
                                    listNameElement.Add(tuple);
                                }
                            }
                            else
                            {
                                double lenth = Convert.ToDouble(resultAllList[i].lenghth) - Convert.ToDouble(resultAllList[i].lenghth) - Convert.ToDouble(resultAllList[i].lenghth);
                                for (int j = 0; resultSum < lenth; j++)
                                {
                                    Command commandS = Command.CreateCommand($"!!DISTMBL = !!KOSTIL(|{resultAllList[i].ce}|)");     //Передаём текущий элемент в макрос "Спросить Даниила"
                                    commandS.Run();                                                                                 //Запуск макроса
                                    distPML = commandS.GetPMLVariableString("DISTMBL");                                             //Получение дистанции до следующего элемента
                                    distPML = distPML.Trim(new char[] { 'm' });                                                     //Вырезаем не нужные символы
                                    distPML = distPML.Replace(".", ",");
                                    resultSum = resultSum + Convert.ToDouble(resultAllList[i].lenghth) + Convert.ToDouble(distPML);
                                    var tuple = Tuple.Create(resultAllList[i + 2].refno, resultSum);        //Создание Tuple из двух элементов для макроса
                                    listNameElement.Add(tuple);
                                }
                            }

                            calculation = 0 - calculation;                              //Превращаем дистанцию в отрицательную, для макроса, а именно установки элемента до текущего
                            listSum.Clear();                                            //Очистка - Очищает если был добавлен новый элемент в listNameElement
                            listSum.Add(calculation);                                   //Добавляется для следующих расчётов. Что бы при следующем цикле дистанция уже была рассчитана
                        }
                    }
                }

                foreach (var coup in listNameElement)                                                       //Перебор для передачи в макрос
                {
                    Command commands = Command.CreateCommand($"{coup.Item1}");                              //Встаём на элемент
                    commands.Run();
                    string dist = coup.Item2.ToString();
                    dist = dist.Replace(",", ".");
                    Command command = Command.CreateCommand($"!!newcoup(|{dist}|, {coup.Item1},||)");       //Создаём новый coup
                    command.Run();
                    if (coup.Item2 >= -100)                                                                 //"-100" минимальная дистанция. Запрос от руководства
                        MessageBox.Show($"Возможно наложение муфты на элементы трубопровода. Необходима проверка.\nЕлемент:{coup.Item1} ");
                }

                foreach ( var items in result)
                    if (havePE == "PE")
                        Command.CreateCommand($"!!newcoup(||, {items.Ref},|{havePE}|)").RunInPdms();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
