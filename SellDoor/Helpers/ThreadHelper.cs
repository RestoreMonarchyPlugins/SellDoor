using Rocket.Core.Logging;
using Rocket.Core.Utils;
using SDG.Unturned;
using System;
using System.Threading;

namespace RestoreMonarchy.SellDoor.Helpers
{
    internal static class ThreadHelper
    {
        internal static void RunAsynchronously(System.Action action, string exceptionMessage = null)
        {
            if (Thread.CurrentThread != ThreadUtil.gameThread)
            {
                action.Invoke();
                return;
            }

            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    RunSynchronously(() => Logger.LogException(e, exceptionMessage));
                }
            });
        }

        internal static void RunSynchronously(System.Action action, float delaySeconds = 0)
        {
            if (Thread.CurrentThread == ThreadUtil.gameThread)
            {
                action.Invoke();
                return;
            }

            TaskDispatcher.QueueOnMainThread(action, delaySeconds);
        }
    }
}
