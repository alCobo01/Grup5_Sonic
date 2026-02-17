using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.SetTrigger("Used");
            //GameManager.Instance.LastCheckPoint(gameObject);
        }
    }
}
