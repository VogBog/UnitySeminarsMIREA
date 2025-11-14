using Data;
using UnityEngine;

namespace Damage
{
    public ref struct GetDamageData
    {
        public int Damage;
        public GameObject Attacker;
        public ElementalData Elemental;
        public bool IsInternal;

        public GetDamageData(int damage, GameObject attacker, ElementalData elemental, bool isInternal)
        {
            Damage = damage;
            Attacker = attacker;
            Elemental = elemental;
            IsInternal = isInternal;
        }
    }
}