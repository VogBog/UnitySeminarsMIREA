using Data;
using UnityEngine;

namespace Damage
{
    public ref struct GetDamageData
    {
        public int Damage;
        public GameObject Attacker;
        public ElementalData Elemental;

        public GetDamageData(int damage, GameObject attacker, ElementalData elemental)
        {
            Damage = damage;
            Attacker = attacker;
            Elemental = elemental;
        }
    }
}