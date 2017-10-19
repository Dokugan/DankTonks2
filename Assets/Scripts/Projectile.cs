using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Rigidbody>().AddForce(transform.right * 500 * -1);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
