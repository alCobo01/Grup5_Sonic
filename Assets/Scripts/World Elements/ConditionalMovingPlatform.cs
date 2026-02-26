using UnityEngine;

public class ConditionalMovingPlatform : MonoBehaviour
{

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    private Vector3[] _waypointPositions;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;

    [Tooltip("Tag del jugador para detectar la colisi�n")]
    [SerializeField] private string playerTag = "Player";

    private int _currentIndex = 0;

    private Vector3 _previousPosition;
    private PlayerPowerUpController _playerCache;
    private bool _hasReachedEnd = false;
    private bool _isPlayerOnPlatform = false;

    private void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning($"[ConditionalMovingPlatform] {name}: necesita al menos 2 waypoints.");
            enabled = false;
            return;
        }

        _waypointPositions = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            _waypointPositions[i] = waypoints[i].position;
        }

        transform.position = _waypointPositions[0];
        _previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_hasReachedEnd) return;
        // Solo se mueve si el jugador está encima y tiene la llave
        if (!CanMove()) return;
        MoveTowardsTarget();
    }

    private bool CanMove()
    {
        if (_playerCache)
        {
            var playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj)
            {
                _playerCache = playerObj.GetComponentInParent<PlayerPowerUpController>();
            }
        }

        return _isPlayerOnPlatform && _playerCache && _playerCache.HasKey;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isPlayerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isPlayerOnPlatform = false;
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _waypointPositions[GetNextIndex()];
        Vector3 delta = target - transform.position;
        float stepSize = speed * Time.deltaTime;

        if (delta.magnitude <= stepSize)
        {
            transform.position = target;
            
            if (_currentIndex >= waypoints.Length - 1)
            {
                _hasReachedEnd = true;
                Debug.Log($"[ConditionalMovingPlatform] {name}: Ha llegado al destino final.");
                return;
            }

            AdvanceIndex();

        }
        else
        {
            transform.position += delta.normalized * stepSize;
        }
        
        _previousPosition = transform.position;
    }
    
    private int GetNextIndex()
    {
        return Mathf.Clamp(_currentIndex + 1, 0, waypoints.Length - 1);
    }

    private void AdvanceIndex()
    {
        _currentIndex = Mathf.Clamp(_currentIndex + 1, 0, waypoints.Length - 1);
    }
}
