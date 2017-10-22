using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    void Update()
    {
        var renderer = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Renderer>();
        var cam = Camera.main;
        cam.orthographicSize = renderer.bounds.size.x  * Screen.height / Screen.width * 0.5f;
        cam.transform.position = new Vector3(renderer.bounds.center.x, cam.orthographicSize, cam.transform.position.z);
    }
}
