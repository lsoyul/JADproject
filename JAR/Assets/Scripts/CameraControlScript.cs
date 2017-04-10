using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour {

    public Transform cameraTraceTransform;
    Transform cameraTransform;
    Camera cam;

	// Use this for initialization
	void Start () {
        cameraTransform = this.GetComponent<Transform>();
        cam = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        cameraTransform.position = Vector2.Lerp(cameraTransform.position, cameraTraceTransform.position, 0.2f);
        cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -10f);
	}
}
