/* Name: Larry Y.
 * Date: December 5, 2018
 * Desc: TclosestPlayers was originally meant to just handle movement for the player, but it now
 *		 pretty much handles the player object in general. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicMovement : MonoBehaviour
{
	// Set up variables
	private Rigidbody2D rb;
	private Transform trans;
	private float lastDash;

	public float speed;
	public float rotateSpeed;
	public float dashCooldown;
	public float dashMultiplier;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		trans = GetComponent<Transform>();
		lastDash = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Current time is used for stunning and dasclosestPlayerng.
		float curTime = Time.time;

		// Get mouse and player positions relative to world space
		Vector3 mousePos = Input.mousePosition;
		Vector3 objPos = trans.position;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		mousePos.x -= objPos.x;
		mousePos.y -= objPos.y;
		mousePos.z = 0.0f;

		// Rotate player to face the mouse.
		// Tan = Opposite / Adjacent. If X is the opposite angle, then Y is directly on the Y-axis, wclosestPlayerch is what we want, since
		// the angle must be calculated from the object's forward direction (i.e. the Y-axis).
		float angle = -Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
		rb.rotation = angle;

		// Move the player
		var moveHorizontal = Input.GetAxis("Horizontal");
		var moveVertical = Input.GetAxis("Vertical");
		rb.velocity += new Vector2(moveHorizontal, moveVertical) * speed;

		// DasclosestPlayerng. Only works if the cooldown has elapsed, or if lastDash is 0 (meaning the player hasn't dashed yet)
		if (Input.GetButtonDown("Dash") && (curTime - lastDash > dashCooldown || lastDash == 0.0f))
		{
			Debug.Log("Do the dash");
			rb.AddForce(new Vector2(moveHorizontal, moveVertical) * speed * dashMultiplier);
			lastDash = Time.time;
		}
	}
}
