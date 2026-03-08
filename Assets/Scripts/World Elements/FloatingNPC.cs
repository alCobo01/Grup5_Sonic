using UnityEngine;
public class FloatingNPC : MonoBehaviour
{
    [Header("Seguimiento")]
    public string tagTarget = "Player";
    public float followSpeed = 3f;
    public float stopDistance = 2f;
    public float rotationSpeed = 5f;

    [Header("Flotación")]
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;
    public float floatHeightOffset = 1.5f; // Altura sobre el player

    [Header("Suavizado")]
    public float smoothTime = 0.3f;

    private Transform _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(tagTarget).transform;
    }

    private void FixedUpdate()
    {
        HandleFollowing();
        HandleRotation();
    }

    private void HandleFollowing()
    {
        Vector3 directionFromPlayer = (transform.position - _target.position);
        directionFromPlayer.y = 0; // Solo calcular distancia horizontal

        float horizontalDistance = directionFromPlayer.magnitude;

        Vector3 targetPos;

        if (horizontalDistance < 0.01f)
        {
            // Si están exactamente en el mismo punto, empujar en cualquier dirección
            directionFromPlayer = Vector3.forward;
        }

        // Siempre mantener exactamente stopDistance del player horizontalmente
        Vector3 desiredHorizontalPos = _target.position + directionFromPlayer.normalized * stopDistance;

        float floatY = _target.position.y + floatHeightOffset + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        targetPos = new Vector3(desiredHorizontalPos.x, floatY, desiredHorizontalPos.z);

        // Si el player se aleja, seguirlo. Si se acerca, huir. En ambos casos va a targetPos.
        float speed = horizontalDistance > stopDistance ? followSpeed : followSpeed * 1.5f; // Huye un poco más rápido

        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 lookDirection = _target.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}