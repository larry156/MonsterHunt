//Ben Boonsiri
//This script is attached to an "avaiable game" prefab object
//and allows the user to click on it and join the game from the join screen
//basically it sets you up to the host and puts you in their lobby

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class HostSetup : MonoBehaviour {
    MatchInfoSnapshot match;
    public Text hostName;
    LobyManager lobbyManager;
    GameObject lobbyParent;
    public AudioSource joinClickNoise;

    // Use this for initialization
    void Start () {
        lobbyManager = GameObject.FindGameObjectWithTag("LManager").GetComponent<LobyManager>();//finds gameobject with the script
        lobbyParent = GameObject.FindGameObjectWithTag("LobbyParent");//This is the gameObject that holds the LobbyScene
    }

    public void setup(MatchInfoSnapshot theMatch)
    {
        match = theMatch;
        hostName.text = match.name;//display name of game to join for the prefab set by the original host
    }

    //When player tries to join an avaiable game and clicks on the prefab
    public void join()
    {
        joinClickNoise.Play();

        //Enables the lobby screen which will now have all the other players
        var thingsToEnable = lobbyParent.GetComponentsInChildren<Transform>(true);
        foreach (var anItem in thingsToEnable)
        {
            anItem.gameObject.SetActive(true);
        }

        //Joins the match
        lobbyManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
    }

}
