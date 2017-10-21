using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour
{
    public float MaxTorque = 50f;
    private float GunRotation = 30f;
    private float _fireForce = 500;
    private float _maxFireForce = 2000;

    public WheelCollider[] WheelColliders = new WheelCollider[5];
    public Transform[] TireMeshes = new Transform[5];

    private Transform _rotationPoint;

	// Use this for initialization
	void Start ()
	{
	    _rotationPoint = transform.GetChild(0);

        foreach (var wheel in WheelColliders)
        {
            wheel.steerAngle = 90;
        }
		
	}
	
	// Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();

        var y = Input.GetAxis("Vertical");

        _rotationPoint.Rotate(new Vector3(0, 0, y), Time.deltaTime * GunRotation);

        if (_rotationPoint.localEulerAngles.z > 90 && _rotationPoint.localEulerAngles.z < 180)
            _rotationPoint.localEulerAngles = new Vector3(0, 0, 90);
        if (_rotationPoint.localEulerAngles.z < 270 && _rotationPoint.localEulerAngles.z >= 180)
            _rotationPoint.localEulerAngles = new Vector3(0, 0, 270);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var projectile = (GameObject)Instantiate(Resources.Load("Projectile"));
            projectile.transform.rotation = Quaternion.Euler(0, 0, _rotationPoint.eulerAngles.z - 90);
            var projectileSpawn = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;
            projectile.transform.position = projectileSpawn.transform.position;
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.right * _fireForce * -1);
        }
    }

	void FixedUpdate ()
	{

	    float accelerate = Input.GetAxis("Horizontal");

	    if (accelerate != 0)
	    {
	        for (int i = 0; i < 5; i++)
	        {
	            WheelColliders[i].brakeTorque = 0;
	            WheelColliders[i].motorTorque = accelerate * MaxTorque; 
	        }
	    }
	    else
	    {
	        for (int i = 0; i < 5; i++)
	        {
	            WheelColliders[i].brakeTorque = 1000;
	        }
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

    public void SetForceValue(float value)
    {
        _fireForce = value * _maxFireForce;
    }
}
