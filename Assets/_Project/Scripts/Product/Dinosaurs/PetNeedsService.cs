using System;

namespace DinosaurGame.Product.Dinosaurs
{
    public static class PetNeedsService
    {
        public static void ApplyOfflineProgress(OwnedDinosaurData pet, DinosaurDefinition definition, DateTime utcNow)
        {
            if (pet == null || definition == null) return;

            var last = pet.lastUpdatedUtcTicks > 0
                ? new DateTime(pet.lastUpdatedUtcTicks, DateTimeKind.Utc)
                : utcNow;
            var hours = Math.Max(0d, (utcNow - last).TotalHours);

            pet.hunger -= definition.HungerDecayPerHour * (float)hours;
            pet.happiness -= definition.HappinessDecayPerHour * (float)hours;
            pet.cleanliness -= definition.CleanlinessDecayPerHour * (float)hours;
            pet.energy += definition.EnergyRecoveryPerHour * (float)hours;
            pet.lastUpdatedUtcTicks = utcNow.Ticks;
            pet.ClampNeeds();
        }

        public static void Feed(OwnedDinosaurData pet, float hunger, int bondXp)
        {
            if (pet == null) return;
            pet.hunger += hunger;
            AddBondXp(pet, bondXp);
            pet.ClampNeeds();
        }

        public static void Pet(OwnedDinosaurData pet, float happiness, int bondXp)
        {
            if (pet == null) return;
            pet.happiness += happiness;
            AddBondXp(pet, bondXp);
            pet.ClampNeeds();
        }

        public static void Wash(OwnedDinosaurData pet, float cleanliness, int bondXp)
        {
            if (pet == null) return;
            pet.cleanliness += cleanliness;
            AddBondXp(pet, bondXp);
            pet.ClampNeeds();
        }

        public static void Play(OwnedDinosaurData pet, float happiness, float energyCost, int bondXp)
        {
            if (pet == null) return;
            pet.happiness += happiness;
            pet.energy -= energyCost;
            AddBondXp(pet, bondXp);
            pet.ClampNeeds();
        }

        public static bool AddBondXp(OwnedDinosaurData pet, int amount)
        {
            if (pet == null || amount <= 0) return false;
            pet.bondXp += amount;
            var leveled = false;
            while (pet.bondXp >= XpForLevel(pet.bondLevel))
            {
                pet.bondXp -= XpForLevel(pet.bondLevel);
                pet.bondLevel++;
                leveled = true;
            }
            return leveled;
        }

        public static int XpForLevel(int level) => 50 + Math.Max(0, level - 1) * 25;
    }
}
