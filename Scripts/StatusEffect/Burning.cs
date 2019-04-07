using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    private ParticleSystem[] PS;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponentsInChildren<ParticleSystem>();
    }

    public void ChangeIntensity(int Intensity)
    {
        if(Intensity <= 0)
        {
            Object.Destroy(this.gameObject);
        }
        foreach(ParticleSystem p in PS)
        {
            p.maxParticles = Intensity;
        }
    }
}
