using Aveva.Core.Utilities.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COUPWELD
{
    internal class ProjCode
    {
        public string Projcode
        {
            get
            {
                var command = Command.CreateCommand("!!PRJ = !!projcode()");
                command.Run();
                var proj = command.GetPMLVariableString("PRJ");
                Command.CreateCommand("!!PRJ.Delete()").Run();
                return proj;
            }
        }
    }
}
