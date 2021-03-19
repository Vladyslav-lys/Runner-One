using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool levelMaker;
    public int level;
    public GameObject[] levels;
    public UIManager uiManager;
    public TargetCamera targetCamera;
    public List<Color> colors;
    public int currentPow;
    public int coins;
    public int needMagnettingTime;
    public int currentColorIndex;
    public long magnettingTimeTicks;
    public bool isStarted;
    public bool isMagnetting;
    public List<Material> levelSkyboxes;
    public LevelGenerator levelGenerator;
    public int needPow;
    public PlayerController playerController;
    private static readonly int SkyGradientTop = Shader.PropertyToID("_SkyGradientTop");

    public delegate void AdditionActions();
    
    private void Awake()
    {
        levelGenerator = GetComponent<LevelGenerator>();
        
        InitPlayerPrefs();

        if (IsLevelScene())
        {
            if (levelMaker) 
                PlayerPrefs.SetInt("Level", level);
        
            levels[PlayerPrefs.GetInt("Level") - 1].GetComponent<Level>().gm = this;
            GameObject.Instantiate(levels[PlayerPrefs.GetInt("Level") - 1]);
        }
        
        RenderSettings.skybox = levelSkyboxes[Random.Range(0, levelSkyboxes.Count)];
        RenderSettings.fogColor = RenderSettings.skybox.GetColor(SkyGradientTop);
        //DynamicGI.UpdateEnvironment();
    }

    private void Start()
    {
        Time.timeScale = 1;
        coins = 0;
        uiManager.coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
        uiManager.levelCoinsText.text = "0";
    }

    public void StartPlay()
    {
        isStarted = true;
        levelGenerator.StartPlay();
    }

    public void SetWin(PlayerMovement playerMovement)
    {
        isStarted = false;
        StopGame(playerMovement.StopMovement, playerController.SetFinish);
        targetCamera.transform.localPosition = 
            new Vector3(playerMovement.boxTransform.localPosition.x, targetCamera.transform.localPosition.y, targetCamera.transform.localPosition.z);
        targetCamera.target = playerMovement.boxTransform;
        Invoke(nameof(SetFinishUI), 1f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + PlayerPrefs.GetInt("Level"));
    }

    public void SetFinishUI()
    {
        uiManager.Finish(coins, currentPow, currentColorIndex);
        if (PlayerPrefs.GetInt("HighScore") < playerController.count)
        {
            uiManager.newHighScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("HighScore", playerController.count);
        }
        PlayerPrefs.SetInt("Coins", coins + PlayerPrefs.GetInt("Coins"));
    }
    
    public void SetLose(PlayerMovement playerMovement)
    {
        isStarted = false;
        StopGame(playerMovement.Obstacle, playerMovement.DisableScript);
        Invoke(nameof(SetLoseUI), 1f);
    }

    public void SetLoseUI() => uiManager.Lose();
    
    public void SetInfinityLoseUI(PlayerMovement playerMovement)
    {
        isStarted = false;
        StopGame(playerMovement.Obstacle, playerMovement.SetStandartTransform, playerMovement.DisableScript);
        Invoke(nameof(SetFinishUI), 1f);
    }
    
    public void SetInfinityLoseOnLavaUI(PlayerMovement playerMovement)
    {
        isStarted = false;
        targetCamera.enabled = false;
        StopGame(playerMovement.LowVelocity, playerMovement.SetFalling, playerMovement.DisableScript);
        Invoke(nameof(SetFinishUI), 1f);
    }

    public void StopGame(params AdditionActions[] additionActions)
    {
        levelGenerator.StopGenerate();
        uiManager.dragCanvas.SetActive(false);
        foreach (var additionAction in additionActions)
        {
            additionAction();
        }
    }
    
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void GoToNextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        if(PlayerPrefs.GetInt("Level") > 10)
            PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public bool IsLevelScene() => SceneManager.GetActiveScene().name == "LevelScene";
    
    public bool IsInfinityScene() => SceneManager.GetActiveScene().name == "InfinityScene";
    
    private void InitPlayerPrefs()
    {
        if(!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        
        if(!PlayerPrefs.HasKey("Audio"))
            PlayerPrefs.SetInt("Audio", 1);

        if (!PlayerPrefs.HasKey("IsVibration"))
            PlayerPrefs.SetInt("IsVibration", 1);
        
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);
    }
}
