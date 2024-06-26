using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsManager : Singletons<SoundsManager>
{
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip[] cardFlip;
    [SerializeField] AudioClip[] correctMatchClips;
    [SerializeField] AudioClip[] missMatchClips;
    [SerializeField] AudioClip[] comboClips;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip levelMusic;
    private void Start()
    {
        SubscribeToSoundActions();
    }
    private void SubscribeToSoundActions()
    {
        CardManager.Singleton.OnCardFlipped += OnFlip;

        LevelManager.Singleton.OnLevelStart += OnLevelStarted;

        LevelManager.Singleton.OnMatch += OnMatch;
        LevelManager.Singleton.OnMisMatch += OnMissMatch;

        LevelManager.Singleton.OnWin += OnWin;
        LevelManager.Singleton.OnLose+= OnLose;

        ScoreManager.Singleton.OnCombo += OnCombo;

    }
    private void OnLevelStarted()
    {
        musicAudioSource.clip = levelMusic;
        musicAudioSource.Play();
    }
    private void OnFlip()
    {
        PlaySound(PickRandomClip(cardFlip));
    }
    private void OnMatch()
    {
        PlaySound(PickRandomClip(correctMatchClips));
    }
    private void OnMissMatch(int mistakesCount)
    {
        PlaySound(PickRandomClip(missMatchClips));
    }
    private void OnCombo(int combo)
    {
        if (combo == -1||combo==0) return;
        PlaySound(PickRandomClip(comboClips));
    }
    private void OnWin()
    {
        musicAudioSource.Stop();
        PlaySound(winSound);
    }
    private void OnLose()
    {
        musicAudioSource.Stop();
        PlaySound(loseSound);
    }
    private AudioClip PickRandomClip(AudioClip[]clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
    private void PlaySound(AudioClip clip)
    {
        sfxAudioSource.clip= clip;
        sfxAudioSource.Play();
    }
}
