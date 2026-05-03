using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    
    [Header("Canvas GO")]
    [SerializeField] private GameObject welcomeGO;
    [SerializeField] private GameObject victoryGO;
    [SerializeField] private GameObject victorytextGO;
    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private GameObject hudGO;

    [Header("Timer")]
    private float time = 300f;
    [SerializeField] private TextMeshProUGUI stopwatch;
    [SerializeField] private TextMeshProUGUI victory;

    private void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(Welcome());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        
        time -= Time.deltaTime;
        stopwatch.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
        if (time <= 0) GameOver();
    }

    public void Win()
    {
        Time.timeScale = 0;
        hudGO.SetActive(false);
        victoryGO.SetActive(true);
        victorytextGO.SetActive(true);
        victory.text += "\n" + (300f - time).ToString("F2");
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        gameOverGO.SetActive(true);
    }

    private IEnumerator Welcome()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        audioManager.GameStart();
        welcomeGO.SetActive(false);
        hudGO.SetActive(true);
        Time.timeScale = 1;
    }
}
