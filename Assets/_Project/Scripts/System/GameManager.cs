using CustomTween;
using DinosaurGame.Core;
using DinosaurGame.Gameplay;
using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public LevelController levelController;
    public GameState gameState;
    public GameFlowMode flowMode { get; private set; } = GameFlowMode.Boot;

    protected override void Awake()
    {
        base.Awake();
        AppRuntime.Configure();
        CustomTweenConfig.warnZeroDuration = false;
    }
    
    void Start()
    {
        ReturnHome();
    }

    public void PlayCurrentLevel(bool ignorePrepareLevel = true)
    {
        if (ignorePrepareLevel) PrepareLevel();
        StartGame();
    }

    public void PrepareLevel()
    {
        gameState = GameState.PrepareGame;
        if (levelController == null || !levelController.PrepareLevel())
        {
            Debug.LogWarning("[LegacyPrototype] Level flow is unavailable. Dinosaur product flow remains on Home.", this);
        }
    }

    public void ReturnHome()
    {
        flowMode = GameFlowMode.Home;

        if (SoundController.Instance != null)
            SoundController.Instance.PlayBackground(SoundName.HomeBackgroundMusic);

        if (PopupController.Instance == null)
        {
            Debug.LogError("[Bootstrap] PopupController is missing; cannot present Home.", this);
            return;
        }

        PopupController.Instance.HideAll();
        PopupController.Instance.Show<PopupBackground>();
        PopupController.Instance.Show<PopupHome>();
    }

    public void ReplayGame()
    {
        if (levelController == null) return;
        Observer.ReplayLevel?.Invoke(levelController.currentLevel);
        PrepareLevel();
        StartGame();
    }

    public void BackLevel()
    {
        Data.PlayerData.CurrentLevelIndex--;
        
        PrepareLevel();
        StartGame();
    }

    public void NextLevel()
    {
        if (levelController == null) return;
        Observer.SkipLevel?.Invoke(levelController.currentLevel);
        Data.PlayerData.CurrentLevelIndex++;

        PrepareLevel();
        StartGame();
    }
    
    public void StartGame()
    {
        if (levelController == null || levelController.currentLevel == null)
        {
            Debug.LogWarning("[LegacyPrototype] StartGame ignored because no legacy level is prepared.", this);
            ReturnHome();
            return;
        }

        gameState = GameState.PlayingGame;
        Observer.StartLevel?.Invoke(levelController.currentLevel);
        
        SoundController.Instance.PlayBackground(SoundName.InGameBackgroundMusic);
        PopupController.Instance.HideAll();
        PopupController.Instance.Show<PopupInGame>();
        
        levelController.currentLevel.gameObject.SetActive(true);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.WinGame;
        Observer.WinLevel?.Invoke(levelController.currentLevel);
        Data.PlayerData.CurrentLevelIndex++;
        Sequence.Create().ChainDelay(delayPopupShowTime).ChainCallback(() =>
        {
            PopupController.Instance.HideAll();
            if (PopupController.Instance.Get<PopupWin>() is PopupWin popupWin)
            {
                popupWin.Show();
            }
        });
    }
    
    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.LoseGame;
        Observer.LoseLevel?.Invoke(levelController.currentLevel);
        
        Sequence.Create().ChainDelay(delayPopupShowTime).ChainCallback(() =>
        {
            PopupController.Instance.Hide<PopupInGame>();
            PopupController.Instance.Show<PopupLose>();
        });
    }
}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    WaitingResult,
    LoseGame,
    WinGame,
}