using UnityEngine;
using System.Runtime.InteropServices;
using FMODUnity;

public class FFTDeformMesh : MonoBehaviour
{
   public EventReference eventPath;
   public GameObject meshObject;
   public float deformationMultiplier = 10f;
   public float smoothingFactor = 0.9f; // Smoothing factor for cooler effect
   public int windowSize = 512;
   public FMOD.DSP_FFT_WINDOW windowShape = FMOD.DSP_FFT_WINDOW.RECT;
   public float updateInterval = 0.1f; // Update interval in seconds

   private FMOD.Studio.EventInstance eventInstance;
   private FMOD.ChannelGroup channelGroup;
   private FMOD.DSP dsp;
   private FMOD.DSP_PARAMETER_FFT fftparam;
   private float[] samples;
   private float[] smoothedSamples;
   private Mesh mesh;
   private Vector3[] originalVertices;
   private Vector3[] modifiedVertices;
   private int numBands = 8; // Number of frequency bands to map to different parts of the sphere
   private float nextUpdateTime = 0f;

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

   private void Update()
   {
       if (Time.time >= nextUpdateTime)
       {
           nextUpdateTime = Time.time + updateInterval;
           GetSpectrumData();
           ApplySmoothing();
           DeformMesh();
       }
   }

   private void GetSpectrumData()
   {
       System.IntPtr data;
       uint length;

       dsp.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out data, out length);
       fftparam = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(data, typeof(FMOD.DSP_PARAMETER_FFT));
       var spectrum = fftparam.spectrum;

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
                   totalChannelData += spectrum[c][s];
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
           Debug.LogError("Mesh is not assigned.");
           return;
       }

       int verticesPerBand = originalVertices.Length / numBands;

       for (int i = 0; i < originalVertices.Length; i++)
       {
           int band = i / verticesPerBand;
           band = Mathf.Clamp(band, 0, smoothedSamples.Length - 1);
           float displacement = smoothedSamples[band] * deformationMultiplier;

           modifiedVertices[i] = originalVertices[i] + originalVertices[i].normalized * displacement;
       }

       mesh.vertices = modifiedVertices;

       // Recalculate normals less frequently for performance
       if (Time.time % (updateInterval * 5) < updateInterval)
       {
           mesh.RecalculateNormals();
       }
   }

   private void OnDestroy()
   {
       channelGroup.removeDSP(dsp); // Ensure DSP is removed before releasing it
       eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
       eventInstance.release();
       dsp.release();
   }
}