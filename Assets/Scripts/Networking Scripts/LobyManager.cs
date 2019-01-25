//Ben Boonsiri
//This is the script for the actual Network Lobby Manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobyManager : NetworkLobbyManager {

    //Declaring
    public GameObject Lobby;
    public GameObject searchRoom;
    public JoinRoom joinRoomScript;
    public GameObject instructionsScreen;
    public AudioSource clickNoise;

    //Shows the main screen only
    private void Start()
    {
        Lobby.SetActive(false);
        searchRoom.SetActive(false);
        instructionsScreen.SetActive(false);
    }

    //Called when player host game
	public override void OnStartHost()
    {
        base.OnStartHost();
        Lobby.SetActive(true);//displays the lobby
	}

	//For when user wants to go back to the start from the menus called from the join scene
	public void RestartScene()
    {
        clickNoise.Play();
        //deactivates all relevant screens
        Lobby.SetActive(false);
        searchRoom.SetActive(false);
        instructionsScreen.SetActive(false);
        joinRoomScript.refreshList();//clears the list of avaiable games so when join button is pressed there aren't like a bunch of the exact same avaiable game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//loads the current scene
    }

}
