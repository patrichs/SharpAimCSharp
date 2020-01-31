namespace SharpAimWpf.Memory
{
    class Base
    {
        public enum MainOffsets
        {
            BaseEntity = 0xA6C49C,
            ViewMatrix = 0x4A4C4A4,
            EntityList = 0x4A5A8E4,
            EnginePtr = 0x6062B4,
            LocalPlayer = 0xA6C49C,
            RadarBase = 0x4A8F58C,
            ForceFire = 0x2EC6938,
        }

        public enum StructSizes
        {
            GlowStruct = 0x38,
            EntityStruct = 0x10,
            RadarStruct = 0x24
        }

        public enum EntityOffsets
        {
            Spotted = 0x935,
            Position = 0x134,
            Rotation = 0x128,
            Velocity = 0x110,
            Team = 0xF0,
            Armor = 0xA8A4,
            Health = 0xFC,
            Dormant = 0xE9,
            Index = 0x64,
            Flags = 0x100,
            LifeState = 0x25B,
            CrosshairId = 0xC550,
            ViewAngles = 0x4D0C,
            ActiveWeapon = 0x4AF8,
            CrosshairY = 0x4DCC, // 4E98
            CrosshairX = 0x4DD0, // 4E9C
            RadarBasePointer = 0x50
        }
    }
}
