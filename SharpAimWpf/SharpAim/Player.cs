using System;
using System.Linq;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Modules;
using SharpAimWpf.Memory;

namespace SharpAimWpf.SharpAim
{
    class Player
    {
        private readonly MemorySharp _process;
        private readonly Entity _entity;
        public Vector3 Position = new Vector3();

        public int LocalPlayer;
        public int Health;
        readonly RemoteModule _clientModule;

        public byte IsInCrosshair;
        public byte ActiveWeapon;
        public byte Team;

        public bool IsTriggerEnabled = true;

        public Player(MemorySharp process)
        {
            _process = process;

            foreach (var module in _process.Modules.RemoteModules.Where(module => string.Equals(module.Name, "client.dll")))
            {
                _clientModule = module;
            }

            _entity = new Entity(_process);
        }

        public void Update()
        {
            if (!_process.IsRunning)
                throw new SharpAimException("Game is not running, please start the game.");

            // Get the LocalPlayer address
            LocalPlayer = _process["client.dll"].Read<int>(Base.MainOffsets.LocalPlayer);

            // Update
            Health = _process.Read<int>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.Health, false);
            ActiveWeapon = _process.Read<byte>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.ActiveWeapon, false);
            Team = _process.Read<byte>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.Team, false);
            Position.X = _process.Read<float>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.Position, false);
            Position.Z = _process.Read<float>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.Position + 0x04, false);
            Position.Y = _process.Read<float>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.Position + 0x08, false);
            IsInCrosshair = _process.Read<byte>(new IntPtr(LocalPlayer) + (int) Base.EntityOffsets.CrosshairId, false);
        }

        public bool TriggerBot()
        {
            if (!IsTriggerEnabled)
                return false;

            var address = _clientModule.BaseAddress.ToInt32() + (int)Base.MainOffsets.ForceFire;

            if (IsInCrosshair == 0)
            {
                if (_process.Read<byte>(new IntPtr(address), false) == 5)
                    _process.Write(new IntPtr(address), 4, false);
            }

            if (IsInCrosshair > 0 && IsInCrosshair < 32)
            {
                _entity.SingleEntity(IsInCrosshair);

                if (Team != _entity.Team)
                {
                    _process.Write(new IntPtr(address), 5, false);
                    return true;
                }
            }

            return false;
        }
    }
}