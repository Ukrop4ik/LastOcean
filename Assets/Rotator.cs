using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float rotationSpeed;
    public Vector3 rotationAxis;
    private float angle = 0f;
	// Update is called once per frame
	void Update () {

        transform.Rotate(rotationAxis, Time.deltaTime * rotationSpeed);
		
	}
}
