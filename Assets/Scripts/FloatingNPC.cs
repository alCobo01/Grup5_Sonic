using UnityEngine;

public class FloatingNPC : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform target;           // El player (se asigna automático por tag)
    public float followSpeed = 3f;     // Velocidad de movimiento
    public float stopDistance = 2f;    // Distancia mínima al player
    public float rotationSpeed = 5f;   // Velocidad para mirar al player

    [Header("Flotación")]
    public float floatHeight = 1.5f;   // Altura base sobre el suelo
    public float floatAmplitude = 0.3f; // Qué tanto sube y baja
    public float floatFrequency = 1.5f; // Qué tan rápido sube y baja

    [Header("Suavizado")]
    public float smoothTime = 0.3f;    // Suavidad del movimiento

    private Vector3 velocity = Vector3.zero;
    private float initialY;

    void Start()
    {
        // Buscar el player automáticamente si no se asignó
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        initialY = transform.position.y;
    }

    void Update()
    {
        if (target == null) return;

        HandleFloating();
        HandleFollowing();
        HandleRotation();
    }

    void HandleFloating()
    {
        // Calcular la posición Y flotante con una onda senoidal
        float newY = floatHeight + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Aplicar solo la altura (sin afectar X y Z)
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, newY, Time.deltaTime * 5f);
        transform.position = pos;
    }

    void HandleFollowing()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 targetPos = new Vector3(
                target.position.x,
                floatHeight + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude,
                target.position.z
            );

            // Lerp suave, sin tirones
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                followSpeed * Time.deltaTime
            );
        }
    }

    void HandleRotation()
    {
        // Hacer que el NPC mire al player (solo en el eje Y)
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0; // Ignorar diferencia de altura para la rotación

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

    // Mostrar el stopDistance en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}