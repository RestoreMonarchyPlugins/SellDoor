using Rocket.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.SellDoor.Helpers
{
    internal static class UconomyHelper
    {
        internal static bool IsUconomyInstalled()
        {
            return R.Plugins.GetPlugin("Uconomy") != null;
        }

        internal static void IncreaseBalance(string playerId, decimal amount)
        {
            if (!IsUconomyInstalled())
            {
                return;
            }

            fr34kyn01535.Uconomy.Uconomy.Instance.Database.IncreaseBalance(playerId, amount);
        }

        internal static decimal GetBalance(string playerId)
        {
            if (!IsUconomyInstalled())
            {
                return 0;
            }

            return fr34kyn01535.Uconomy.Uconomy.Instance.Database.GetBalance(playerId);
        }
    }
}
