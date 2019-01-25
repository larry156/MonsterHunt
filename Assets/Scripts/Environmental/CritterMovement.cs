/* Name: Larry Y.
 * Date: December 18, 2018
 * Desc: This script handles critter AI. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CritterMovement : NetworkBehaviour {

	private Rigidbody2D rb;
	private Transform trans;
	private int direction; // 0 is forward, 1 is left, 2 is right.
	private float curTime;
	private float lastTurn;

	public float acceleration;
	public float maxSpeed;
	public float speedRotate;
	public float timeBetweenTurns;
	public string critterName; // Accessed by child objects for debug logging

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		trans = GetComponent<Transform>();
		direction = Random.Range(0, 3);
		lastTurn = 0.0f;
	}

	void FixedUpdate () {
		curTime = Time.time;

		// Change direction every timeBetweenTurns seconds.
		if (curTime - lastTurn > timeBetweenTurns)
		{
			direction = Random.Range(0, 3);
			lastTurn = curTime;
		}
		//direction = 0;

		// Go forwards relative to rotation
		rb.AddRelativeForce(Vector2.up * acceleration);
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

		// Rotate in a certain direction
		if (direction == 1)
		{
			trans.Rotate(0.0f, 0.0f, speedRotate);
		}
		else if (direction == 2)
		{
			trans.Rotate(0.0f, 0.0f, -speedRotate);
		}
	}

	// Rotate a specific amount, and stop moving if the turn exceeds 90 degrees.
	// randRange controls the range of the random number. E.g. if randRange is 45, and degrees is 90, a number between 45 and 135 will be generated.
	public void TurnAround(int degrees, bool rand = true, int randRange = 45)
	{
		// For reference, positive degrees rotate counter-clockwise, negative rotates clockwise.
		int degsToRotate = degrees;
		if (rand)
		{
			degsToRotate = Random.Range(degrees - 45, degrees + 46); // Max is exclusive, hence the +46.
		}
		trans.Rotate(0.0f, 0.0f, degsToRotate);
		if (degsToRotate > 90 || degsToRotate < -90)
		{
			rb.velocity = Vector2.zero;
		}
	}
}
