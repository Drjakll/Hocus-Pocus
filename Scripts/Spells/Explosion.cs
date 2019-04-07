using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem PS;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PS.isPlaying)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
