using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aveva.Core.Database;
using Aveva.Core.Utilities.Messaging;

namespace Aveva.C.Sharp.Training
{//Разобрать вкладку------------------------------
    public static class Utilites
    {
        public static string AttributeStringByName(DbElement element, string expression)
        {
            try
            {
                PdmsMessage message;
                DbExpression dbExpression;
                var sucess = DbExpression.Parse(expression, out dbExpression, out message);
                return sucess ? element.EvaluateAsString(dbExpression) : "";

            }
            catch{ return string.Empty;}
        }

        public static float AttributeFloatByName(DbElement element, string expression)
        {
            try
            {
                PdmsMessage message;
                DbExpression dbExpression;
                var sucess = DbExpression.Parse(expression, out dbExpression, out message);
                return (float) (sucess ? element.EvaluateDouble(dbExpression, DbAttributeUnit.NONE) : 0.0f);
            }
            catch{ return 0.0f;}
        }

        // Получение перечня трубопроводов
        public static IEnumerable<DbElement> ElementsFromHashtable(Hashtable namesOfPipe)
        {
            var pipeList = new string[namesOfPipe.Count];
            for (var i = 1.0; i <= namesOfPipe.Count; i++)
            {
                var ht = namesOfPipe[i] as Hashtable;
                if (ReferenceEquals(ht, null))
                    continue;
                pipeList[(int)i - 1] = (ht)[1.0].ToString();
            }

            return pipeList.Select(DbElement.GetElement).ToList();
        }

        public static IEnumerable<DbElement> ElementsFromHashtable2(Hashtable namesOfPipe)
        {
            var pipeList = new string[namesOfPipe.Count];
            for (var i = 1.0; i <= namesOfPipe.Count; i++)
            {
                var ht = namesOfPipe[i] as Hashtable;
                if (ReferenceEquals(ht, null))
                    continue;
                pipeList[(int)i - 1] = (ht)[2.0].ToString();
            }

            return pipeList.Select(DbElement.GetElement).ToList();
        }
    }
}
