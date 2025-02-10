using Aveva.Core.Database;
using Aveva.Core.Utilities.CommandLine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PipeCheck
{
    using static Aveva.C.Sharp.Training.Utilites;
    class ItemForCheck
    {
        public DbElement Element { get; set; }

        public ItemForCheck(DbElement element)
        {
            Element = element;
        }

        public string NamnBran
        {
            get
            {

                string _namnPipe = AttributeStringByName(Element, "name");
                return _namnPipe;

            }
        }
        
        public string NamnForISO
        {
            get
            {
                string _namn = AttributeStringByName(Element, "namn");

                int firstSlashIndex = _namn.IndexOf("/");
                int fifthDashIndex = -1;
                for (int i = 0, count = 0; i < _namn.Length; i++)
                {
                    if (_namn[i] == '-')
                    {
                        count++;
                        if (count == 5)
                        {
                            fifthDashIndex = i;
                            break;
                        }
                    }
                }

                // Если нашли первый слеш и пятый дефис
                if (firstSlashIndex != -1 && fifthDashIndex != -1)
                {
                    // Получаем нужный фрагмент строки
                    _namn = _namn.Substring(firstSlashIndex + 1, fifthDashIndex - firstSlashIndex - 1);
                }

                return _namn;

            }
        }
        public string Dtxr
        {
            get
            {
                string _dtxr = AttributeStringByName(Element, "dtxr of next tubi");
                if (_dtxr == "")
                    _dtxr = AttributeStringByName(Element, "dtxr of prev tubi");

                string[] checkDE = _dtxr.Split(',');
                bool result = checkDE.Contains(" PE");
                if (!result)
                    return null;

                return "true";

            }
        }
        public string Name
        {
            get
            {

                string _namnPipe = AttributeStringByName(Element, "name");
                return _namnPipe;

            }
        }
        public string Type
        {
            get
            {
                string Type = AttributeStringByName(Element, "type");
                if (Type.Contains("PIPE"))
                    return null;
                else
                {
                    return Type;
                }
            }
        }
        public string TypeForCheck
        {
            get
            {
                string Type = AttributeStringByName(Element, "type");
                if (!Type.Contains("PIPE"))
                    return null;
                else
                {
                    return Type;
                }
            }
        }
        
        public string Itlength
        {
            get
            {
                string ltlength = AttributeStringByName(Element, "Itlength");
                return ltlength;
            }
        }
        
        public string Refno
        {
            get
            {
                string refno = AttributeStringByName(Element, "refno");
                return refno;
            }
        }
        
        public string Position
        {
            get
            {
                string pos = AttributeStringByName(Element, "pos");
                return pos;
            }
        }
        
        public string Dist
        {
            get
            {
                string Type = AttributeStringByName(Element, "type");
                string Ref = AttributeStringByName(Element, "refno");

                if (Type == "TUBI")
                {
                    return "0";
                }
                else
                {
                    string getNameElement = Element.ToString();
                    string stringToRemove = "ileave tube of ";
                    string resultString = getNameElement.Replace(stringToRemove, "");


                    Command commandS = Command.CreateCommand($"!!DIST = !!distofnext(|{Ref}|)");    //Передаём текущий элемент в макрос
                    commandS.Run();
                    string dist = commandS.GetPMLVariableString("DIST");

                    return dist;
                }
            }
        }
        public string CE
        {
            get
            {
                string _ce = AttributeStringByName(Element, "ce");
                return _ce;
            }
        }
        
        public string Lineshifr
        {
            get
            {
                string _lineshifr = AttributeStringByName(Element, ":Lineshifr");
                return _lineshifr;
            }
        }
        
        public string PStLenghth
        {
            get
            {
                float abore = AttributeFloatByName(Element, "abore");
                string pdaref = AttributeStringByName(Element, "pdaref of spec of pspec of pipe");

                Command pstlencollect = Command.CreateCommand($"var !!PSTLENCOL COLLECT ALL pdaele with (Nbore eq {abore}) for {pdaref}");
                pstlencollect.Run();

                Command pstlen = Command.CreateCommand($"!!PSTLEN = !!pmlcommand(|pstlen of $!!PSTLENCOL[1]|)");
                pstlen.Run();
                string pstlenres = pstlen.GetPMLVariableString("PSTLEN");

                pstlenres = pstlenres.Trim(new char[] { 'm' });
                pstlenres = pstlenres.Replace(".", ",");
                return pstlenres;
            }
        }
        
        public string Hcon
        {
            get
            {
                string _Hcon = AttributeStringByName(Element, "hcon");
                return _Hcon;
            }
        }
        
        public string Tcon
        {
            get
            {
                string _Tcon = AttributeStringByName(Element, "tcon");
                return _Tcon;
            }
        }
        
        public string Href
        {
            get
            {
                string _href = AttributeStringByName(Element, "href");              //Href анализируемого бранча
                string _hrefType = AttributeStringByName(Element, "type of href");  //Тип Href анализируемого бранча

                if (_hrefType == "unset" || _hrefType == "Nulref")                  //Проверка на наличие Href анализируемого бранча
                    return null;

                if(_hrefType == "BRAN")                                             //Проверка Href на бранч или это другой элемент 
                    return _href;

                return _href;
            }
        }
        
        public string TrefPos
        {
            get
            {
                string _tref = AttributeStringByName(Element, "tref");              //Href анализируемого бранча
                string _trefType = AttributeStringByName(Element, "type of tref");  //Тип Href анализируемого бранча

                if (_trefType == "unset" || _trefType == "Nulref")                  //Проверка на наличие Href анализируемого бранча
                    return null;

                if(_trefType == "BRAN")                                             //Проверка Href на бранч или это другой элемент 
                    return _tref;

                return _tref;
            }
        }

        public string HrefPos
        {
            get
            {
                string _href = AttributeStringByName(Element, "href");              //Href анализируемого бранча
                string _hrefType = AttributeStringByName(Element, "type of href");  //Тип Href анализируемого бранча

                if (_hrefType == "unset" || _hrefType == "Nulref")                  //Проверка на наличие Href анализируемого бранча
                    return null;

                if (_hrefType == "BRAN")                                             //Проверка Href на бранч или это другой элемент 
                    return _href;

                return null;
            }
        }

        public string NameOwn
        {
            get
            {
                string _nameOwn = AttributeStringByName(Element, "name of own");
                return _nameOwn;
            }

        }

        public string NameOwnPipe
        {
            get
            {
                string _nameOwn = AttributeStringByName(Element, "name of pipe");
                return _nameOwn;
            }
        }

        public string NameOwnBran
        {
            get
            {
                string _nameOwn = AttributeStringByName(Element, "name of bran");
                return _nameOwn;
            }
        }

        public string TypeOfHref
        {
            get
            {
                string _typeOfHref = AttributeStringByName(Element, "type of href");
                return _typeOfHref;
            }
        }
    }
}
