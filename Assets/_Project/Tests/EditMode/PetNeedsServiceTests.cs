using System;
using DinosaurGame.Product.Dinosaurs;
using NUnit.Framework;
using UnityEngine;

namespace DinosaurGame.Tests.EditMode
{
    public sealed class PetNeedsServiceTests
    {
        [Test]
        public void CareActionsClampNeedsAndAdvanceBond()
        {
            var pet = new OwnedDinosaurData
            {
                bondLevel = 1,
                bondXp = 48,
                hunger = 95f,
                happiness = 95f,
                cleanliness = 95f,
                energy = 5f,
                lastUpdatedUtcTicks = DateTime.UtcNow.Ticks
            };

            PetNeedsService.Feed(pet, 20f, 5);

            Assert.That(pet.hunger, Is.EqualTo(100f));
            Assert.That(pet.bondLevel, Is.EqualTo(2));
            Assert.That(pet.bondXp, Is.EqualTo(3));
        }

        [Test]
        public void OfflineProgressUsesElapsedUtcTime()
        {
            var definition = ScriptableObject.CreateInstance<DinosaurDefinition>();
            var now = DateTime.UtcNow;
            var pet = new OwnedDinosaurData
            {
                hunger = 100f,
                happiness = 100f,
                cleanliness = 100f,
                energy = 0f,
                lastUpdatedUtcTicks = now.AddHours(-2).Ticks
            };

            PetNeedsService.ApplyOfflineProgress(pet, definition, now);

            Assert.That(pet.hunger, Is.LessThanOrEqualTo(100f));
            Assert.That(pet.happiness, Is.LessThanOrEqualTo(100f));
            Assert.That(pet.cleanliness, Is.LessThanOrEqualTo(100f));
            Assert.That(pet.energy, Is.GreaterThanOrEqualTo(0f));
            UnityEngine.Object.DestroyImmediate(definition);
        }
    }
}
