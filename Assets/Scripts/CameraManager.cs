using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public float				speed = 1.0f;
	public float				runSpeed = 3.0f;
	public float				maxHeight = 180.0f;
	public bool					freeCam = true;
	public Terrain				terrain;
	private Vector3				lastMousePosition;
	public static CameraManager	instance = null;

	// Use this for initialization
	void Start () {
		lastMousePosition = Vector3.zero;
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	// Update is called once per frame
	void Update () {
		OnUpdate();
		if (!freeCam)
			return ;
		Vector3 motion = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Zaxis"));
		if (Input.GetKeyDown(KeyCode.R))
			transform.rotation = Quaternion.identity;
		if (Input.GetMouseButton(0))
			MouseEvent();
		if (motion == Vector3.zero)
			return ;
		motion.Normalize();
		if (Input.GetKey(KeyCode.LeftControl))
			Rotationate(motion);
		else
			MoveTranslate(motion);
	}

	private void MouseEvent()
	{
		transform.Rotate(Input.GetAxis("Mouse Y"), 0, 0);
		transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X"));
		lastMousePosition = Input.mousePosition;
	}

	private void Rotationate(Vector3 motion)
	{
		Vector3		tmp = motion;
	
		motion = Vector3.zero;
		motion.x = tmp.z;
		motion.z = tmp.y;
		motion.y = tmp.x;
		transform.rotation *= Quaternion.AngleAxis(speed, motion);
	}

	private void MoveTranslate(Vector3 motion)
	{
		bool	error = false;
		Vector3	newpos;

		if (motion == Vector3.zero)
			return ;
		motion = transform.rotation * motion;
		motion *= speed;
		if (Input.GetAxis("Run") > 0)
			motion *= runSpeed;
		newpos = MoveAboveTerrain(transform.position + motion, ref error);
		if (!error)
			transform.position = newpos;
	}

	protected Vector3 MoveAboveTerrain(Vector3 destination, ref bool error)
	{
		float height = terrain.SampleHeight(destination) + 2;

		if (height < maxHeight)
		{
			destination.y = Mathf.Clamp(destination.y, height, maxHeight);
			error = false;
		}
		else
			error = true;
		return (destination);
	}

	virtual public void OnUpdate()
	{
	} 
}
