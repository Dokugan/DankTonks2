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
	    var x = Input.GetAxis("Horizontal");
	    var y = Input.GetAxis("Vertical");
	    transform.position += new Vector3(x, 0, 0) * MovementSpeed * Time.deltaTime;

	    RotationPoint.Rotate(new Vector3(0, 0, 90 * y), Time.deltaTime * GunRotation);
        if (RotationPoint.localEulerAngles.z > 90)
            RotationPoint.localEulerAngles = new Vector3(0, 0, 90);
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
