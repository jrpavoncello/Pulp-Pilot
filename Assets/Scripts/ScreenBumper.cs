using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenBumper : MonoBehaviour
{
    [SerializeField]
    private bool drawInEditor = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }


    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }


    private void DrawGizmos(bool selected)
    {
        if(drawInEditor)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position, .3f);
        }
    }
}