using System;
using System.Collections.Generic;
using System.Text;
using Binarysharp.MemoryManagement;
using SharpAimWpf.Memory;

namespace SharpAimWpf.SharpAim
{
    class Entity
    {
        public Vector3 Position = new Vector3();
        public int Health;
        public byte ActiveWeapon;
        public byte Team;
        public byte Dormant;
        public byte Spotted;
        public string Name;

        private readonly MemorySharp _process;
        public int EntityAddr;
        public int RadarNameAddress;

        public Entity(MemorySharp process)
        {
            _process = process;
        }

        public void SingleEntity(int entityId)
        {
            if (!_process.IsRunning)
                throw new SharpAimException("Game is not running, please start the game.");

            try
            {
                // Get the address for our Entities
                EntityAddr = _process["client.dll"].Read<int>(Base.MainOffsets.EntityList + ((entityId - 1) * 0x10));

                // Update
                Health = _process.Read<int>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Health, false);
                ActiveWeapon = _process.Read<byte>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.ActiveWeapon, false);
                Team = _process.Read<byte>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Team, false);
                Dormant = _process.Read<byte>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Dormant, false);
                Spotted = _process.Read<byte>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Spotted, false);
                Name = GetPlayerName(entityId);

                Position.X = _process.Read<float>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Position, false);
                Position.Z = _process.Read<float>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Position + 0x04, false);
                Position.Y = _process.Read<float>(new IntPtr(EntityAddr) + (int)Base.EntityOffsets.Position + 0x08, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught: {e}");
            }
        }

        public static List<Entity> AllEntities(MemorySharp process)
        {
            var entities = new List<Entity>();

            for (var i = 0; i < 64; i++)
            {
                var obj = new Entity(process);
                obj.SingleEntity(i);

                if (obj.EntityAddr == 0)
                    continue;

                entities.Add(obj);
            }

            return entities;
        } 

        private string GetPlayerName(int entityId) // Works but should create a new Object every time instead of using an update function. TODO:
        {
            var radarBase = _process["client.dll"].Read<int>(Base.MainOffsets.RadarBase);
            var radarBasePointer = _process.Read<int>(new IntPtr(radarBase + (int)Base.EntityOffsets.RadarBasePointer), false);

            RadarNameAddress = radarBasePointer + (0x1E0*(entityId + 1)) + (int)Base.StructSizes.RadarStruct;

            return _process.ReadString(radarBasePointer + (0x1E0 * (entityId + 1)) + Base.StructSizes.RadarStruct,
                Encoding.Unicode, false, 64);
        }
    }
}
