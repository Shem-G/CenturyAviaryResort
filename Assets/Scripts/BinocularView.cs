using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularView : MonoBehaviour {

    public float zoom = 20;
    public float normal = 60;
    public float smooth = 5;

    public GameObject bino;
    private bool isZoomed = false;
    public Camera cam;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        bino.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            isZoomed = !isZoomed;
        }

        if (isZoomed == true)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * smooth);
            bino.SetActive(true);
        }

        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal, Time.deltaTime * smooth);
            bino.SetActive(false);
        }
    }
}

