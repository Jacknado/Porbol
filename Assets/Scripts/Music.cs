using System;
using Unity.Mathematics;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float phase;
    public float frequency = 440f;
    public float time = 0;
    void OnAudioFilterRead(float[] data, int channels)
    {
        float increment = frequency * 2f * Mathf.PI / 48000f;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            float sample = Mathf.Sin(phase) * MathF.Tan(phase);  // sine wave

            data[i] = sample;
            if (channels > 1)
                data[i+1] = sample;
        }
    }
    void Update()
    {
        time += Time.deltaTime;
        frequency += Mathf.Floor(time)/5 - 440;
        if(frequency > 443)
        {
            frequency = 440;
            time = 0;
        }
    }

}
