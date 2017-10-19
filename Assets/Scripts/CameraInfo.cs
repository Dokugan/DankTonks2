using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var cam = Camera.main;
	    Vector3 p = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
    }
}
