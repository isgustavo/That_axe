using UnityEngine;

public enum AxeState
{
    Static,
    Thrown,
    Travelling,
    Returning
}

public class AxeGrabbableBehaviour : OVRGrabbable
{
    [Header("Axe")]
    [SerializeField]
    private Transform axeMeshTransform;
    [SerializeField]
    private float axeRotationSpeedWhenTravelling;
    [SerializeField]
    private float axeRotationSpeedWhenReturning;

    [SerializeField]
    private Transform rHand;

    private AxeState axeState = AxeState.Static;
    private float angularVelocityThreshold = 350f;
    private Vector3 linearVelocityWhenGrabEnd;
    private Vector3 angularVelocityWhenGrabEnd;
    private float axeTimeWhenGrabEnd;
    private float axeReturningTimeThreshold = 5f;
    private float axeReturningStartTime;
    private Vector3 axeReturningStartPosition;
    private Vector3 axeReturningMiddlePosition;
    private float axeReturnArcZ = 10f;
    private float axeJourneyLength;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        axeMeshTransform.rotation = new Quaternion(0, 0, 0, 0);

        axeState = AxeState.Static;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        if ((int)angularVelocity.x < angularVelocityThreshold)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().velocity = linearVelocity;
            gameObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

        }
        else
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().velocity = linearVelocity * 2;
            gameObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

            axeState = AxeState.Travelling;
        }

        linearVelocityWhenGrabEnd = linearVelocity;
        angularVelocityWhenGrabEnd = angularVelocity;
        axeTimeWhenGrabEnd = Time.time;

        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    public void OnAxeCalled()
    {
        if ((axeState == AxeState.Travelling || axeState == AxeState.Thrown)
            && axeTimeWhenGrabEnd + axeReturningTimeThreshold > Time.time)
        {
            axeState = AxeState.Returning;

            axeReturningStartTime = Time.time;
            axeReturningStartPosition = gameObject.GetComponent<Rigidbody>().transform.position;

            if (axeReturningStartPosition.x > 0)
            {
                axeReturningMiddlePosition = axeReturningStartPosition + (rHand.position - axeReturningStartPosition) / 2 + (-Vector3.forward * axeReturnArcZ);
            }
            else
            {
                axeReturningMiddlePosition = axeReturningStartPosition + (rHand.position - axeReturningStartPosition) / 2 + (Vector3.forward * axeReturnArcZ);
            }

            axeJourneyLength = Vector3.Distance(axeReturningStartPosition, rHand.position);
        }
    }

    public void OnAxeCollided()
    {
        if (axeState == AxeState.Returning)
        {
            return;
        }

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        axeState = AxeState.Thrown;
    }

    private void FixedUpdate()
    {
        if (axeState == AxeState.Travelling)
        {
            axeMeshTransform.Rotate(0, 0, axeRotationSpeedWhenTravelling * Time.deltaTime, Space.Self);
        }

        if (axeState == AxeState.Returning)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            float distCovered = (Time.time - axeReturningStartTime) * 12;
            float fracJourney = distCovered / axeJourneyLength;

            Vector3 m1 = Vector3.Lerp(axeReturningStartPosition, axeReturningMiddlePosition, fracJourney);
            Vector3 m2 = Vector3.Lerp(axeReturningMiddlePosition, rHand.position, fracJourney);

            gameObject.GetComponent<Rigidbody>().transform.position = Vector3.Lerp(m1, m2, fracJourney);

            axeMeshTransform.transform.Rotate(0, 0, axeRotationSpeedWhenReturning * Time.deltaTime, Space.Self);
        }
    }
}

