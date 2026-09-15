using System;
using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    public sealed class DinosaurSaveRepository
    {
        private const string Key = "DinosaurGame.ActivePet.v1";

        public bool TryLoad(out DinosaurSaveData save)
        {
            save = null;
            if (!PlayerPrefs.HasKey(Key)) return false;

            var json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrWhiteSpace(json)) return false;

            try
            {
                save = JsonUtility.FromJson<DinosaurSaveData>(json);
                return save != null && save.activePet != null;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to load dinosaur save: {exception.Message}");
                return false;
            }
        }

        public void Save(OwnedDinosaurData pet)
        {
            if (pet == null) return;
            pet.lastUpdatedUtcTicks = DateTime.UtcNow.Ticks;
            var data = new DinosaurSaveData { activePet = pet };
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
