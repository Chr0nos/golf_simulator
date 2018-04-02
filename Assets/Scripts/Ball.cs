using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public GameObject				arrow;
	new public Camera				camera;
	public Terrain					terrain;
	public bool						crappySpace = false;

	public float					maxForce = 10.0f;
	public float					forceDelta = 50;
	private Rigidbody				rb;
	[HideInInspector]
	public float					shootHeight = 1.0f;
	[HideInInspector]
	public float					currentForce = 0;
	private float					spaceTime;
	private bool					pushing = false;

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
			pushing = false;
			return ;
		}
		shootHeight += Input.GetAxis("Zaxis") * Time.deltaTime;
		// CameraManager.instance.transform.RotateAround(transform.position,
		// 	CameraManager.instance.transform.rotation * Vector3.left, shootHeight * Mathf.Rad2Deg);
		if (CameraManager.instance.freeCam)
			arrow.SetActive(false);
		else {
			arrow.SetActive(true);
			// on tappe dans la balle au lacher de la touche
			Vector3 direction = ComputeCameraDirection();
			UpdateArrow(direction);
			if (crappySpace) {
				if (Input.GetKeyDown(KeyCode.Space))
					SetPushing(!pushing, direction);
			} else {
				if (Input.GetKeyDown(KeyCode.Space))
					SetPushing(true, direction);
				if (Input.GetKeyUp(KeyCode.Space))
					SetPushing(false, direction);
			}
			if (pushing)
				currentForce = Mathf.Min(currentForce + forceDelta * Time.deltaTime, maxForce);
		}
	}

	private Vector3 ComputeCameraDirection() {
		Vector3 direction = transform.position - camera.transform.position;
		direction.y = 0.0f;
		direction.Normalize();
		direction.y = shootHeight;
		return direction;
	}

	private void SetPushing(bool value, Vector3 direction) {
		pushing = value;
		if (!pushing) {
			Shoot(direction);
		}
	}

	private void Shoot(Vector3 direction) {
		rb.AddForce(direction * currentForce, ForceMode.Impulse);
		shootHeight = 0.5f;
		currentForce = 0.0f;
		arrow.SetActive(false);
	}

	private void UpdateArrow(Vector3 direction) {
		Vector3 euler = arrow.transform.eulerAngles;
		float scale = currentForce / maxForce;

		arrow.transform.position = transform.position;
		euler.x = Mathf.Rad2Deg * Mathf.Atan(shootHeight);
		euler.y = camera.transform.eulerAngles.y + 180.0f;
		arrow.transform.eulerAngles = euler;
		scale = scale * 0.75f + 0.25f;
		arrow.transform.localScale = new Vector3(scale, scale, scale);
	}
}
