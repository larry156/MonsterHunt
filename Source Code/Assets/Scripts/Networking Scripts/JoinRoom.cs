//Ben Boonsiri
//Controls all interactions in the join screen where avaiable games can be found

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class JoinRoom : MonoBehaviour {

    //Declaring
    LobyManager lobbyManager;
    public GameObject prefabForHost;
    public GameObject parentForHost;
    public GameObject[] availableGamesArray;//holds all the games before they are destroyed
    public AudioSource clickNoise;
    

	// Use this for initialization
	void Start () {
        //searches for tag of network manager so it doesn't have to be referenced
        lobbyManager = GameObject.FindGameObjectWithTag("LManager").GetComponent<LobyManager>();
	}

    //Refreshes the list of available games so that if a user presses join when there aren't any games available he doesn't have to reload
    public void onRefreshButtonPressed()
    {
        clickNoise.Play();
        clearJoinGameScreen();
        refreshList();
    }

    //Function used to clear screen when refresh button is pressed so that multiple of the same games aren't shown
    public void clearJoinGameScreen()
    {
        //finds all the avaiable games buttons and puts them in array
        availableGamesArray = GameObject.FindGameObjectsWithTag("LobbyNameButton");
        //destorys all of them in a loop
        for (int i = 0; i < availableGamesArray.Length; i++)
        {
            Destroy(availableGamesArray[i]);
        }
    }

    //finds list of all hosted games
    public void refreshList()
    {
        if (lobbyManager == null)
        {//checks again
            lobbyManager = GameObject.FindGameObjectWithTag("LManager").GetComponent<LobyManager>();
        }
        if (lobbyManager.matchMaker == null)
        {
            lobbyManager.StartMatchMaker();
        }

        //Create list of avaiable matches
        lobbyManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, onMatchList);
    }

    //gets called when a game is found
    private void onMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {   
        //For all the avaiable games
        foreach (MatchInfoSnapshot match in matchList)
        {
            //Creates and puts the avaiable game prefab into the gameObject parentForHost to keep the joinscreen organized
            GameObject listGameObject = Instantiate(prefabForHost);
            //assigns the parent gameObject. False sets orientation and scaling to stay the same
            listGameObject.transform.SetParent(parentForHost.transform,false);

            HostSetup hostSetUp = listGameObject.GetComponent<HostSetup>();
            hostSetUp.setup(match);//calls to the function in host setup script
        }
    }
}
