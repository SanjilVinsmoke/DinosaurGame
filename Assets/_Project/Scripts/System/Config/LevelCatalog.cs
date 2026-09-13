using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dinosaur Game/Legacy/Level Catalog", fileName = "LevelCatalog")]
public sealed class LevelCatalog : ScriptableObject
{
    [SerializeField] private Level[] levels = Array.Empty<Level>();

    public int Count => levels?.Length ?? 0;

    public Level Get(int oneBasedIndex)
    {
        if (levels == null || oneBasedIndex <= 0 || oneBasedIndex > levels.Length)
            return null;
        return levels[oneBasedIndex - 1];
    }
}
