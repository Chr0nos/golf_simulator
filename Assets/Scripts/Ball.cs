using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Camera					camera;

	public float					maxForce = 200.0f;
	public float					forceDelta = 50;
	private Rigidbody				rb;
	[HideInInspector]
	public float					shootHeight = 0.5f;
	[HideInInspector]
	public float					currentForce = 0;
	private float					spaceTime;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 0.1)
			return ;
		if (Input.GetKey(KeyCode.Space))
			currentForce += forceDelta * Time.deltaTime;
		if (Input.GetKeyUp(KeyCode.Space)) {
			Vector3 direction = transform.position - camera.transform.position;
			direction.y = 0.0f;
			direction.Normalize();
			direction.y = shootHeight;
			rb.AddForce(direction * Mathf.Min(currentForce, maxForce), ForceMode.Impulse);
			currentForce = 0.0f;
		}
	}
}
