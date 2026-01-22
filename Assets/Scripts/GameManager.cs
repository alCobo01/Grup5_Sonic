using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private int indexCheckPoints;

    private void Awake()
    {
        Instance = this;

        if (indexCheckPoints >= checkPoints.Length)
        {
            PlayerPrefs.SetInt("checkPointIndex", 0);
            indexCheckPoints = 0;
        }

        indexCheckPoints = PlayerPrefs.GetInt("checkPointIndex");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = Instantiate(playerPrefab, checkPoints[indexCheckPoints].transform.position, Quaternion.identity);
        }
        else
        {
            player.transform.position = checkPoints[indexCheckPoints].transform.position;
        }
    }
    public void LastCheckPoint(GameObject checkPoint)
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i] == checkPoint && i > indexCheckPoints)
            {
                PlayerPrefs.SetInt("checkPointIndex", i);
            }
        }
    }
    public void SetStartPoint()
    {
        PlayerPrefs.SetInt("checkPointIndex", 0);
        indexCheckPoints = 0;
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
