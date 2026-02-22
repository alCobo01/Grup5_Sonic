using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spawnOffset = 2f;
    
    private GameObject _player;
    private int _indexCheckPoints;

    private void Awake()
    {
        Instance = this;

        _indexCheckPoints = PlayerPrefs.GetInt("checkPointIndex", 0);
        if (_indexCheckPoints < 0 || _indexCheckPoints >= checkPoints.Length)
        {
            PlayerPrefs.SetInt("checkPointIndex", 0);
            _indexCheckPoints = 0;
        }

        _player = GameObject.FindGameObjectWithTag("Player");

        Transform cpTransform = checkPoints[_indexCheckPoints].transform;
        Vector3 spawnPosition = cpTransform.position - cpTransform.forward * spawnOffset;
        
        if (_player == null)
        {
            _player = Instantiate(playerPrefab, spawnPosition, cpTransform.rotation);
        }
        else
        {
            _player.transform.position = spawnPosition;
            _player.transform.rotation = cpTransform.rotation;
        }
    }
    
    public void LastCheckPoint(GameObject checkPoint)
    {
        for (var i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i] == checkPoint && i > _indexCheckPoints)
            {
                PlayerPrefs.SetInt("checkPointIndex", i);
            }
        }
    }
    
    public void SetStartPoint()
    {
        PlayerPrefs.SetInt("checkPointIndex", 0);
        _indexCheckPoints = 0;
    }
    private void OnEnable()
    {
        BaseMenu.RestartCheckPoint += SetStartPoint;
    }

    private void OnDisable()
    {
        BaseMenu.RestartCheckPoint -= SetStartPoint;
    }
}
