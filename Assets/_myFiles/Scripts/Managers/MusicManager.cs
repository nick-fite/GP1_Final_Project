using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private SimpleSineGenerator generator;
    void Start()
    {
        generator = this.AddComponent<SimpleSineGenerator>();
        this.AddComponent<AudioSource>();
        
    }

    public void PlaySound()
    {
        StartCoroutine(generator.AmplitudeKickWave());
    }

    public SimpleSineGenerator GetGenerator() { return generator; }
}
