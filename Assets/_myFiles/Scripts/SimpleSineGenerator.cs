using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSineGenerator : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float initialAmplitude = 0.5f;
    [SerializeField] private float frequency = 80.7f; //middle C 261.62f

    [SerializeField] private float amplitude;
    private double _phase;
    private int _sampleRate;

    [SerializeField]private EMusicState musicState;

    private void Awake()
    { 
        _sampleRate = AudioSettings.outputSampleRate;
        musicState = EMusicState.SineWave;
    }

    /*private void Start()
    {
        StartCoroutine(AmplitudeSineWave());
    }*/

    private void SineWave(float[] data, int channels)
    {
        double phaseIncrement = frequency / _sampleRate;

        for (int sample = 0; sample < data.Length; sample += channels)
        {
            float value = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;

            _phase = (_phase + phaseIncrement) % 1;

            for (int channel = 0; channel < channels; channel++)
            {
                data[sample + channel] = value;
            }
        }
    }

    private void SawWave(float[] data, int channels)
    {
        double phaseIncrement = frequency / _sampleRate;

        for (int sample = 0; sample < data.Length; sample += channels)
        {
            float value = ((((float)_phase + 0.5f) % 1) - 0.5f) * amplitude;

            _phase = (_phase + phaseIncrement) % 1;

            for (int channel = 0; channel < channels; channel++)
            {
                data[sample + channel] = value;
            }
        }
    }

    private void SquareWave(float[] data, int channels)
    {
        double phaseIncrement = frequency / _sampleRate;

        for (int sample = 0; sample < data.Length; sample += channels)
        {
            //float value = ((((float)_phase + 0.5f) % 1) - 0.5f) * amplitude;
            float value = ((float)_phase < 0.5 ? 1 : -1) * amplitude;
            _phase = (_phase + phaseIncrement) % 1;

            for (int channel = 0; channel < channels; channel++)
            {
                data[sample + channel] = value;
            }
        }
    }

    public IEnumerator AmplitudeKickWave()
    {
        amplitude = 0;
        float amplitudePhase = 0;
        while (amplitudePhase < 0.5)
        {
            Debug.Log(amplitudePhase);
            if (amplitudePhase < 0.072f)
            {
                amplitude = 10 * (amplitudePhase * amplitudePhase) + 0.5f;
            }
            else if (amplitudePhase >= 0.072f && amplitudePhase < 0.5)
            {
                amplitude = 0.4f / amplitudePhase;
            }
            //amplitude *= 10;
            amplitudePhase += Time.deltaTime;
            yield return null;
        }
        amplitude = 0;
        yield return null;
    }



    private void OnAudioFilterRead(float[] data, int channels)
    {
        switch (musicState)
        {
            case EMusicState.SineWave:
                SineWave(data, channels);
                break;
            case EMusicState.SawWave:
                SawWave(data, channels);
                break;
            case EMusicState.SquareWave:
                SquareWave(data, channels);
                break;
        }
    }


}

enum EMusicState { SineWave, SawWave, SquareWave }