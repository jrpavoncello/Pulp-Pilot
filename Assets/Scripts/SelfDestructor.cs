﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, this.timeToDestruct);
    }

    [SerializeField]
    private float timeToDestruct;
}
