using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spawnOffset = 2f;
    
    private GameObject _player;
    private int _indexCheckPoints;
    private Vector3 _lastSpawnPosition;
    private Transform _lastCheckpointTransform;

    private void Awake()
    {
        Screen.SetResolution(2560, 1440, FullScreenMode.MaximizedWindow);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        
        Instance = this;

        _indexCheckPoints = PlayerPrefs.GetInt("checkPointIndex", 0);
        if (_indexCheckPoints < 0 || _indexCheckPoints >= checkPoints.Length)
        {
            PlayerPrefs.SetInt("checkPointIndex", 0);
            _indexCheckPoints = 0;
        }

        _player = GameObject.FindGameObjectWithTag("Player");

        Transform cpTransform = checkPoints[_indexCheckPoints].transform;
        Vector3 spawnPosition = cpTransform.position - cpTransform.right * spawnOffset;

        _lastSpawnPosition = spawnPosition;
        _lastCheckpointTransform = cpTransform;

        if (_player == null)
        {
            _player = Instantiate(playerPrefab, spawnPosition, cpTransform.rotation);
        }
        else
        {
            LoadPlayerOnCheckpoint(spawnPosition, cpTransform);
        }
    }
    
    private void OnEnable()
    {
        BaseMenu.RestartCheckPoint += SetStartPoint;
        PlayerHealthController.ReloadPlayer += HandleReloadPlayer;
    }

    private void OnDisable()
    {
        PlayerHealthController.ReloadPlayer -= HandleReloadPlayer;
        BaseMenu.RestartCheckPoint -= SetStartPoint;
    }
    
    
    public void LastCheckPoint(GameObject checkPoint)
    {
        for (var i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i] == checkPoint && i > _indexCheckPoints)
            {
                PlayerPrefs.SetInt("checkPointIndex", i);
                _indexCheckPoints = i;

                Transform cpTransform = checkPoints[i].transform;
                _lastSpawnPosition = cpTransform.position - cpTransform.forward * spawnOffset;
                _lastCheckpointTransform = cpTransform;
            }
        }
    }
    
    public void LoadPlayerOnCheckpoint(Vector3 spawnPosition, Transform cpTransform)
    {
        _player.transform.position = spawnPosition;
        _player.transform.rotation = cpTransform.rotation;
    }
    
    public void LoadPlayerOnCheckpoint() { LoadPlayerOnCheckpoint(_lastSpawnPosition, _lastCheckpointTransform); }
    
    public void SetStartPoint()
    {
        PlayerPrefs.SetInt("checkPointIndex", 0);
        _indexCheckPoints = 0;
    }
    
    private void HandleReloadPlayer()
    {
        StartCoroutine(ReloadSequence());
    }

    private System.Collections.IEnumerator ReloadSequence()
    {
        if (ScreenFader.Instance != null)
        {
            yield return ScreenFader.Instance.FadeOut();
        }

        LoadPlayerOnCheckpoint();

        if (ScreenFader.Instance != null)
        {
            yield return ScreenFader.Instance.FadeIn();
        }
    }
}
