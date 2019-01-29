/* Name: Larry Y.
 * Date: December 20, 2018
 * Desc: This script allows for players to vote on which map to play on. */
 // Actually, only the host can pick maps now. It's technically still voting, but only one person can vote.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapLoader : NetworkBehaviour {

	//private Dictionary<string, int> mapVotes; // Number of votes for a specific map
	//private Dictionary<string, Button> mapButtons;
	//private int mostVotes;
	private Dictionary<string, string> buttonOfMap; // The button associated with a map name.
	private string curVote; //, lastVote;
	private LobyManager nManagerScript;

	public string[] mapsToUse;

	private void Start()
	{
		nManagerScript = GameObject.FindGameObjectWithTag("LManager").GetComponent<LobyManager>();
		//mapVotes = new Dictionary<string, int>();
		buttonOfMap = new Dictionary<string, string>();
		//mapButtons = new Dictionary<string, Button>();
		// Register buttons associated with map names. Hopefully the map buttons are named properly in the editor.
		for (int i = 0; i < mapsToUse.Length; i++)
		{
			//mapVotes[mapsToUse[i]] = 0;
			buttonOfMap[mapsToUse[i]] = "btn" + mapsToUse[i].Substring(0, mapsToUse[i].Length-3);

			//mapButtons[buttonOfMap[mapsToUse[i]]] = GameObject.Find(buttonOfMap[mapsToUse[i]]).GetComponent<Button>();
			//mapButtons[buttonOfMap[mapsToUse[i]]].onClick.AddListener(delegate { VoteOnMap(mapsToUse[i]); });

			Debug.Log(mapsToUse[i] + "'s button is called " + buttonOfMap[mapsToUse[i]]);
			//Debug.Log(mapButtons[mapsToUse[i]] + " will vote on " + mapsToUse[i]);
		}
		// "Random!" just tells the map picker to choose a random map, and isn't an actual map, so it shouldn't be in mapsToUse.
		buttonOfMap["Random!"] = "btnRandomMap";

		//mapVotes["NULL"] = 0;
		buttonOfMap["NULL"] = "NULL";
		curVote = "NULL";
	}

	// This function is called by the map picker buttons, which are only visible to the server host.
	// It allows the server host to choose which map to play on.
	public void VoteOnMap(string mapName)
	{
		//if (!isLocalPlayer)
		//	return;

		/* This stuff is all legacy code from when voting was actually a thing. */
		//Debug.Log("Someone voted on a map");
		//try
		//{
		//	if (mapVotes[mapName] < nManagerScript.numPlayers)
		//	{
		//		CmdVote(mapName);
		//	}
		//	if (curVote != "NULL" && !GameObject.Find(buttonOfMap[curVote]).GetComponent<Button>().interactable && mapVotes[curVote] > 0)
		//	{
		//		CmdUnVote(curVote);
		//	}
		//}
		//catch (KeyNotFoundException)
		//{
		//	mapVotes[mapName] = 1;
		//}
		//GameObject.Find(buttonOfMap[mapName]).GetComponent<Button>().interactable = false;
		//if (curVote != "NULL")
		//{
		//	GameObject.Find(buttonOfMap[curVote]).GetComponent<Button>().interactable = true;
		//}
		//Debug.Log(mapVotes[mapName] + " votes for " + mapName);
		//foreach (KeyValuePair<string, int> entry in mapVotes)
		//{
		//	if (entry.Value > mostVotes || (entry.Value == mostVotes && mostVotes == 1))
		//	{
		//		mostVotes = entry.Value;
		//		nManagerScript.playScene = entry.Key;
		//		Debug.Log(nManagerScript.playScene + " will be loaded.");
		//	}
		//}
		//curVote = mapName;

		// New improved functionality. Host presses a button, and that map is the map that gets loaded.
		if (mapName == "Random!")
		{
			int mapRoll = Random.Range(0, mapsToUse.Length);
			nManagerScript.playScene = mapsToUse[mapRoll];
			Debug.Log("A random map was chosen.");
		}
		else
		{
			nManagerScript.playScene = mapName;
		}
		Debug.Log("Players will be travelling to " + nManagerScript.playScene);
		// Disable the button that the host just pressed, and allow them to pick their previous map choice if they desire.
		GameObject.Find(buttonOfMap[mapName]).GetComponent<Button>().interactable = false;
		if (curVote != "NULL")
		{
			GameObject.Find(buttonOfMap[curVote]).GetComponent<Button>().interactable = true;
		}
		curVote = mapName;
	}

	//[Command]
	//public void CmdVote(string mapKey)
	//{
	//	mapVotes[mapKey]++;
	//	Debug.Log(mapKey + " now has " + mapVotes[mapKey] + " votes.");
	//}
	//[Command]
	//public void CmdUnVote(string mapKey)
	//{
	//	mapVotes[mapKey]--;
	//	Debug.Log(mapKey + " now has " + mapVotes[mapKey] + " votes.");
	//}
}
