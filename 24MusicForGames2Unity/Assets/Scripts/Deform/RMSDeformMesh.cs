using UnityEngine;
using System.Runtime.InteropServices;
using FMODUnity;

public class RMSDeformMesh : MonoBehaviour
{
    public EventReference eventPath;
    public GameObject meshObject;
    public float deformationMultiplier = 10f;
    public float smoothingFactor = 0.9f; // Smoothing factor for cooler effect
    public int windowSize = 512;
    public FMOD.DSP_FFT_WINDOW windowShape = FMOD.DSP_FFT_WINDOW.RECT;

    private FMOD.Studio.EventInstance eventInstance;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.DSP dsp;
    private FMOD.DSP_PARAMETER_FFT fftparam;
    private float[] samples;
    private float[] smoothedSamples;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;

    private void Start()
    {
        mesh = meshObject.GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];

        samples = new float[windowSize];
        smoothedSamples = new float[windowSize];

        // Prepare FMOD event
        PrepareFMODEventInstance();
    }

    private void PrepareFMODEventInstance()
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        eventInstance.start();

        FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out dsp);
        dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)windowShape);
        dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, windowSize * 2);

        eventInstance.getChannelGroup(out channelGroup);
        channelGroup.addDSP(0, dsp);
    }

    private void FixedUpdate()
    {
        GetSpectrumData();
        ApplySmoothing();
        DeformMesh();
    }

    private void GetSpectrumData()
    {
        System.IntPtr data;
        uint length;

        dsp.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out data, out length);
        fftparam = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(data, typeof(FMOD.DSP_PARAMETER_FFT));

        if (fftparam.numchannels == 0)
        {
            eventInstance.getChannelGroup(out channelGroup);
            channelGroup.addDSP(0, dsp);
        }
        else if (fftparam.numchannels >= 1)
        {
            for (int s = 0; s < windowSize; s++)
            {
                float totalChannelData = 0f;
                for (int c = 0; c < fftparam.numchannels; c++)
                    totalChannelData += fftparam.spectrum[c][s];
                samples[s] = totalChannelData / fftparam.numchannels;
            }
        }
    }

    private void ApplySmoothing()
    {
        for (int i = 0; i < samples.Length; i++)
        {
            smoothedSamples[i] = Mathf.Lerp(smoothedSamples[i], samples[i], 1.0f - smoothingFactor);
        }
    }

    private void DeformMesh()
    {
        if (mesh == null)
        {
            UnityEngine.Debug.LogError("Mesh is not assigned.");
            return;
        }

        // Calculate RMS value of the smoothed samples
        float rms = 0f;
        for (int i = 0; i < smoothedSamples.Length; i++)
        {
            rms += smoothedSamples[i] * smoothedSamples[i];
        }
        rms = Mathf.Sqrt(rms / smoothedSamples.Length);

        float displacement = rms * deformationMultiplier;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = originalVertices[i] + originalVertices[i].normalized * displacement;
        }

        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }

    private void OnDestroy()
    {
        channelGroup.removeDSP(dsp); // Ensure DSP is removed before releasing it
        eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        eventInstance.release();
        dsp.release();
    }
}