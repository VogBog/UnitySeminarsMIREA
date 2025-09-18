using Data;

namespace Lobby
{
    public struct PlayerData
    {
        public int Index;
        public ElementalData Data;

        public PlayerData(int index, ElementalData data)
        {
            Index = index;
            Data = data;
        }
    }
}