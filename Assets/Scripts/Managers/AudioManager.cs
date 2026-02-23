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

    [Header("SFX Prefab")]
    [Tooltip("Prefab con AudioSource configurado. Si es null se crea uno básico.")]
    public AudioSource soundFXObject;

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

    public void PlaySFX(string sfxName, Transform spawnTransform)
    {
        if (!sfxDict.TryGetValue(sfxName, out SoundEffect sfx))
        {
            Debug.LogWarning($"[AudioManager] SFX not found: '{sfxName}'");
            return;
        }

        if (sfx.clip == null)
        {
            Debug.LogWarning($"[AudioManager] Clip is null on SFX: '{sfxName}'");
            return;
        }

        // Spawn gameobject desde prefab o uno básico
        AudioSource audioSource = soundFXObject != null
            ? Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity)
            : new GameObject($"SFX_{sfx.clip.name}").AddComponent<AudioSource>();

        audioSource.transform.position = spawnTransform.position;
        audioSource.transform.SetParent(null);

        // Set clip
        audioSource.clip = sfx.clip;
        // Set volume
        audioSource.volume = sfx.volume * sfxVolume;
        // Set pitch
        audioSource.pitch = sfx.pitch;
        // 2D - se escucha igual desde cualquier distancia
        audioSource.spatialBlend = 0f;
        audioSource.playOnAwake = false;

        // Play sound
        audioSource.Play();

        // Destroy gameobject after clip length
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayAccelerator(Transform t) => PlaySFX("Accelerator", t);
    public void PlayBoost(Transform t) => PlaySFX("BoostVelocity", t);
    public void PlayCheckpoint(Transform t) => PlaySFX("Checkpoint", t);
    public void PlayEnemyShot(Transform t) => PlaySFX("EnemyShot", t);
    public void PlayShield(Transform t) => PlaySFX("Shield", t);
    public void PlayEnemyDeath(Transform t) => PlaySFX("EnemyDeath", t);
    public void PlayLoseRings(Transform t) => PlaySFX("LoseRings", t);
    public void PlaySpikes(Transform t) => PlaySFX("Spikes", t);
    public void PlayPickEmerald(Transform t) => PlaySFX("PickEmerald", t);
    public void PlayPickRings(Transform t) => PlaySFX("PickRings", t);
    public void PlayRunning(Transform t) => PlaySFX("Running", t);
    public void PlayJump(Transform t) => PlaySFX("Jump", t);
    public void PlayPowerUp(Transform t) => PlaySFX("PowerUp", t);
    public void PlayTrampolines(Transform t) => PlaySFX("Trampolines", t);
    public void PlayWalking(Transform t) => PlaySFX("Walking", t);

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