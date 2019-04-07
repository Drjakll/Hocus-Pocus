using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAnimation : MonoBehaviour
{
    private ParticleSystem PS;
    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PS.isPlaying)
            Object.Destroy(this.gameObject);
    }
}
