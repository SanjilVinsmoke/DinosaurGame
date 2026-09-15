using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    public enum PetCareItemType
    {
        Food,
        Toy
    }

    [CreateAssetMenu(menuName = "Dinosaur Game/Dinosaurs/Pet Care Item", fileName = "PetCareItem")]
    public sealed class PetCareItemDefinition : ScriptableObject
    {
        [SerializeField] private string id = "item";
        [SerializeField] private string displayName = "Item";
        [SerializeField] private PetCareItemType type;
        [SerializeField] private float hungerGain;
        [SerializeField] private float happinessGain;
        [SerializeField] private float energyCost;
        [SerializeField] private int bondXp = 5;

        public string Id => id;
        public string DisplayName => displayName;
        public PetCareItemType Type => type;
        public float HungerGain => Mathf.Max(0f, hungerGain);
        public float HappinessGain => Mathf.Max(0f, happinessGain);
        public float EnergyCost => Mathf.Max(0f, energyCost);
        public int BondXp => Mathf.Max(0, bondXp);

        private void OnValidate()
        {
            id = string.IsNullOrWhiteSpace(id) ? name : id.Trim();
            displayName = string.IsNullOrWhiteSpace(displayName) ? name : displayName.Trim();
        }
    }
}
