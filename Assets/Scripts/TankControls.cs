using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankControls : MonoBehaviour
{
    public float MaxTorque = 50f;
    private float GunRotation = 30f;
    private float _fireForce = 500;
    private float _maxFireForce = 2000;
    private float _health = 100;
    private Slider _healthbarSlider;

    public WheelCollider[] WheelColliders = new WheelCollider[5];
    public Transform[] TireMeshes = new Transform[5];

    private Transform _rotationPoint;

	// Use this for initialization
	void Start ()
	{
	    gameObject.tag = "Tank";

	    //GetComponent<Rigidbody>().centerOfMass = transform.GetChild(6).position;

        _rotationPoint = transform.GetChild(0);
	    _healthbarSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
	    UpdateHealthbarValue();
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
	            WheelColliders[i].brakeTorque = 10000;
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

    public void DealDamage(float damage)
    {
        if (_health - damage <= 0)
        {
            //TODO: Ga dood
            _health = 0;
        }
        else
        {
            _health -= damage;
        }
        UpdateHealthbarValue();
    }

    public void UpdateHealthbarValue()
    {
        if (_healthbarSlider != null)
            _healthbarSlider.value = _health;
    }
}
