using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    [CreateAssetMenu(menuName = "Dinosaur Game/Dinosaurs/Dinosaur Definition", fileName = "DinosaurDefinition")]
    public sealed class DinosaurDefinition : ScriptableObject
    {
        [SerializeField] private string id = "starter_triceratops";
        [SerializeField] private string displayName = "Triceratops";
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite portrait;
        [SerializeField] private int startingBondLevel = 1;
        [SerializeField] private float hungerDecayPerHour = 4f;
        [SerializeField] private float happinessDecayPerHour = 2f;
        [SerializeField] private float cleanlinessDecayPerHour = 2.5f;
        [SerializeField] private float energyRecoveryPerHour = 6f;

        public string Id => id;
        public string DisplayName => displayName;
        public GameObject Prefab => prefab;
        public Sprite Portrait => portrait;
        public int StartingBondLevel => Mathf.Max(1, startingBondLevel);
        public float HungerDecayPerHour => Mathf.Max(0f, hungerDecayPerHour);
        public float HappinessDecayPerHour => Mathf.Max(0f, happinessDecayPerHour);
        public float CleanlinessDecayPerHour => Mathf.Max(0f, cleanlinessDecayPerHour);
        public float EnergyRecoveryPerHour => Mathf.Max(0f, energyRecoveryPerHour);

        private void OnValidate()
        {
            id = string.IsNullOrWhiteSpace(id) ? name : id.Trim();
            displayName = string.IsNullOrWhiteSpace(displayName) ? name : displayName.Trim();
        }
    }
}
