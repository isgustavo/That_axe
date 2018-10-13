using UnityEngine;

public class AxeCollisionBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameEvent OnAxeCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
            OnAxeCollisionEvent.Raise();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
            OnAxeCollisionEvent.Raise();
        
    }*/
}

