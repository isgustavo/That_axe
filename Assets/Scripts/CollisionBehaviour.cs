using UnityEngine;
using UnityEngine.Events;

public class CollisionBehaviour : MonoBehaviour
{
    [SerializeField]
    private string collisionTag;

    [SerializeField]
    protected UnityEvent OnCollisionEnterEvent;

    [SerializeField]
    protected UnityEvent OnCollisionExitEvent;


    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == collisionTag)
            OnCollisionEnterEvent.Invoke();
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == collisionTag)
            OnCollisionExitEvent.Invoke();
    }

    protected void OnDestroy()
    {
        OnCollisionEnterEvent.RemoveAllListeners();
        OnCollisionExitEvent.RemoveAllListeners();
    }
}

