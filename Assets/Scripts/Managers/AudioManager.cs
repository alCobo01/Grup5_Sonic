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

    [Header("SFX 3D Settings")]
    public float minDistance = 1f;
    public float maxDistance = 20f;

    [Header("Sound Effects")]
    public SoundEffect[] soundEffects;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
    }

    private AudioSource musicSource;
    private Dictionary<string, SoundEffect> sfxDict = new Dictionary<string, SoundEffect>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        musicSource.spatialBlend = 0f;

        foreach (var sfx in soundEffects)
            if (!string.IsNullOrEmpty(sfx.name))
                sfxDict[sfx.name] = sfx;

        if (backgroundMusic != null)
            PlayMusic(backgroundMusic);
    }

    public void PlaySFX(string sfxName, Vector3 position)
    {
        if (!sfxDict.TryGetValue(sfxName, out SoundEffect sfx))
        {
            Debug.LogWarning($"[AudioManager] SFX not found: '{sfxName}'");
            return;
        }

        if (sfx.clip == null) return;

        GameObject go = new GameObject($"SFX_{sfx.clip.name}");
        go.transform.position = position;
        go.transform.SetParent(null);

        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = sfx.clip;
        src.volume = sfx.volume * sfxVolume;
        src.pitch = sfx.pitch;
        src.spatialBlend = 1f;
        src.minDistance = minDistance;
        src.maxDistance = maxDistance;
        src.rolloffMode = AudioRolloffMode.Logarithmic;
        src.playOnAwake = false;
        src.Play();

        Destroy(go, sfx.clip.length / Mathf.Abs(sfx.pitch));
    }

    public void PlayAccelerator(Vector3 pos) => PlaySFX("Accelerator", pos);
    public void PlayBoost(Vector3 pos) => PlaySFX("BoostVelocity", pos);
    public void PlayCheckpoint(Vector3 pos) => PlaySFX("Checkpoint", pos);
    public void PlayEnemyShot(Vector3 pos) => PlaySFX("EnemyShot", pos);
    public void PlayShield(Vector3 pos) => PlaySFX("Shield", pos);
    public void PlayEnemyDeath(Vector3 pos) => PlaySFX("EnemyDeath", pos);
    public void PlayLoseRings(Vector3 pos) => PlaySFX("LoseRings", pos);
    public void PlaySpikes(Vector3 pos) => PlaySFX("Spikes", pos);
    public void PlayPickEmerald(Vector3 pos) => PlaySFX("PickEmerald", pos);
    public void PlayPickRings(Vector3 pos) => PlaySFX("PickRings", pos);
    public void PlayRunning(Vector3 pos) => PlaySFX("Running", pos);
    public void PlayJump(Vector3 pos) => PlaySFX("Jump", pos);
    public void PlayPowerUp(Vector3 pos) => PlaySFX("PowerUp", pos);
    public void PlayTrampolines(Vector3 pos) => PlaySFX("Trampolines", pos);
    public void PlayWalking(Vector3 pos) => PlaySFX("Walking", pos);

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