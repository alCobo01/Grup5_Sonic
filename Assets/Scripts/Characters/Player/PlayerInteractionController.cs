using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactionLayer = ~0;
    [SerializeField] private Transform rayOrigin;

    private IInteractable _currentInteractable;
    private PlayerInputController _inputController;

    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
        _inputController.OnInteractEvent += HandleInteraction;
        
        if (rayOrigin == null)
            if (Camera.main != null) rayOrigin = Camera.main.transform;
    }

    private void Update() => DetectInteractable();
    
    private void DetectInteractable()
    {
        var ray = new Ray(rayOrigin.position, rayOrigin.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            _currentInteractable = hit.collider.TryGetComponent(out IInteractable interactable) ? interactable : null;
        }
    }

    private void HandleInteraction() => _currentInteractable?.Interact();
}