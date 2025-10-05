using UnityEngine;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip flipClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip mismatchClip;
    [SerializeField] private AudioClip gameOverClip;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [SerializeField] private int audioPoolSize = 5;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private int currentSourceIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < audioPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.volume = volume;
            audioSources.Add(src);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource src = audioSources[currentSourceIndex];
        src.clip = clip;
        src.volume = volume;
        src.Play();

        currentSourceIndex = (currentSourceIndex + 1) % audioSources.Count;
    }

    // 🔊 Public Methods
    public void PlayFlip() => PlayClip(flipClip);
    public void PlayMatch() => PlayClip(matchClip);
    public void PlayMismatch() => PlayClip(mismatchClip);
    public void PlayGameOver() => PlayClip(gameOverClip);

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        foreach (var src in audioSources)
            src.volume = volume;
    }
}
