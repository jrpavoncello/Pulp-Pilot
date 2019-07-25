using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color fogColor;

    [SerializeField]
    private float fogDensity;

    private Color oldFogColor;
    private float oldFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        oldFogColor = RenderSettings.fogColor;
        oldFogDensity = RenderSettings.fogDensity;
        RenderSettings.fog = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = FogMode.Exponential;
    }

    private void OnTriggerExit(Collider collider)
    {
        RenderSettings.fogColor = oldFogColor;
        RenderSettings.fogDensity = oldFogDensity;
        RenderSettings.fogMode = FogMode.Linear;
    }
}
