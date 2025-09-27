using Data;
using UnityEngine.InputSystem;

namespace Lobby
{
    public struct PlayerData
    {
        public int Index;
        public InputDevice Device;
        public ElementalData Data;

        public PlayerData(int index, InputDevice device, ElementalData data)
        {
            Index = index;
            Device = device;
            Data = data;
        }
    }
}