using System;
using System.Linq;
using Binarysharp.MemoryManagement;
using SharpAimWpf.Memory;

namespace SharpAimWpf.SharpAim
{
    public static class General
    {
        /// <summary>
        /// Returns the Base Address of the Remote Module specified in remoteModule that exists within the MemorySharp object.
        /// </summary>
        /// <param name="process">The MemorySharp object</param>
        /// <param name="remoteModule">The name of the Remote Module, e.g. "client.dll", "engine.dll"...</param>
        /// <returns>(int) The Base Address</returns>
        public static int GetModuleBaseAddr(MemorySharp process, string remoteModule)
        {
            return process.Modules.RemoteModules.Where(module => string.Equals(module.Name, remoteModule)).Select(module => module.BaseAddress.ToInt32()).FirstOrDefault();
        }

        public static int GetPlayers(MemorySharp process) // TODO:
        {
            return !process.IsRunning ? 0 : process["engine.dll"].Read<int>(0x0);
        }

        public enum Team
        {
            Spectator = 1,
            Terrorist = 2,
            CounterTerrorist = 3
        }

        /// <summary>
        /// Gets the Team Name of the supplied Team ID
        /// </summary>
        /// <param name="teamId">The Team ID (1 = Spectator, 2 = T, 3 = CT)</param>
        /// <returns>The Team Name (string)</returns>
        public static string GetTeamName(int teamId)
        {
            return Enum.GetName(typeof(Team), teamId);
        }

        /// <summary>
        /// Returns the distance between a player and another EntityAddr.
        /// </summary>
        /// <param name="player">A Vector3 object consisting of X, Y and Z coordinates of the Player.</param>
        /// <param name="enemy">A Vector3 object consisting of X, Y and Z coordinates of the EntityAddr.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double GetDistanceToEnemyFromPlayer(Vector3 player, Vector3 enemy)
        {
            return Math.Sqrt(Math.Pow(player.X - enemy.X, 2.0) + Math.Pow(player.Y - enemy.Y, 2.0) + Math.Pow(player.Z - enemy.Z, 2.0));
        }
    }
}
