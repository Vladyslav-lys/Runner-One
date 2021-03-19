using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject levelText;
    public GameObject mainMenu;
    public GameObject finishCanvas;
    public GameObject dragCanvas;
    public GameObject playCanvas;
    public GameObject loseCanvas;
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;
    public GameObject creditsCanvas;
    public GameObject coinUI;
    public GameObject infinityModeBtnObj;
    public GameObject infinityModeUnlockedObj;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI levelCoinsText;
    public TextMeshProUGUI finishCoinsTexts;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI newHighScoreText;
    public Image finishCubeIMG;
    public TextMeshProUGUI[] countText;
    public Image startCubeIMG;
    public TextMeshProUGUI[] startCountText;
    private int _finishCoins;
    public GameManager gm;
    public GameObject tutorial;
    
    void Start()
    {
        CheckMode();
        
        if (gm.IsLevelScene())
        {
            levelText.SetActive(true);
            levelText.GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("Level");
        }

        if (gm.IsInfinityScene())
        {
            highScoreText.text = "High Score: \n" + PlayerPrefs.GetInt("HighScore").ToString();
        }
        
        mainMenu.SetActive(true);
        finishCanvas.SetActive(false);
        playCanvas.SetActive(false);
    }

    public void InitStartCube()
    {
        Play();
        
        if(!gm.IsLevelScene())
            return;
        
        for (int i = 0; i < startCountText.Length; i++)
        {
            startCountText[i].text = Math.Pow(2, gm.needPow).ToString();
        }
        startCubeIMG.color = gm.colors[gm.needPow - 1];
    }
    
    public void UpdateCoinsText()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    
    public void UpdateLevelCoinsText(int coins)
    {
        levelCoinsText.text = coins.ToString();
    }
    
    public void Play()
    {
        mainMenu.SetActive(false);
        playCanvas.SetActive(true);
        
        //PlayerPrefs.DeleteKey("Tutorial");
        if(!PlayerPrefs.HasKey("Tutorial"))
            Invoke(nameof(ShowTutorial),  0.5f);
    }
    
    public void OpenPause()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }
    
    public void ClosePause()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }
    
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsCanvas.SetActive(true);
    }
    
    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }
    
    public void CloseCredits()
    {
        creditsCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }
    
    public void HideInfinityModeWarning()
    {
        infinityModeUnlockedObj.SetActive(false);
    }
    
    public void OnOffVibration(TextMeshProUGUI text)
    {
        if (PlayerPrefs.GetInt("IsVibration") != 0)
        {
            PlayerPrefs.SetInt("IsVibration", 0);
            text.text = "Vibration Off";
        }
        else
        {
            PlayerPrefs.SetInt("IsVibration", 1);
            text.text = "Vibration On";
        }
    }
    
    public void OnOffSound(TextMeshProUGUI text)
    {
        if(PlayerPrefs.GetInt("Audio") != 0)
        {
            PlayerPrefs.SetInt("Audio", 0);
            text.text = "Sound Off";
        }
        else
        {
            PlayerPrefs.SetInt("Audio", 1);
            text.text = "Sound On";
        }
    }
    
    public void Finish(int coins, int pow, int colorIndex)
    {
        for (int i = 0; i < countText.Length; i++)
        {
            countText[i].text = Math.Pow(2, pow).ToString();
        }
        _finishCoins = coins;
        finishCubeIMG.color = gm.colors[colorIndex];
        StartCoroutine(nameof(FinishCoinsCo));
        if(gm.IsLevelScene())
            levelText.SetActive(false);
        playCanvas.SetActive(false);
        finishCanvas.SetActive(true);
    }

    public void Lose()
    {
        levelText.SetActive(false);
        playCanvas.SetActive(false);
        loseCanvas.SetActive(true);
    }

    public void CreateCoinUI(Transform boxTransform)
    {
        Vector3 coinPos = new Vector3(boxTransform.localPosition.x*70f, -100f, 10f);
        GameObject localCoinUI = Instantiate(coinUI, coinPos, Quaternion.identity);
        localCoinUI.GetComponent<CoinUI>().targetTransform = levelCoinsText.transform;
        localCoinUI.transform.SetParent(playCanvas.transform);
        localCoinUI.transform.localPosition = coinPos;
        localCoinUI.transform.localRotation = Quaternion.Euler(Vector3.zero);
        localCoinUI.transform.localScale = Vector3.one;
    }

    private IEnumerator FinishCoinsCo()
    {
        int localCoins = 0;
        for (int i = 0; i <=_finishCoins; i++)
        {
            finishCoinsTexts.text = "+" + localCoins++;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void ShowTutorial()
    {
        tutorial.SetActive(true);
        Time.timeScale = 0f;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void HideTutorial()
    {
        Time.timeScale = 1f;
        tutorial.SetActive(false);
    }
    
    private void CheckMode()
    {
        //PlayerPrefs.DeleteKey("IsInfinityModeOpened");
        if (PlayerPrefs.GetInt("Level") == 5 && !PlayerPrefs.HasKey("IsInfinityModeOpened"))
        {
            PlayerPrefs.SetInt("IsInfinityModeOpened", 1);
            infinityModeUnlockedObj.SetActive(true);
        }

        if(infinityModeBtnObj)
            infinityModeBtnObj.SetActive(PlayerPrefs.HasKey("IsInfinityModeOpened"));
        
    }
}
