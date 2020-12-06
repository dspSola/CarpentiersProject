using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;

    [SerializeField] int totalCollectibles;
    int collectiblesOnDeath;
    [SerializeField] float percentageCollected;
    float percentageCalc;
    [SerializeField] GameObject finishPanel;
    bool nextLevel = false;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else { DontDestroyOnLoad(gameObject); }
    }

    void Start ()
    {
        liveText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void CountCollectibles()
    {
        totalCollectibles++;
        Debug.Log("Food" + totalCollectibles);
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        collectiblesOnDeath++;
        scoreText.text = score.ToString();
    }

    public float GetPercentageProgress()
    {
        Debug.Log(totalCollectibles);
        if (totalCollectibles == 0) { totalCollectibles = FindObjectsOfType<Collectible>().Length; }
        percentageCalc = (float)score * 0.1f / (float)totalCollectibles;
        percentageCalc *= 100;
        percentageCollected = Mathf.RoundToInt(percentageCalc);
        Debug.Log(percentageCollected+" %");
        return percentageCollected;
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            
            StartCoroutine(TakeLife());
        }
        else {StartCoroutine(ResetGameSession()); }
    }

    public IEnumerator ResetGameSession()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private IEnumerator TakeLife()
    {
        playerLives--;
        liveText.text = playerLives.ToString();
        yield return new WaitForSeconds(2);
        totalCollectibles = collectiblesOnDeath / 10;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int SendScore()
    {
        return score;
    }

    //public void SetUpStars() //SCORING
    //{
    //    if (!finishPanel) { finishPanel = FindObjectOfType<Stars>().transform.parent.gameObject; }
    //    finishPanel.gameObject.SetActive(true);
    //    FindObjectOfType<Stars>().ActivePanel(true);
    //    if (percentageCollected >= 25) { nextLevel = true; }
    //    Debug.Log("WON " + nextLevel);
    //}

    public bool NextLevelGranted()
    {
        return nextLevel;
    }


    public void CloseTabAndContinue()
    {
        PlayerPrefs.SetInt("Score", score);
        score = 0;
        totalCollectibles = 0;
        finishPanel.gameObject.SetActive(false);
        Debug.Log("FINISH :" + totalCollectibles);
    }
}
