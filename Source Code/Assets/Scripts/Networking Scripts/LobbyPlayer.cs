//Ben Boonsiri
//Script attached to the lobby Player prefab
//this prefab represents everyplayer while their waiting in the lobby and allows them to say their ready to play

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPlayer : NetworkLobbyPlayer {

    //Declaring
    public GameObject parentPref;
    public Button readyButton;
    public Text playerNameText;
    public Text buttonText;
    public GameObject lobbyParentToDisable;
    public AudioSource clickNoise;

    //called when user clicks ready button on their lobbyplayer indicating their ready to play game
    public void onClickReadyButton()
    {
        clickNoise.Play();
        SendReadyToBeginMessage();//sends message to serve to show player is ready to play
        //when all players are ready, they will all be sent to the play scene
        readyButton.enabled=false;//so they can't click it again and tells them to wait
        buttonText.text = "Waiting";
    }

    //called when player enters the game
    //Puts their lobby player that represents them into the gameObject to organize them all
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        parentPref = GameObject.FindGameObjectWithTag("ParentPref");//determines where to be nested
        gameObject.transform.SetParent(parentPref.transform,false);//false sets the orientation and scaling from being huge
        //noteToSelf:Make sure lobbyparent is enabled or it won't work
    }

    //Called when player enters
    //This setup modifies the prefabs elements so there can be one custom to the user in the lobby
    //This allows the user to ready himself, and when all other users ready themselves, the game starts
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (isLocalPlayer)//If player is local
        {
            //Sets up all the prefab changes for the local player
            readyButton.interactable = true;//button to say their ready can now be clicked
            playerNameText.text = "My Player";//tells user that this lobby player is there's
            readyButton.enabled = true;
            buttonText.text = "Ready";
        }
    }

    //Not needed anymore-------------------------------
    //Called every frame
    //void Update()
    //{
    //    // Create a temporary reference to the current scene.
    //    Scene currentScene = SceneManager.GetActiveScene();

    //    // Retrieve the name of this scene.
    //    string sceneName = currentScene.name;

    //    //If the player is in the gamescene
    //    if (sceneName != "MainScene" && sceneName != "DemoMenu")
    //    {
    //        ////disables all the menus by disabling the canvas they are on
    //        //lobbyParentToDisable = GameObject.FindGameObjectWithTag("startScreenCanvas");
    //        //lobbyParentToDisable.SetActive(false);
    //    }
    //}

}
