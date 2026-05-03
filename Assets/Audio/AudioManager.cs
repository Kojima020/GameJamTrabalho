using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----Audio Source ----")]
    [SerializeField] private AudioSource welcomeSource;
    [SerializeField] private AudioSource musicSource;

    [Header("----Audio Clips ----")]
    [SerializeField] private AudioClip welcome;
    [SerializeField] private AudioClip background;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip checkpoint;
    [SerializeField] private AudioClip wallTouch;
    [SerializeField] private AudioClip grapplingGun;
    [SerializeField] private AudioClip grapplingHook;
    [SerializeField] private AudioClip throwHat;
    
    
    public void GameStart()
    {
        musicSource.Play();
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        musicSource.volume = 0;
        for (float volume = 0; volume < 1; volume += Time.deltaTime * 0.5f)
        {
            musicSource.volume = volume;
            welcomeSource.volume = 1f - volume;
            yield return null;
        }
        
        musicSource.volume = 1;
        welcomeSource.Stop();
    }
}
