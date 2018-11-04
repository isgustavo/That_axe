using UnityEngine;
using UnityEngine.Events;

public class CollisionBehaviour : MonoBehaviour
{
    [SerializeField]
    private string collisionTag;
    [SerializeField]
    private bool justOneTime;

    [SerializeField]
    protected UnityEvent OnCollisionEnterEvent;

    [SerializeField]
    protected UnityEvent OnCollisionExitEvent;

    private Collider col;

    private bool triggerEnter = false;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == collisionTag && !triggerEnter)
        {
            triggerEnter = true;
            OnCollisionEnterEvent.Invoke();     
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == collisionTag && triggerEnter) 
        {
            triggerEnter = false;
            OnCollisionExitEvent.Invoke();
            if (justOneTime)
            {
                col.enabled = false;
            }
        }
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        col.enabled = true;
    }

    protected void OnDestroy()
    {
        OnCollisionEnterEvent.RemoveAllListeners();
        OnCollisionExitEvent.RemoveAllListeners();
    }
}

