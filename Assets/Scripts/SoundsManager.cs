using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : Singletons<SoundsManager>
{
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip[] cardFlip;
    [SerializeField] AudioClip[] correctMatchClips;
    [SerializeField] AudioClip[] missMatchClips;
    [SerializeField] AudioClip[] comboClips;
    [SerializeField] AudioClip[] clickOnMenuButtons;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;

    protected override void Awake()
    {
        base.Awake();
        SubscribeToSoundActions();
    }
    private void SubscribeToSoundActions()
    {
        Card.OnCardStartedFlip += OnFlip;
        Card.OnMatching += OnMatch;
        Card.OnMissMatching += OnMissMatch;
        ScoreManager.Singleton.OnCombo += OnCombo;

        LevelManager.Singleton.OnWin += OnWin;
        LevelManager.Singleton.OnLose+= OnLose;
    }
    private void OnFlip(Card card)
    {
        PlaySound(PickRandomClip(cardFlip));
    }
    private void OnMatch()
    {
        PlaySound(PickRandomClip(correctMatchClips));
    }
    private void OnMissMatch()
    {
        PlaySound(PickRandomClip(missMatchClips));
    }
    private void OnCombo()
    {
        PlaySound(PickRandomClip(comboClips));
    }
    private void OnClick()
    {
        PlaySound(PickRandomClip(clickOnMenuButtons));
    }
    private void OnWin()
    {
        PlaySound(winSound);
    }
    private void OnLose()
    {
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
