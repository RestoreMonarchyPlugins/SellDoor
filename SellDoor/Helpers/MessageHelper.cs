using Rocket.API;
using Rocket.Unturned.Chat;

namespace RestoreMonarchy.SellDoor.Helpers
{
    public class MessageHelper
    {
        private static SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public static void Send(IRocketPlayer player, string translationKey, params object[] placeholder)
        {
            ThreadHelper.RunSynchronously(() =>
            {
                UnturnedChat.Say(player, pluginInstance.Translate(translationKey, placeholder).Replace("{", "<").Replace("}", ">"), pluginInstance.MessageColor, true);
            });            
        }

        public static void Send(string translationKey, params object[] placeholder)
        {
            ThreadHelper.RunSynchronously(() =>
            {
                UnturnedChat.Say(pluginInstance.Translate(translationKey, placeholder).Replace("{", "<").Replace("}", ">"), pluginInstance.MessageColor, true);
            });            
        }
    }
}
