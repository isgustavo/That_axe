using UnityEngine;
using UnityEngine.Events;

public class AxeCollisionBehaviour : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnAxeCollisionEvent;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
            OnAxeCollisionEvent.Invoke();
    }

    protected void OnDestroy()
    {
        OnAxeCollisionEvent.RemoveAllListeners();
    }
}

