using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Elemental Data", menuName = "Data/Elemental Data")]
    public class ElementalData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}