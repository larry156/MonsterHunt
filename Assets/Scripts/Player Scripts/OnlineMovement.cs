/* Name: Larry Y. (Actual movement & actions) & Ben B. (Audio, camera, and dying)
 * Date: December 5, 2018
 * Desc: This was originally meant to just handle movement for the player, but it now pretty much handles the player object in general.
 *		  Although, most of the stuff in this script *technically* involves movement, so the name isn't exactly misleading. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnlineMovement : NetworkBehaviour
{
	// Set up variables
	private Rigidbody2D rb;
	private Transform trans;
	private float lastDash;
	private float lastStun;
	[SyncVar] private float moveHorizontal;
	[SyncVar] private float moveVertical;
	private bool debugStunLogged; // This is used so that the console isn't spammed with stun notifications

	// These are public to make things less of a headache.
	[SyncVar] public bool isStunned;
	[SyncVar] public float stunnedAt;

	// These are public so they show up in the inspector.
	public float speed;
	public float dashCooldown;
	public float stunCooldown;
	public float dashMultiplier;
	public float dashThreshold;
	public float stunDuration;
	public Button btnStun, btnDash;
	public GameObject myHUD, stunNotifier;

	// This is Ben's stuff
    public AudioSource dashNoise;
    public AudioSource dieNoise;
    public AudioSource stunNoise;

	void Start()
	{
		// Should be self-explanatory.
		rb = GetComponent<Rigidbody2D>();
		trans = GetComponent<Transform>();
		debugStunLogged = false;
		lastDash = 0.0f;
		lastStun = 0.0f;
		stunnedAt = 0.0f;
		if (isLocalPlayer)
		{
			Debug.Log("I am player " + netId);
		}
		else
		{
			myHUD.SetActive(false); // We don't want other people's HUDs to be cluttering our screen.
		}
	}

	// FixedUpdate is called 30 times per second, and should be used instead of Update for physics.
	void FixedUpdate()
	{
		// So that other players can't control your character.
		if (!isLocalPlayer)
		{
			return;
		}

		// Get mouse and player positions relative to world space
		Vector3 mousePos = Input.mousePosition;
		Vector3 objPos = trans.position;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		mousePos.x -= objPos.x;
		mousePos.y -= objPos.y;
		mousePos.z = 0.0f;

		// Player should only move if they aren't stunned
		if (!isStunned)
		{
			// Rotate player to face the mouse.
			// Tan = Opposite / Adjacent. If X is the opposite side, then Y is directly on the Y-axis, which is what we want, since
			// the angle must be calculated from the object's forward direction (i.e. the Y-axis).
			float angle = -Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
			rb.rotation = angle;

			// Move the player
			moveHorizontal = Input.GetAxis("Horizontal");
			moveVertical = Input.GetAxis("Vertical");
			rb.velocity += new Vector2(moveHorizontal, moveVertical) * speed;

			// Dash if user presses the button
			if (Input.GetButtonDown("Dash") && rb.velocity != Vector2.zero && (Time.time - lastDash > dashCooldown || lastDash == 0.0f))
			{
				Dash();
			}

			// Failsafe disabling of the stun notifier
			if (stunNotifier.activeInHierarchy)
			{
				stunNotifier.SetActive(false);
			}
		}
		else if (isStunned && !debugStunLogged)
		{
			Debug.Log("Am stunned");
			stunNotifier.SetActive(true);
			debugStunLogged = true;
		}
	}

	// Dashing. Only works if the player is moving and the cooldown has either elapsed, or lastDash is 0 (meaning the player hasn't dashed yet)
	public void Dash()
	{
		if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) > Mathf.Abs(dashThreshold) && (Time.time - lastDash > dashCooldown || lastDash == 0.0f))
		{
			// Actual dashing.
			Debug.Log("Do the dash");
			dashNoise.Play();
			rb.AddForce(new Vector2(moveHorizontal, moveVertical) * speed * dashMultiplier);
			lastDash = Time.time;

			// Tell the UI that a dash happened.
			btnDash.GetComponent<UIAbilityButtons>().DoAction();
		}
	}

	// Get or set certain private properties.
	public float CheckLastStun()
	{
		return lastStun;
	}
	public float CheckLastDash()
	{
		return lastDash;
	}
	public void SetLastStun(float time)
	{
		lastStun = time;
	}
	public bool IsControlledPlayer()
	{
		return isLocalPlayer;
	}
	public bool IsServerHost()
	{
		return isServer;
	}

	/* This looks really dumb, but it has to be this way because RPCs can only be called from the server,
	 * which means that clients have to send a command to the server to call the RPC, except you can only call commands from an 
	 * object that the client has authority over, so the command has to be called with a public function, which calls a command, which calls an RPC. */
	 // This gets called by PlayerStun.cs
	public void CallStunFunc(GameObject toStun)
	{
		//updateUI = true;
		//btnStun.interactable = false;
		Debug.Log("toStun is " + toStun.name);
		if (!toStun.GetComponent<OnlineMovement>().isStunned)
		{
			CmdCallRPCStun(toStun.GetComponent<NetworkIdentity>().netId);
		}
	}
	[Command] // Commands are run on the server.
	private void CmdCallRPCStun(NetworkInstanceId netID)
	{
		Debug.Log("A client tried to stun player " + netID);
		RpcStunPlayer(netID);
	}
	[ClientRpc] // RPCs are run by the server on the client.
	private void RpcStunPlayer(NetworkInstanceId netID)
	{
		Debug.Log("Going to stun player " + netID);
		// Find the local player who netID is associated with, and get their script.
		OnlineMovement playerScript = ClientScene.FindLocalObject(netID).GetComponent<OnlineMovement>();
		// Stun them, mark the time they got stunned at, and unstun them after stunDuration seconds.
		playerScript.isStunned = true;
		playerScript.stunnedAt = Time.time;
		playerScript.Invoke("UnstunMe", playerScript.stunDuration);
	}
	
	// Unstunning is also done in a Function->Command->RPC format, because there would be a strange bug otherwise
	// where the command to unstun wouldn't get called, leaving the player "megastunned".
	private void UnstunMe()
	{
		Debug.Log("I was supposed to be stunned for " + stunDuration + " seconds.");
		Debug.Log("I was actually stunned for " + (Time.time - stunnedAt) + " seconds.");
		debugStunLogged = false;
		stunNotifier.SetActive(false);
		CmdUnStun(netId);
	}
	[Command]
	public void CmdUnStun(NetworkInstanceId netID)
	{
		RpcUnStun(netID);
	}
	[ClientRpc]
	public void RpcUnStun(NetworkInstanceId netID)
	{
		OnlineMovement playerScript = ClientScene.FindLocalObject(netID).GetComponent<OnlineMovement>();
		playerScript.isStunned = false;
		Debug.Log("I'm free!"); // Since this is an RPC, this might also get logged on the server, even if the server host wasn't stunned.
	}

	// CmdDie is from Ben.
	//Function is a commmand to the server. this means when the client is destroyed, they send it
	//to the server to show all clients that that player has been destroyed after he is killed
	[Command]
	public void CmdDie()
	{
        dieNoise.Play();
		myHUD.SetActive(false);
		NetworkServer.Destroy(this.gameObject);
	}

	//When a local player is created
	public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //sets the camera to the player when the game starts
        Camera.main.GetComponent<OnlineCameraScript>().setPlayerPosition(gameObject.transform);//sends location data to camera to follow
    }

}
