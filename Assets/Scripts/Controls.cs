using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float MovementSpeed = 2f;
    public float GunRotation = 3f;

    public Transform RotationPoint;
    
	void Update ()
	{
	    var x = Input.GetAxis("Horizontal");//A & D
	    var y = Input.GetAxis("Vertical");// S & W
	    transform.position += new Vector3(x, 0, 0) * MovementSpeed * Time.deltaTime;

        RotationPoint.Rotate(new Vector3(0, 0,y), Time.deltaTime * GunRotation);

	    if (RotationPoint.localEulerAngles.z > 90 && RotationPoint.localEulerAngles.z < 180)
            RotationPoint.localEulerAngles = new Vector3(0, 0, 90);
	    if (RotationPoint.localEulerAngles.z < 270 && RotationPoint.localEulerAngles.z >= 180)
	        RotationPoint.localEulerAngles = new Vector3(0, 0, 270);

        //else
        //if (RotationPoint.localEulerAngles.z < -90)
        //RotationPoint.localEulerAngles = new Vector3(0, 0, -90);
        /*
        if (y != 0)
        {
            rot.z += y * GunRotation * Time.deltaTime;
            RotationPoint.rotation = rot;

            Debug.Log(RotationPoint.rotation.eulerAngles.z);
        }
        */
    }
}
