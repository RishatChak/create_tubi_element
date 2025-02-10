using Aveva.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeCheck
{
    public class ElementTypeList
    {
        public static readonly DbElementType[] Types = {
            DbElementTypeInstance.PIPE,
            DbElementTypeInstance.ELBOW,
            DbElementTypeInstance.BRANCH,
            DbElementTypeInstance.TUBING,
            DbElementTypeInstance.FTUBE,
            DbElementTypeInstance.BEND,
            DbElementTypeInstance.REDUCER,
            DbElementTypeInstance.TEE,
            DbElementTypeInstance.CROSS,
            DbElementTypeInstance.CAP,
            DbElementTypeInstance.CLOSURE,
            DbElementTypeInstance.OLET,
            DbElementTypeInstance.COUPLING,
            DbElementTypeInstance.UNION,
            DbElementTypeInstance.VALVE,
            DbElementTypeInstance.VTWAY,
            DbElementTypeInstance.VENT,
            DbElementTypeInstance.FILTER,
            DbElementTypeInstance.TRAP,
            DbElementTypeInstance.INSTRUMENT,
            DbElementTypeInstance.FLANGE,
            DbElementTypeInstance.LJSE,
            DbElementTypeInstance.FBLIND,
            DbElementTypeInstance.PCOMPONENT,
            DbElementTypeInstance.SPCOMPONENT,
            DbElementTypeInstance.GASKET,
            DbElementTypeInstance.BBOLT,
            DbElementTypeInstance.WELD,
            DbElementTypeInstance.ATTACHMENT,
            DbElementTypeInstance.AHU,
            DbElementTypeInstance.FLEXIBLE,
            DbElementTypeInstance.SHU,
            DbElementTypeInstance.GENSEC,
            DbElementTypeInstance.PANEL,
            // Добавьте остальные элементы типа DbElementType
    };

        public static DbElementType[] AllElement = new DbElementType[]
        {
            DbElementTypeInstance.PIPE,
            DbElementTypeInstance.ELBOW,
            DbElementTypeInstance.BRANCH,
            DbElementTypeInstance.TUBING,
            DbElementTypeInstance.FTUBE,
            DbElementTypeInstance.BEND,
            DbElementTypeInstance.REDUCER,
            DbElementTypeInstance.TEE,
            DbElementTypeInstance.CROSS,
            DbElementTypeInstance.CAP,
            DbElementTypeInstance.CLOSURE,
            DbElementTypeInstance.OLET,
            DbElementTypeInstance.COUPLING,
            DbElementTypeInstance.UNION,
            DbElementTypeInstance.VALVE,
            DbElementTypeInstance.VTWAY,
            DbElementTypeInstance.VENT,
            DbElementTypeInstance.FILTER,
            DbElementTypeInstance.TRAP,
            DbElementTypeInstance.INSTRUMENT,
            DbElementTypeInstance.FLANGE,
            DbElementTypeInstance.LJSE,
            DbElementTypeInstance.FBLIND,
            DbElementTypeInstance.PCOMPONENT,
            DbElementTypeInstance.SPCOMPONENT,
            DbElementTypeInstance.GASKET,
            DbElementTypeInstance.BBOLT,
            DbElementTypeInstance.WELD,
            DbElementTypeInstance.ATTACHMENT,
            DbElementTypeInstance.AHU,
            DbElementTypeInstance.FLEXIBLE,
            DbElementTypeInstance.SHU,
            DbElementTypeInstance.GENSEC,
            DbElementTypeInstance.PANEL,
            // Добавьте остальные элементы типа DbElementType
        };
    }
}
