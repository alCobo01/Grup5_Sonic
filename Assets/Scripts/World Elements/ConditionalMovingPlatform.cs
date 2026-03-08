using UnityEngine;

public class ConditionalMovingPlatform : MonoBehaviour
{

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    private Vector3[] _waypointPositions;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;

    [SerializeField] private string playerTag = "Player";

    private int _currentIndex = 0;

    private Vector3 _previousPosition;
    private PlayerPowerUpController _playerCache;
    private bool _hasReachedEnd = false;
    private bool _isPlayerOnPlatform = false;
    private bool _isActivated = false;

    [Header("Activation Mode")]
    [SerializeField] private bool moveOnceAndDone = true;

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

        if (_isActivated)
        {
            MoveTowardsTarget();
            return;
        }
        if (_isPlayerOnPlatform && HasKeyCheck())
        {
            _isActivated = true;
            MoveTowardsTarget();
        }
    }

    private bool HasKeyCheck()
    {
        if (_playerCache == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
            {
                _playerCache = playerObj.GetComponentInParent<PlayerPowerUpController>();
            }
        }
        
        return _playerCache != null && _playerCache.HasKey;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckPlayerContact(other.gameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckPlayerContact(other.gameObject, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckPlayerContact(collision.gameObject, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        CheckPlayerContact(collision.gameObject, false);
    }

    private void CheckPlayerContact(GameObject obj, bool isEntering)
    {
        if (obj.CompareTag(playerTag) || obj.GetComponentInParent<PlayerPowerUpController>() != null)
        {
            _isPlayerOnPlatform = isEntering;
               
            if (isEntering && _playerCache == null)
            {
                _playerCache = obj.GetComponentInParent<PlayerPowerUpController>();
            }
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
