using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour
{
    public float MaxTorque = 25f;

    public WheelCollider[] WheelColliders = new WheelCollider[5];
    public Transform[] TireMeshes = new Transform[5];

	// Use this for initialization
	void Start () {

        foreach (var wheel in WheelColliders)
        {
            wheel.steerAngle = 90;
        }
		
	}
	
	// Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();
    }

	void FixedUpdate ()
	{

	    float accelerate = Input.GetAxis("Horizontal");

	    for (int i = 0; i < 5; i++)
	    {
	        WheelColliders[i].motorTorque = accelerate * MaxTorque;
	    }

    }

    void UpdateMeshesPositions()
    {
        for (int i = 0; i < 5; i++)
        {
            Quaternion quat;
            Vector3 pos;
            WheelColliders[i].GetWorldPose(out pos, out quat);

            TireMeshes[i].position = pos;
            //TireMeshes[i].rotation = quat;
        }
    }
}
