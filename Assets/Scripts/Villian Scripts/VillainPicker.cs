/* Name: Larry Y.
 * Date: January 10, 2019
 * Desc: This script will randomly pick a monster to harass players on the map. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VillainPicker : NetworkBehaviour {

	// Please make sure that all monsters/villains are actually allowed to spawn according to the NetworkManager
	public GameObject[] villainsToUse;

	private void Start ()
	{
		// Pick a random villain from villainsToUse and spawn it on the server.
		int villainRoll = Random.Range(0, villainsToUse.Length);
		GameObject villainInstantiated = Instantiate(villainsToUse[villainRoll], GetComponent<Transform>()); // Make sure whatever object this is attached to isn't like in a wall or something.
		Debug.Log("Spawning a " + villainInstantiated.name);
		NetworkServer.Spawn(villainInstantiated);
	}
}
