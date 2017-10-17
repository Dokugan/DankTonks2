using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    public GameObject Target;
    private Renderer _renderer;

    void Start()
    {
        _renderer = Target.GetComponent<Renderer>();
    }


    void Update()
    {
        var cam = Camera.main;
        var p = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        cam.orthographicSize = _renderer.bounds.size.x  * Screen.height / Screen.width * 0.5f;
        
        cam.transform.position = new Vector3(_renderer.bounds.center.x, _renderer.bounds.max.y, cam.transform.position.z);
        cam.transform.position = new Vector3(cam.transform.position.x, cam.orthographicSize, cam.transform.position.z);
    }

}
