using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Runtime.InteropServices;

public class FlorianAudioManager : MusicManager
{
    public static MusicManager instance;
    public GameObject mainCamera;
    public Volume volume;

    public float monitoringBuffer = 20f;

    private LensDistortion ld;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.DSP mixerHead;
    private FMOD.DSP_METERING_INFO outputMetering;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        volume = mainCamera.GetComponent<Volume>();

        FMOD.ChannelGroup channelGroup;
        FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out channelGroup);
        channelGroup.getDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, out mixerHead);
        mixerHead.setMeteringEnabled(false, true);

        if (volume.profile.TryGet<LensDistortion>(out ld))
        {
            ld.intensity.value = 0f;


        }
    }

    void Update()
    {
        float masterVolume = 0.0f;
        //masterBus.getVolume(out masterVolume);

        if (volume.profile.TryGet<LensDistortion>(out ld)) {
            //Debug.Log(masterVolume);
            mixerHead.getMeteringInfo(new IntPtr(), out outputMetering);

            float value = 0.0f;

            for (int i = 0; i < outputMetering.numchannels; i++)
            {
                value += outputMetering.rmslevel[i];
            }

            value /= outputMetering.numchannels;

            if (float.IsNaN(value))
            {
                value = 0f;
            }

            Debug.Log(value);

            ld.intensity.value = (ld.intensity.value * (monitoringBuffer - 1) + (- 1 * (value * 5))) / monitoringBuffer;
            //ld.intensity = n;
        }
    }

    public static float Sigmoid(float value)
    {
        double k = Math.Exp((double) value);
        return (float) (k / (1.0f + k));
    }


}
