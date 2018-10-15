using UnityEngine;

public class AxeHapticsVibrationBehaviour : MonoBehaviour
{

    [SerializeField]
    protected OVRInput.Controller controllerMask;

    protected OVRHapticsClip clipLight;
    protected OVRHapticsClip clipMedium;
    protected OVRHapticsClip clipHard;

    protected OVRHaptics.OVRHapticsChannel channel = OVRHaptics.RightChannel;

    public void VibrateLight()
    {
        channel.Preempt(clipLight);
    }

    public void VibrateMedium()
    {
        channel.Preempt(clipMedium);
    }

    public void VibrateHard()
    {
        channel.Preempt(clipHard);
    }

    public void VibrateByDistance(float inicialDistance, float current)
    {

        float distanceFactor = inicialDistance / 3f;

        if (distanceFactor * 2 < current)
        {
            channel.Queue(clipLight);
        }
        else if (distanceFactor < current)
        {
            channel.Queue(clipMedium);
        }
        else
        {

            channel.Queue(clipHard);
        }
    }


    protected void InitializeOVRHaptics()
    {

        int cnt = 10;
        clipLight = new OVRHapticsClip(cnt);
        clipMedium = new OVRHapticsClip(cnt);
        clipHard = new OVRHapticsClip(cnt);
        for (int i = 0; i < cnt; i++)
        {
            clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)50;
            clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)100;
            clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)250;
        }

        clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
        clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
        clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
    }

    protected void OnEnable()
    {
        InitializeOVRHaptics();

        if (controllerMask == OVRInput.Controller.LTouch)
            channel = OVRHaptics.LeftChannel;
    }

}
