using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Persist Between Scenes")]
    public bool dontDestroyOnLoad = true;

    [Header("Volume")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Sound Effects")]
    public SoundEffect[] soundEffects;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip[] clips;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
        [Range(0f, 0.3f)] public float pitchVariation = 0.05f;
    }

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private Dictionary<string, SoundEffect> sfxDict = new Dictionary<string, SoundEffect>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        foreach (var sfx in soundEffects)
            if (!string.IsNullOrEmpty(sfx.name))
                sfxDict[sfx.name] = sfx;

        if (backgroundMusic != null)
            PlayMusic(backgroundMusic);
    }

    public void PlaySFX(string sfxName)
    {
        if (!sfxDict.TryGetValue(sfxName, out SoundEffect sfx))
        {
            Debug.LogWarning($"[AudioManager] SFX not found: '{sfxName}'");
            return;
        }

        if (sfx.clips == null || sfx.clips.Length == 0) return;

        AudioClip clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
        float finalVol = sfx.volume * sfxVolume;
        float finalPitch = sfx.pitch + Random.Range(-sfx.pitchVariation, sfx.pitchVariation);

        sfxSource.pitch = finalPitch;
        sfxSource.PlayOneShot(clip, finalVol);
    }

    public void PlayAccelerator() => PlaySFX("Accelerator");
    public void PlayBoost() => PlaySFX("BoostVelocity");
    public void PlayCheckpoint() => PlaySFX("Checkpoint");
    public void PlayEnemyShot() => PlaySFX("EnemyShot");
    public void PlayShield() => PlaySFX("Shield");
    public void PlayEnemyDeath() => PlaySFX("EnemyDeath");
    public void PlayLoseRings() => PlaySFX("LoseRings");
    public void PlaySpikes() => PlaySFX("Spikes");
    public void PlayPickEmerald() => PlaySFX("PickEmerald");
    public void PlayPickRings() => PlaySFX("PickRings");
    public void PlayRunning() => PlaySFX("Running");
    public void PlayJump() => PlaySFX("Jump");
    public void PlayPowerUp() => PlaySFX("PowerUp");
    public void PlayTrampolines() => PlaySFX("Trampolines");
    public void PlayWalking() => PlaySFX("Walking");

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value) => sfxVolume = Mathf.Clamp01(value);
}