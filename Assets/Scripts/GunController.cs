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

        StopFiring();
    }

    public void StartFiring()
    {
        ParticleSystem.EmissionModule em = this.gunParticles.emission;

        em.enabled = true;
    }

    public void StopFiring()
    {
        ParticleSystem.EmissionModule em = this.gunParticles.emission;

        em.enabled = false;
    }
}
