using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ODT.ThatAxe
{


    [RequireComponent(typeof(Rigidbody))]
    public class AxeBehaviour : MonoBehaviour
    {
        [Header("Axe object")]
        [SerializeField]
        private GameObject axe;
        [SerializeField]
        private GameObject axeTempHolder;

        [Header("Axe Stats")]
        [SerializeField]
        private float flightSpeed;
        [SerializeField]
        private float throwPower;
        [SerializeField]
        private float rotationSpeed;

        private Rigidbody rb;
        private AxeCollisionBehaviour collision;
        private AxeState state;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

        }

        private void FixedUpdate()
        {
            if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            {
                if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
                {
                    Debug.Log("On");
                }
            }
        }

    }
}
