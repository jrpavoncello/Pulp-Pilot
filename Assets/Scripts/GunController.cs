using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private ParticleSystem gunParticles;

    // Start is called before the first frame update
    void Start()
    {
        this.gunParticles = this.GetComponent<ParticleSystem>();
    }

    private void OnBeginFiring()
    {
        this.gunParticles.Play();
    }

    private void OnStopFiring()
    {
        this.gunParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
