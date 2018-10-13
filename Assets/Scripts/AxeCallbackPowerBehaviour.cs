using UnityEngine;

public class AxeCallbackPowerBehaviour : MonoBehaviour {

    [SerializeField]
    private Transform centerEyeAnchorTransform;
    [SerializeField]
    private Transform rightHandAnchorTransform;

    [SerializeField]
    private GameEvent OnAxeCallbackEvent;

    private bool isEventRaiseOnce = false;

    private void Update()
    {
        if (rightHandAnchorTransform.position.y > centerEyeAnchorTransform.position.y)
        {
            if (!isEventRaiseOnce)
            {
                isEventRaiseOnce = true;
                OnAxeCallbackEvent.Raise();
            } 
        }
        else
        {
            isEventRaiseOnce = false;
        }
    }
}
