using CustomInspector;
using UnityEngine;

public class LevelController : SingletonDontDestroy<LevelController>
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelCatalog levelCatalog;
    [ReadOnly] public Level currentLevel;

    public bool PrepareLevel()
    {
        return GenerateLevel(Data.PlayerData.CurrentLevelIndex);
    }

    public bool GenerateLevel(int indexLevel)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }

        Level level = GetLevelByIndex(indexLevel);
        if (level == null)
        {
            Debug.LogWarning($"[LegacyPrototype] No explicit level reference for level {indexLevel}. Legacy level generation skipped.", this);
            return false;
        }

        currentLevel = Instantiate(level);
        currentLevel.gameObject.SetActive(false);
        currentLevel.name = indexLevel > levelConfig.maxLevel ? $"Level {indexLevel} - {currentLevel.name}" : $"Level {indexLevel}";
        return true;
    }

    public Level GetLevelByIndex(int indexLevel)
    {
        if (indexLevel >= levelConfig.maxLevel)
        {
            switch (levelConfig.levelLoopType)
            {
                case LevelLoopType.Recycle:
                    indexLevel = (indexLevel - levelConfig.startLoopLevel) % (levelConfig.maxLevel - levelConfig.startLoopLevel + 1) + levelConfig.startLoopLevel;
                    break;
                case LevelLoopType.Random:
                    indexLevel = Random.Range(1, levelConfig.maxLevel + 1);
                    break;
            }
        }
        else
        {
            indexLevel = (indexLevel - 1) % levelConfig.maxLevel + 1;
        }

        if (levelCatalog == null)
            return null;

        return levelCatalog.Get(indexLevel);
    }
}