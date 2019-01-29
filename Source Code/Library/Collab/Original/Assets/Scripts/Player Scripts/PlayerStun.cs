using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{

	// If player presses the stun key while another player is nearby
	private void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetButtonDown("Stun") && other.tag == "Player" && !other.GetComponent<OnlineMovement>().localPlayer)
		{
			Debug.Log("You did the stun");
			Debug.Log(other.name);
			GetComponentInParent<OnlineMovement>().RpcStunPlayer(other.gameObject);
		}
	}
}
