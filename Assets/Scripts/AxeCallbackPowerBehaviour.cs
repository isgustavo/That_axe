﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AxeCallbackPowerBehaviour : MonoBehaviour {

    [SerializeField]
    protected Transform centerEyeAnchorTransform;
    [SerializeField]
    protected Transform rightHandAnchorTransform;

    [SerializeField]
    protected UnityEvent OnAxeCallbackEvent;

    public void AvailablePower()
    {
        StartCoroutine(OnListenerHandCoroutine());
    }

    public void UnavailiablePower()
    {
        StopAllCoroutines();
    }

    protected IEnumerator OnListenerHandCoroutine()
    {
        while (true)
        {

            if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch)
                && OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.RTouch) == 0
                &&  rightHandAnchorTransform.position.y > centerEyeAnchorTransform.position.y)
            {

                OnAxeCallbackEvent.Invoke();
            }

            yield return new WaitForSeconds(.5f);
        }

    }

    protected void OnDestroy()
    {
        OnAxeCallbackEvent.RemoveAllListeners();
    }
}
