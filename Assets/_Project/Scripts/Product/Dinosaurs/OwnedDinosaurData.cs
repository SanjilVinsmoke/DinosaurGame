using System;
using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    [Serializable]
    public sealed class OwnedDinosaurData
    {
        public string uniqueId;
        public string definitionId;
        public string displayName;
        public int bondLevel = 1;
        public int bondXp;
        public float hunger = 100f;
        public float happiness = 100f;
        public float cleanliness = 100f;
        public float energy = 100f;
        public long lastUpdatedUtcTicks;

        public static OwnedDinosaurData CreateStarter(DinosaurDefinition definition)
        {
            var now = DateTime.UtcNow;
            return new OwnedDinosaurData
            {
                uniqueId = Guid.NewGuid().ToString("N"),
                definitionId = definition.Id,
                displayName = definition.DisplayName,
                bondLevel = definition.StartingBondLevel,
                bondXp = 0,
                hunger = 100f,
                happiness = 100f,
                cleanliness = 100f,
                energy = 100f,
                lastUpdatedUtcTicks = now.Ticks
            };
        }

        public void ClampNeeds()
        {
            hunger = Mathf.Clamp(hunger, 0f, 100f);
            happiness = Mathf.Clamp(happiness, 0f, 100f);
            cleanliness = Mathf.Clamp(cleanliness, 0f, 100f);
            energy = Mathf.Clamp(energy, 0f, 100f);
            bondLevel = Mathf.Max(1, bondLevel);
            bondXp = Mathf.Max(0, bondXp);
        }
    }
}
