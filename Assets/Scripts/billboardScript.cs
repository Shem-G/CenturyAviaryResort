using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardScript : MonoBehaviour {

    public Transform[] objects;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		for(int i = 0; i < objects.Length; i++)
        {
            objects[i].LookAt(Camera.main.transform.position, Vector3.up);
        }
	}
}
