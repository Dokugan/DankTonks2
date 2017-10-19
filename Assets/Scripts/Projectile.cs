using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(-transform.right * 500);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
