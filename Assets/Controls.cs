using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float MovementSpeed = 2f;
	
	void Update ()
	{
	    var x = Input.GetAxis("Horizontal");
	    transform.position += new Vector3(x, 0, 0) * MovementSpeed * Time.deltaTime;
	}
}
