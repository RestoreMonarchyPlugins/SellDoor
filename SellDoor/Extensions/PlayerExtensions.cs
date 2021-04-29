using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.SellDoor.Extensions
{
    public static class PlayerExtensions
    {
        public static string ID(this Player player)
        {
            return player.channel.owner.playerID.steamID.m_SteamID.ToString();
        }

        public static CSteamID CSteamID(this Player player)
        {
            return player.channel.owner.playerID.steamID;
        }

        public static string DisplayName(this Player player)
        {
            return player?.channel?.owner?.playerID?.characterName ?? string.Empty;
        }

        public static CSteamID GroupID(this Player player)
        {
            return player.channel.owner.playerID.group;
        }
    }
}
