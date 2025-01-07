using Rocket.Core;

namespace RestoreMonarchy.SellDoor.Helpers
{
    internal static class UconomyHelper
    {
        internal static bool IsUconomyInstalled()
        {
            return R.Plugins.GetPlugin("Uconomy") != null;
        }

        internal static decimal IncreaseBalance(string playerId, decimal amount)
        {
            if (!IsUconomyInstalled())
            {
                throw new System.Exception("Uconomy plugin is not installed on this server.");
            }

            return fr34kyn01535.Uconomy.Uconomy.Instance.Database.IncreaseBalance(playerId, amount);
        }

        internal static decimal GetBalance(string playerId)
        {
            if (!IsUconomyInstalled())
            {
                throw new System.Exception("Uconomy plugin is not installed on this server.");
            }

            return fr34kyn01535.Uconomy.Uconomy.Instance.Database.GetBalance(playerId);
        }
    }
}
