using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGolf : CameraManager {
	public Ball ball;
	public float lookupSpeed = 2.0f;
	public float maxHeightFromBall = 3.0f;	
	public float maxDistanceFromBall = 5.0f;
	public float angularSpeed = 45.0f;

	public override void OnUpdate()
	{
		if (Input.GetKeyDown(KeyCode.O))
			LookAtBall();
		if (Input.GetKeyDown(KeyCode.F))
			freeCam = (freeCam == false);
		if (!freeCam)
		{
			transform.RotateAround(ball.transform.position, Vector3.up,
				Time.deltaTime * -angularSpeed * Input.GetAxis("Horizontal"));
			LookAtBall();
		}
	}

	private void LookAtBall()
	{
		Vector3 newpos = transform.position;
		bool error = false;
		float distance = Vector3.Distance(newpos, ball.transform.position);

		if (distance > maxDistanceFromBall)
			newpos = Vector3.MoveTowards(newpos, ball.transform.position,
				Mathf.Min(distance * lookupSpeed * Time.deltaTime, distance - maxDistanceFromBall));
		else {
			if (transform.position.y - maxHeightFromBall > ball.transform.position.y)
				newpos.y = ball.transform.position.y + maxHeightFromBall;
			else if (transform.position.y + maxHeightFromBall < ball.transform.position.y)
				newpos.y = ball.transform.position.y - maxHeightFromBall;
		}
		newpos = MoveAboveTerrain(newpos, ref error);
		if (error)
			return ;
		transform.position = newpos;
		transform.LookAt(ball.transform);
	}
}
