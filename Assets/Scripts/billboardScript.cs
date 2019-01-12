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
        Vector3 angle = new Vector3(0, Camera.main.transform.position.y, 0);

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].LookAt(angle, Vector3.up);
        }
	}
}
