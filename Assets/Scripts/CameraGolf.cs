using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGolf : CameraManager {
	public Ball ball;

	public override void OnUpdate()
	{
		Debug.Log("update");
		if (Input.GetKeyDown(KeyCode.O))
			LookAtBall();

		if (Input.GetKeyDown(KeyCode.F))
		{
			freeCam = false;
		}
		if (!freeCam)
			LookAtBall();
	}

	private void LookAtBall()
	{
		Vector3		newpos = ball.transform.position;

		newpos.y += 5;
		newpos.z += 10;
		transform.position = newpos;
		transform.LookAt(ball.transform);
	}
}
