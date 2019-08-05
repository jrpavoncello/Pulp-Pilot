using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EffectsSpawner : MonoBehaviour
{
    public void SpawnEffects()
    {
        foreach (var deathEffect in this.deathEffects)
        {
            var newEffects = Instantiate(deathEffect, this.transform.position, Quaternion.identity);

            newEffects.transform.parent = this.runtimeSpawnObject.transform;

            newEffects.SetActive(true);
        }
    }

    [SerializeField]
    private List<GameObject> deathEffects;

    [SerializeField]
    private GameObject runtimeSpawnObject;
}
