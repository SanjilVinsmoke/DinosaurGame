using System;
using System.Collections.Generic;
using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    [CreateAssetMenu(menuName = "Dinosaur Game/Dinosaurs/Dinosaur Catalog", fileName = "DinosaurCatalog")]
    public sealed class DinosaurCatalog : ScriptableObject
    {
        [SerializeField] private DinosaurDefinition starter;
        [SerializeField] private DinosaurDefinition[] definitions = Array.Empty<DinosaurDefinition>();

        private Dictionary<string, DinosaurDefinition> _byId;

        public DinosaurDefinition Starter => starter;

        public bool TryGet(string id, out DinosaurDefinition definition)
        {
            EnsureIndex();
            return !string.IsNullOrWhiteSpace(id) && _byId.TryGetValue(id, out definition);
        }

        private void EnsureIndex()
        {
            if (_byId != null) return;
            _byId = new Dictionary<string, DinosaurDefinition>(StringComparer.Ordinal);
            if (starter != null) _byId[starter.Id] = starter;
            foreach (var item in definitions)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.Id)) _byId[item.Id] = item;
            }
        }

        private void OnEnable() => _byId = null;
        private void OnValidate() => _byId = null;
    }
}
