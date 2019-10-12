using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellDoor.Commands
{
    public class CostDoorCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "selldoor";

        public string Help => "Puts the door you are looking at on sale";

        public string Syntax => "<price>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
