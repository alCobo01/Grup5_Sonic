using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private GameObject playerPrefab;
    private GameObject _player;
    private int _indexCheckPoints;

    private void Awake()
    {
        Instance = this;

        if (_indexCheckPoints >= checkPoints.Length)
        {
            PlayerPrefs.SetInt("checkPointIndex", 0);
            _indexCheckPoints = 0;
        }

        _indexCheckPoints = PlayerPrefs.GetInt("checkPointIndex");
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            _player = Instantiate(playerPrefab, checkPoints[_indexCheckPoints].transform.position, Quaternion.identity);
        }
        else
        {
            _player.transform.position = checkPoints[_indexCheckPoints].transform.position;
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
        //PauseMenu.RestartCheckPoint += SetStartPoint;
        //WinMenu.RestartCheckPoint += SetStartPoint;
    }

    private void OnDisable()
    {
        //PauseMenu.RestartCheckPoint -= SetStartPoint;
        //WinMenu.RestartCheckPoint += SetStartPoint;
    }
}
