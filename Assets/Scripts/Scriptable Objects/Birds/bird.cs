﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {

    [SerializeField]
    public birdObject BirdData;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(BirdData.BirdName);
        }
    }

}
