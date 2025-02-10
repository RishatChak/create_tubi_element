using Aveva.Core.Database;
using Aveva.Core.Database.Filters;
using Aveva.Core.Utilities.CommandLine;
using PipeCheck;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COUPWELD
{

    public class CreateWELDING
    {
        PMLFunction pMLFunction = new PMLFunction();
        ProjCode projCode = new ProjCode();

        private IEnumerable<DbElement> GetCollection(string cemem)                                  //PML Функция забирает коллекцию из AVEVA
        {
            var command = Command.CreateCommand($"!!LISTFILE = !!getclashcollections(|{cemem}|)");
            command.Run();
            string listfile = command.GetPMLVariableString("LISTFILE");
            Command.CreateCommand("!LISTFILE.Delete()").Run();
            return File.ReadAllLines(listfile, Encoding.Default).
                    Select(DbElement.GetElement).ToList();
        }

        public void CreateWELDStart(string NamePipe, string collect, bool onlyMem)                   //Начало построение weld
        {
            DbElement ce = DbElement.GetElement("/" + NamePipe);                   // "/" добавляется для библиотеки AVEVA. Что бы программа увидела элемент и не вернула null
            if (collect == "CE")                                                        //Если приходит СЕ значит труба одна и код работает без цикла
                ChecheBoxResult(ce, null, onlyMem);
            else
                ChecheBoxResult(null, GetCollection(collect), onlyMem);                 //Если другое значение, не СЕ, то используется цикл
        }

        private void ChecheBoxResult(DbElement elementCreate, IEnumerable<DbElement> dbElements, bool onlyMem)
        {
            if (!onlyMem)
            {
                if (elementCreate != null)              //Проверка используется для распознания коллекции. Если коллекция передаётся в цикл.
                    Create(elementCreate);
                else
                    foreach (var memOfCollection in dbElements)    //Цикл обработки элементов коллекции
                        Create(memOfCollection);
            }
            else
            {
                if (elementCreate != null)              //Проверка используется для распознания коллекции. Если коллекция передаётся в цикл.
                    CreateOnlyMem(elementCreate);
                else
                    foreach (var memOfCollection in dbElements)
                        CreateOnlyMem(memOfCollection);
            }
        }

        public void CreateOnlyMem(DbElement elementCreate)
        {
            DbElementType bran = DbElementTypeInstance.BRANCH;                                          //Выбор только бранчей
            TypeFilter filtBran = new TypeFilter(bran);                                                 //Настройка фильтра
            DBElementCollection CollectBran = new DBElementCollection(elementCreate, filtBran);         //Собираем в коллекцию
            var OutBran = CollectBran.Cast<DbElement>()
                       .Where(element => element.ElementType == DbElementTypeInstance.BRANCH).ToList(); //Получение из коллекции всех бранчей в трубе в цикле
            var BranList = OutBran.Select(dbElement => new ItemForCheck(dbElement)).ToList();           //Подключаем класс ItemForCheck для работы с pml запросами

            var result = BranList                                                                       //result хранит все элементы внутри бранча
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
                    refno = group.Key.Refno,
                }).ToList();

            foreach (var item in result)
            {
                if (item.name.StartsWith("/"))
                    Command.CreateCommand($"{item.name}").RunInPdms();                  //Переходим на указанный элемент(труба или бранч)          
                else
                    Command.CreateCommand($"/{item.name}").RunInPdms();                 //Переходим на указанный элемент(труба или бранч)          
                Command.CreateCommand($"!!newweld(||, {item.refno}, ||)").RunInPdms();      //Установка WELD. Устанавливается на начале и в конце elbow, тее и tube
            }
        }

        private void Create(DbElement elementCreate)
        {
            try
            {
                DbElementType bran = DbElementTypeInstance.BRANCH;                                          //Выбор только бранчей
                TypeFilter filtBran = new TypeFilter(bran);                                                 //Настройка фильтра
                DBElementCollection CollectBran = new DBElementCollection(elementCreate, filtBran);         //Собираем в коллекцию
                var OutBran = CollectBran.Cast<DbElement>()
                           .Where(element => element.ElementType == DbElementTypeInstance.BRANCH).ToList(); //Получение из коллекции всех бранчей в трубе в цикле
                var BranList = OutBran.Select(dbElement => new ItemForCheck(dbElement)).ToList();           //Подключаем класс ItemForCheck для работы с pml запросами

                var result = BranList                                                                       //result хранит все элементы внутри бранча
                .GroupBy(
                    element =>
                    new
                    {
                        element.NamnBran,
                        element.Name,
                        element.Refno,
                        element.Hcon,
                        element.Href,
                        element.TypeOfHref,
                    })
                    .Select(group => new
                    {
                        name = group.Key.NamnBran,
                        typeOfHref = group.Key.TypeOfHref,
                        nameof = group.Key.Name,
                        refno = group.Key.Refno,
                        hcon = group.Key.Hcon,
                        href = group.Key.Href,
                    }).ToList();

                foreach (var item in result)
                {
                    if (item.name.StartsWith("/"))
                        Command.CreateCommand($"{item.name}").RunInPdms();                  //Переходим на указанный элемент(труба или бранч)          
                    else
                        Command.CreateCommand($"/{item.name}").RunInPdms();                 //Переходим на указанный элемент(труба или бранч)          
                    Command.CreateCommand($"!!newweld(||, {item.refno}, ||)").RunInPdms();      //Установка WELD. Устанавливается на начале и в конце elbow, тее и tube
                }

                var listHcon = new List<Tuple<double, string>>();                               //Слева хранится дистанция до предыдущего weld, справа имя bran
                double dDistconvert = 0;

                foreach (var item in result)
                {
                    if (item.href != null)
                        if (item.hcon != "BWD" || item.hcon != "TBP")                                 //item.hcon тип подключения
                        {
                            if (item.typeOfHref == "BRAN")
                                pMLFunction.ReturnDistFirstMemtoLastMem(item.href, item.name, out dDistconvert);
                            else
                            {
                                double distofpref = 0;
                                pMLFunction.ConstDistofPrefWeld(item.href, out distofpref);
                                var tuple = Tuple.Create(distofpref, item.name);
                                listHcon.Add(tuple);
                            }
                        }
                }

                foreach (var item in result)
                {
                    var listSum = new List<double>();                                           //Лист для будущих расчётов, суммой всех дистанции. +Сброс при новой итерации
                    var getBran = DbElement.GetElement(item.name);                              //Получаем бранч в виде DbElement

                    DbElementType[] All = ElementTypeList.Types;                                //Выбор всех элементов
                    TypeFilter filtAll = new TypeFilter(All);                                   //Настройка фильтра
                    DBElementCollection collectAll = new DBElementCollection(getBran, filtAll); //Собираем коллекцию

                    var outAll = collectAll.Cast<DbElement>()                                   //Собираем все элементы внутри бранча
                    .Where(element =>
                    element.ElementType == DbElementTypeInstance.TUBING ||
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

                    var AllElementList = outAll.Select(dbElement => new ItemForCheck(dbElement)).ToList(); //Подключаем класс ItemForCheck для выполнения запросов PML

                    var resultAllList = AllElementList
                        .GroupBy(
                    element =>
                    new
                    {
                        element.Dist,
                        element.Dtxr,
                        element.Refno,
                        element.CE,
                        element.Type,
                        element.Name,
                        element.NameOwn,
                        element.PStLenghth,
                    })
                    .Select(group => new
                    {
                        dist = group.Key.Dist,
                        dtxr = group.Key.Dtxr,
                        ce = group.Key.CE,
                        refno = group.Key.Refno,
                        type = group.Key.Type,
                        nameOwn = group.Key.NameOwn,
                        name = group.Key.Name,
                        pstLenghth = group.Key.PStLenghth,
                    }).ToList();

                    foreach (var itemHcon in listHcon)
                        if (itemHcon.Item2 == item.nameof)
                            listSum.Add(itemHcon.Item1);

                    double sumres = 0;
                    double calculation = 0;
                    string distPML = "";
                    string havePE = "";                                //HavePЕ было добавлено по просьбе. Скорее всего данный код уникальный и потребуется лишь пару раз и после станет бесполезным
                    for (int i = 0; i < resultAllList.Count; i++)
                    {
                        if (resultAllList[0].type == "TUBI" && i == 1) //Если 0 элемент TUBI и мы стоим на первом[втором] элементе. Специфичный костыль. Запрос сверху
                        {
                            Command.CreateCommand($"{resultAllList[i].name}").RunInPdms();                          //Встаём на указанный анализируемый бранч
                            Command getHrefOfOwner = Command.CreateCommand($"!!GETHREFOWER = !!pmlcommand(|type of href of owner|)");   //Запрос типа родительского элемента. В данном случае BRAN
                            getHrefOfOwner.Run();                                                                   //Запускаем макрос
                            string res = getHrefOfOwner.GetPMLVariableString("GETHREFOWER");                        //Получаем тип в переменную
                            Command.CreateCommand("!!GETHREFOWER.Delete()").Run();

                            if (res == "BRAN")                                                                      //Выполнение в случае BRAN
                            {
                                double distofpref2 = 0;
                                pMLFunction.ReturnDistElementtoNameLastMemHrefOwn(out distofpref2);
                                listSum.Add(distofpref2);
                            }
                            else                                                                                    //Выполняем в случае другого элемента
                            {
                                double distofpref2 = 0;
                                pMLFunction.RetrunDistElemntToHref(item.href, out distofpref2);
                                listSum.Add(distofpref2);
                            }
                        }

                        string str = resultAllList[i].dist.ToString();                                              //Конвертируем в стринг что бы избавиться от лишнего
                        if (resultAllList[i].type == "WELD")
                            listSum.Clear();                                                                        //Очищаем список если встретили weld
                        string message = "Ошибка: Create";
                        listSum.Add(PMLFunction.ConvertToDouble(str, message));                                                         //Возвращаем double
                        //listSum.Add(PMLFunction Convert.ToDouble(str));                                                         //Возвращаем double

                        int countlastElem = resultAllList.Count - 1;
                        if (i == countlastElem)                                                     //Если это последний элемент и не совпадает с позицией хвоста 
                        {                                                                           //Заменяем точку на запятую. C# не работает с точкой
                            double distofpref2 = 0;

                            if (resultAllList.Count() == 1)
                            {
                                pMLFunction.CreateWeldOneTubi(resultAllList[i].refno.ToString(), resultAllList[i].pstLenghth, out distofpref2);

                            }
                            else
                                pMLFunction.ReturnPosTpos(resultAllList[i].refno.ToString(), out distofpref2);

                            listSum.Add(distofpref2);
                        }

                        sumres = listSum.Sum();

                        if ((resultAllList[i].type == "TUBI"
                            || resultAllList[i].type == "REDU")
                           // || resultAllList[i].type == "ATTA")
                            && projCode.Projcode == "GCC")                                          //Анализирует текущий элемент. Если это REDU или TUBI анализирует dtxr следующего туби
                        {                                                                           //При наличии "PE" в атрибуте расставляет: стык муфта стык
                            havePE = "";                                                            //Если стоим на элементе TUBI или REDU сбрасываем атрибут dtxr для проверки наличии "РЕ"
                            if (resultAllList[i].dtxr == "true")                                    //Если приходит true значит есть "PE" и расставляем: Муфта стык муфта
                                havePE = "PE";                                                      //HavePE становиться "РЕ" для передачи PML функцию. При передачи "РЕ" расставляет: Муфта стык муфта
                        }                                                                           //Так как это не универсальный код и требуется только "РАЗОВО" код можно удалить. 

                        try { double checkPstLen = Convert.ToDouble(resultAllList[i].pstLenghth); }
                        catch (Exception ) { MessageBox.Show("Инструмент не может найти атрибут (pstLenghth)"); return; }

                        if (sumres >= Convert.ToDouble(resultAllList[i].pstLenghth))                                   //length - максимальная дистанция трубы из каталога
                        {
                            double resultSum = Convert.ToDouble(resultAllList[i].pstLenghth) - sumres;                 //resultSum будет хранить в себе дистанцию на которой нужно установить элемент
                            int countList = listSum.Count;
                            double Itsafinalcountdown = listSum[countList - 1];
                            double finaliresultsum = Itsafinalcountdown + resultSum;

                            if (i != resultAllList.Count - 1)                                               //Проверка, что следующий элемент существует //
                            {
                                if (resultAllList[i + 1].ce.Contains("ileave tube of"))                     //Если содержит ileave tube of берём след элемент //Добавляем для установки элементов через макрос
                                    pMLFunction.CreateElement(resultAllList[i + 2].refno, resultSum, havePE);//PE - содержит ли атрибут dtxr PE
                                else                                                                        //Добавляем для установки элементов через макрос
                                    pMLFunction.CreateElement(resultAllList[i + 1].refno, resultSum, havePE);
                            }
                            else
                                pMLFunction.CreateElement(resultAllList[i].refno, finaliresultsum, havePE);

                            ///
                            /// В случае если расстояние больше чем указано в каталоге, то работает следующий код
                            /// В зависимости от расстояния цикл может работать несколько раз.
                            /// Перестает работать когда расстояние становится меньше каталожной
                            ///

                            if (resultSum > 0)        //При условии что resultSum положительное число нужно отнимать
                            {
                                for (int j = 0; resultSum > Convert.ToDouble(resultAllList[i].pstLenghth); j++)
                                {
                                    if (havePE == "PE")
                                        resultSum = CalculationMBL(resultAllList[i].ce, resultSum, Convert.ToDouble(resultAllList[i].pstLenghth), havePE);//расчёт дистанции у капов(если нужно)
                                    else
                                        resultSum = resultSum - Convert.ToDouble(resultAllList[i].pstLenghth);

                                    if (resultAllList[i + 1].ce.Contains("ileave tube of"))
                                        pMLFunction.CreateElement(resultAllList[i + 2].refno, resultSum, havePE);
                                    else
                                        pMLFunction.CreateElement(resultAllList[i + 1].refno, resultSum, havePE);
                                }
                            }
                            else
                            {
                                double lenth = Convert.ToDouble(resultAllList[i].pstLenghth) - Convert.ToDouble(resultAllList[i].pstLenghth) - Convert.ToDouble(resultAllList[i].pstLenghth);
                                for (int j = 0; resultSum < lenth; j++)
                                {
                                    if (havePE == "PE")
                                        resultSum = CalculationMBL(resultAllList[i].ce, resultSum, Convert.ToDouble(resultAllList[i].pstLenghth), havePE);
                                    else
                                        resultSum = resultSum + Convert.ToDouble(resultAllList[i].pstLenghth);

                                    // Проверка, что i + 1 и i + 2 находятся в пределах границ массива
                                    if (i + 1 < resultAllList.Count && resultAllList[i + 1].ce.Contains("ileave tube of"))
                                    {
                                        if (i + 2 < resultAllList.Count)
                                            pMLFunction.CreateElement(resultAllList[i + 2].refno, resultSum, havePE);
                                    }
                                    else if (i + 1 < resultAllList.Count)
                                    {
                                        pMLFunction.CreateElement(resultAllList[i + 1].refno, resultSum, havePE);
                                    }
                                }

                            }
                            calculation = resultSum;                                                        //calculation будет хранить resultSum для следующего элемента
                            calculation = 0 - calculation;                                                  //Превращаем в отрицательное число 
                            listSum.Clear();                                                                //Очистка - Очищает если был добавлен новый элемент в listNameElement
                            if (i != resultAllList.Count - 1)
                                listSum.Add(calculation);                                                   //При следующем входе в цикл расчёт начнётся с учётом calculation
                        }
                    }
                }

                foreach (var item in result)
                {
                    if (item.name.StartsWith("/"))
                        Command.CreateCommand($"{item.name}").RunInPdms();                  //Переходим на указанный элемент(труба или бранч)          
                    else
                        Command.CreateCommand($"/{item.name}").RunInPdms();                 //Переходим на указанный элемент(труба или бранч)          
                    Command.CreateCommand($"!!newweld(||, {item.refno}, ||)").RunInPdms();      //Установка WELD. Устанавливается на начале и в конце elbow, тее и tube
                }

                foreach (var item in result)
                    pMLFunction.DeleteWeldSamePos(item.refno);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private double CalculationMBL(string ce, double resultSum, double pstlen, string havePE)
        {
            string distPML;
            string message = "Ошибка: CalculationMBL";
            if (havePE == "PE")
            {
                if (resultSum > 0)
                {
                    Command commandSs = Command.CreateCommand($"!!DISTMBL = !!KOSTIL(|{ce}|)");     //Передаём текущий элемент в макрос для получении дистанции P1 and P2 у COUP
                    commandSs.Run();                                                                //Запуск макроса
                    distPML = commandSs.GetPMLVariableString("DISTMBL");                            //Получение дистанции P1 and P2 у COUP
                    resultSum = resultSum - Convert.ToDouble(pstlen) - PMLFunction.ConvertToDouble(distPML, message);
                }
                else
                {
                    Command commandS = Command.CreateCommand($"!!DISTMBL = !!KOSTIL(|{ce}|)");      //Передаём текущий элемент в макрос для получения дистанции P1 and P2 у COUP
                    commandS.Run();                                                                 //Запуск макроса
                    distPML = commandS.GetPMLVariableString("DISTMBL");                             //Получение дистанции P1 and P2 у COUP
                    resultSum = resultSum + Convert.ToDouble(pstlen) + PMLFunction.ConvertToDouble(distPML, message);
                }
                Command.CreateCommand("!!DISTMBL.Delete()").Run();
                return resultSum;
            }
            return resultSum;
        }
    }
}
