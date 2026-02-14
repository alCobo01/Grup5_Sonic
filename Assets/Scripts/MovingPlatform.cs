using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum LoopMode
    {
        Loop,
        PingPong
    }

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    private Vector3[] _waypointPositions;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private LoopMode loopMode = LoopMode.PingPong;

    [Header("Passenger Detection")]
    [SerializeField] private LayerMask passengerLayer;

    private int _currentIndex = 0;
    private int _direction = 1;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    private Vector3 _previousPosition;

    private void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning($"[MovingPlatform] {name}: necesita al menos 2 waypoints.");
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
        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f) _isWaiting = false;
            return;
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _waypointPositions[GetNextIndex()];
        Vector3 delta = target - transform.position;
        float stepSize = speed * Time.deltaTime;

        if (delta.magnitude <= stepSize)
        {
            transform.position = target;
            AdvanceIndex();

            if (waitTime > 0f)
            {
                _waitTimer = waitTime;
                _isWaiting = true;
            }
        }
        else
        {
            transform.position += delta.normalized * stepSize;
        }

        Vector3 platformDelta = transform.position - _previousPosition;
        MovePassengers(platformDelta);

        _previousPosition = transform.position;
    }

    private void MovePassengers(Vector3 delta)
    {
        if (delta == Vector3.zero) return;

        Collider platformCollider = GetComponent<Collider>();
        if (platformCollider == null) return;

        Bounds bounds = platformCollider.bounds;
        Vector3 center = bounds.center + Vector3.up * (bounds.extents.y + 0.05f);
        Vector3 halfExt = new Vector3(bounds.extents.x * 0.95f, 0.1f, bounds.extents.z * 0.95f);

        Collider[] hits = Physics.OverlapBox(center, halfExt, Quaternion.identity, passengerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.MovePosition(hit.attachedRigidbody.position + delta);
            }
            else
            {
                hit.transform.position += delta;
            }
        }
    }

    private int GetNextIndex()
    {
        return loopMode == LoopMode.Loop
            ? (_currentIndex + 1) % waypoints.Length
            : Mathf.Clamp(_currentIndex + _direction, 0, waypoints.Length - 1);
    }

    private void AdvanceIndex()
    {
        if (loopMode == LoopMode.Loop)
        {
            _currentIndex = (_currentIndex + 1) % waypoints.Length;
        }
        else
        {
            _currentIndex += _direction;
            if (_currentIndex >= waypoints.Length - 1 || _currentIndex <= 0)
                _direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.15f);

            int next = loopMode == LoopMode.Loop
                ? (i + 1) % waypoints.Length
                : i + 1;

            if (next < waypoints.Length && waypoints[next] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}