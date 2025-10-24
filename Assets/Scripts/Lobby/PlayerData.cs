using Data;
using UnityEngine.InputSystem;

namespace Lobby
{
    public struct PlayerData
    {
        public bool IsActive;
        public int Index;
        public InputDevice Device;
        public ElementalData Data;

        public PlayerData(bool isActive, int index, InputDevice device, ElementalData data)
        {
            IsActive = isActive;
            Index = index;
            Device = device;
            Data = data;
        }
    }
}