//Ben Boonsiri
//This controls the majority of the UI on the main screen. 
//This includes all the buttons to enter the different screens

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Declaring
    public InputField theMatchName;//where the host enters game name
    public LobyManager theLobbyManager;
    public GameObject joinRoomObject;
    public AudioSource clickNoise;
    public GameObject instructionsScreen;
    public JoinRoom joinRoom;

    //Funciton for when player hosts game using the button
    public void hostButtonClick()
    {
        clickNoise.Play();//sound
        theLobbyManager.StartMatchMaker();
        //creates the game for people to join
        //as seen theMatchName is derived from the input field enters by the user
        theLobbyManager.matchMaker.CreateMatch(theMatchName.text, (uint)theLobbyManager.maxPlayers, true, "", "", "", 0, 0, theLobbyManager.OnMatchCreate);
    }

    //when player wants to join game using the button
    public void joinButtonClick()
    {
        clickNoise.Play();
        theLobbyManager.StartMatchMaker();
        joinRoomObject.SetActive(true);
        joinRoom = joinRoomObject.GetComponent<JoinRoom>();//Gets the script
        joinRoom.clearJoinGameScreen();//This clears the joinroom list of games to avoid repeated games if userjust came back from the join room and clicks it again
        joinRoom.refreshList();//calls the function from joinRoom script to create the list of avaiable games
    }

    //When player wants to leave
    public void exitButtonClick()
    {
        Application.Quit();//This leaves the game but in testing in unity editor won't work
    }

    //When player needs instructions simply enables it on canvas
    public void instructionsButtonClick()
    {
        instructionsScreen.SetActive(true);
        clickNoise.Play();
    }

    //When player wants to go back from instructions screen it does the opposite
    public void instructionsBackButtonClick()
    {
        instructionsScreen.SetActive(false);
        clickNoise.Play();
    }

    //For when user restarts from the lobby scene. This is more complicated then just reloading the scene because
    //when a player hosts the game they need to stop hosting otherwise if they try to host again they'll get an error.
    public void RestartFromLobby()
    {
        clickNoise.Play();
        //used so that you can start a new create a new match when hosting again
        theLobbyManager.StopHost();
        theLobbyManager.StopMatchMaker();
        Destroy(GameObject.Find("Network Manager"));//destorys the network manager so that a new one can be created
        SceneManager.LoadScene(0);//loads up the mainscene
    }
}
