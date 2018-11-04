using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum AxeState
{
    None,
    Static,
    //Attacking,
    Thrown,
    Grabbled,
    Travelling,
    Returning
}

[RequireComponent(typeof(Rigidbody))]
public class AxeGrabbableBehaviour : OVRGrabbable
{
    [Header("Axe")]
    [SerializeField]
    private Transform axeMeshTransform;
    [SerializeField]
    private float rotationSpeedWhenTravelling;
    [SerializeField]
    private float rotationSpeedWhenReturning;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnGrabBegin;
    [SerializeField]
    private UnityEvent OnFirstGrabBegin;
    [SerializeField]
    private UnityEvent OnThrown;
    [SerializeField]
    private UnityEvent OnGrabbled;
    [SerializeField]
    private UnityEvent OnAttacking;
    [SerializeField]
    private UnityEvent OnTravelling;
    [SerializeField]
    private UnityEvent OnReturning;

    private Rigidbody rb;
    private Vector3 normalGravity = new Vector3(0f, -9.81f, 0f);
    private Vector3 travellingGravity = new Vector3(0f, -2.81f, 0f);

    private AxeState _axeState = AxeState.Thrown;
    protected AxeState axeState
    {
        get
        {
            return _axeState;
        }
        set {
            _axeState = value;
            switch (_axeState)
            {
                case AxeState.Static:
                    OnGrabBegin.Invoke();
                    break;
                case AxeState.Thrown:
                    OnThrown.Invoke();
                    break;
                case AxeState.Grabbled:
                    OnGrabbled.Invoke();
                    break;
                case AxeState.Travelling:
                    OnTravelling.Invoke();
                    break;
                //case AxeState.Attacking:
                //    OnAttacking.Invoke();
                //    break;
                case AxeState.Returning:
                    OnReturning.Invoke();
                        break;
                default:
                    break;
            }
        }
    }

    [SerializeField]
    private Transform rHand;

    private float angularVelocityThreshold = 350f;
    private Vector3 linearVelocityWhenGrabEnd;
    private Vector3 angularVelocityWhenGrabEnd;
    private bool isAvailableToReturn = true;
    private bool throwedToClose = false;
    private float returningTimeThreshold = .5f;
    private float returningStartTime;
    private Vector3 returningStartPosition;
    private Vector3 returningMiddlePosition;
    private float returnArcZ = 10f;
    private float journeyLength;

    private bool isFirstGrab = true;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
       
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
        rb.isKinematic = true;
        rb.useGravity = false;

        if (isFirstGrab)
        {
            isFirstGrab = false;
            OnFirstGrabBegin.Invoke();
        }

        axeMeshTransform.rotation = new Quaternion(0, 0, 0, 0);
        isAvailableToReturn = false;
        axeState = AxeState.Static;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        rb.isKinematic = false;
        if (Mathf.Abs((int)angularVelocity.x) < angularVelocityThreshold)
        {
            Physics.gravity = normalGravity;
            rb.useGravity = true;
            rb.velocity = linearVelocity;
        }
        else
        {
            rb.velocity = linearVelocity * 2;
            Physics.gravity = travellingGravity;
            rb.useGravity = true;
            axeState = AxeState.Travelling;
        }

        rb.angularVelocity = angularVelocity;
        linearVelocityWhenGrabEnd = linearVelocity;
        angularVelocityWhenGrabEnd = angularVelocity;

        StartCoroutine(WaitForReturnCoroutine());

        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    public void OnAxeCalled()
    {
       if ((axeState == AxeState.Travelling || axeState == AxeState.Thrown || axeState == AxeState.Grabbled)
            && isAvailableToReturn)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            returningStartTime = Time.time;
            returningStartPosition = gameObject.GetComponent<Rigidbody>().transform.position;

            journeyLength = Vector3.Distance(returningStartPosition, rHand.position);

            if (journeyLength < 6)
            {
                throwedToClose = true;
            }
            else
            {
                throwedToClose = false;
                if (transform.forward.z < 0)
                {
                    returningMiddlePosition = returningStartPosition + (rHand.position - returningStartPosition) / 2 + (-Vector3.forward * returnArcZ);
                }
                else
                {
                    returningMiddlePosition = returningStartPosition + (rHand.position - returningStartPosition) / 2 + (Vector3.forward * returnArcZ);
                }
                
            }

            axeState = AxeState.Returning;
        }
    }

    public void OnAxeCollided()
    {
        if (gameObject.activeInHierarchy)
        {
            if (axeState == AxeState.Returning)
            {
                return;
            }

            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            axeState = AxeState.Thrown;
        }
    }

    public void OnAxeMonsterCollided()
    {
        if (axeState == AxeState.Returning)
        {
            return;
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        axeState = AxeState.Grabbled;
    }

    protected IEnumerator WaitForReturnCoroutine()
    {
        yield return new WaitForSeconds(returningTimeThreshold);
        isAvailableToReturn = true;
    }

    protected void SetAvailableToReturn()
    {
        isAvailableToReturn = true;
    }

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        returningStartPosition = transform.position;
        m_grabbedKinematic = rb.isKinematic;

        isAvailableToReturn = false;
        Invoke("SetAvailableToReturn", 5f);
       
    }

    protected void Update()
    {
        switch (axeState)
        {
            case AxeState.Static:
                if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > 3)
                {
                    //axeState = AxeState.Attacking;
                    OnAttacking.Invoke();
                }
                break;
            //case AxeState.Attacking:
            //    if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude < 2)
            //    {
            //        axeState = AxeState.Static;
            //    }
            //    break;
            case AxeState.Travelling:
                axeMeshTransform.Rotate(0, 0, rotationSpeedWhenTravelling * Time.deltaTime, Space.Self);
                break;
            case AxeState.Returning:

                float distCovered = (Time.time - returningStartTime) * journeyLength;
                float fracJourney = distCovered / journeyLength;

                if (throwedToClose)
                {

                    transform.position = Vector3.Lerp(returningStartPosition, rHand.position, fracJourney * 2);
                }
                else
                {
                    Vector3 m1 = Vector3.Lerp(returningStartPosition, returningMiddlePosition, fracJourney);
                    Vector3 m2 = Vector3.Lerp(returningMiddlePosition, rHand.position, fracJourney);

                    transform.position = Vector3.Lerp(m1, m2, fracJourney);
                }
                
                axeMeshTransform.transform.Rotate(0, 0, rotationSpeedWhenReturning * Time.deltaTime, Space.Self);
                break;
        }
    }

    protected void OnDestroy()
    {
        OnGrabBegin.RemoveAllListeners();
        OnThrown.RemoveAllListeners();
        OnTravelling.RemoveAllListeners();
        OnReturning.RemoveAllListeners();
    }
}

