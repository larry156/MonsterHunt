//Not used anymore replaced with online camera script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraScript : MonoBehaviour {

	public GameObject player;
	private Transform playerTrans, cameraTrans;

	// Use this for initialization
	void Start () {
		playerTrans = player.GetComponent<Transform>();
		cameraTrans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		cameraTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y, cameraTrans.position.z);
	}
}
