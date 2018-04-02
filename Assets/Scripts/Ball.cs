using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public GameObject				arrow;
	new public Camera				camera;
	public Terrain					terrain;

	public float					maxForce = 10.0f;
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
		if (Input.GetKeyDown(KeyCode.I))
		{
			rb.velocity = Vector3.zero;
			transform.position = new Vector3(transform.position.x,
											 terrain.SampleHeight(transform.position),
											 transform.position.z);
		}
		if (rb.velocity.magnitude > 0.1) {
			arrow.SetActive(false);
			return ;
		}
		if (CameraManager.instance.freeCam)
			arrow.SetActive(false);
		else {
			arrow.SetActive(true);
			if (Input.GetKey(KeyCode.Space))
				currentForce = Mathf.Min(currentForce + forceDelta * Time.deltaTime, maxForce);
		}
		Vector3 direction = transform.position - camera.transform.position;
		direction.y = 0.0f;
		direction.Normalize();
		direction.y = shootHeight;
		UpdateArrow(direction);
		if (currentForce > 0.0f) {
			if (Input.GetKeyUp(KeyCode.Space)) {
				rb.AddForce(direction * currentForce, ForceMode.Impulse);
				currentForce = 0.0f;
				arrow.SetActive(false);
			}
		}
	}

	private void UpdateArrow(Vector3 direction) {
		Vector3 euler = arrow.transform.eulerAngles;
		float scale = currentForce / maxForce;

		arrow.transform.position = transform.position;
		euler.y = camera.transform.eulerAngles.y + 180.0f;
		arrow.transform.eulerAngles = euler;
		scale = scale * 0.6f + 0.4f;
		arrow.transform.localScale = new Vector3(scale, scale, scale);
	}
}
