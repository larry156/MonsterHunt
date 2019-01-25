//Ben Boonsiri
//This script is used to track the amount of players in the game for various scoring purposes
//It also displays a GUI of these scores to tell players how there doing
//this includes how many players are left in the game and the position you finished in
//Finally, it also controls the endscreen and it's functionality

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerTracker : NetworkBehaviour {

    //Declaring
    LobyManager lobbyManager;//Reference to the script
    public OnlineCameraScript OnlineCameraScript;//script for the individual's camera
    public GameObject[] lobbyPlayersArray;//holds the players when the gamestart to determine gamesize
    public int amountOfPlayers;//num of players curently in the game
    public bool thePlayerDead = false;
    //objects in game that needs to be manipulated
    public Text playerAmountText;
    public GameObject endScreen;
    public Text finalRankTextGame;
    public Text finalRankTextEndScreen;
    //Sound
    public bool endScreenHasBeenEnabled = false;//used so that the call to stop and play audio is repeated over and over
    public AudioSource clickNoise;
    public AudioSource playerDieNoise;
    public AudioSource endgameSoundTrack;
    private AudioSource[] audioSourcesArray;//array to store audiosources and eventually end them

    // Use this for initialization
    void Start () {
        //Links to the gameObjects in the Unity game
        OnlineCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OnlineCameraScript>();
        lobbyManager = GameObject.FindGameObjectWithTag("LManager").GetComponent<LobyManager>();
        endScreen = GameObject.FindGameObjectWithTag("endScreen");
    }

    // Update is called once per frame
    //Updates the amount of players that are in the game for when people die
    void Update () {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
   
        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        //If it's in a playscreen
        if (sceneName != "MainScene")
        {
            //finds all the lobby players in the game and puts them into an array
            lobbyPlayersArray = GameObject.FindGameObjectsWithTag("Player");
            //finds the amount of players in the array to be used in game
            amountOfPlayers = lobbyPlayersArray.Length;
            //Constantly updates UI component telling user how many players are left in the game
            playerAmountText.text = "Players Left: " + amountOfPlayers;
        }

        //Sets player to spectate
        //Called from here to allign with the amount of players in the game since this is the player tracker
        if (thePlayerDead == true && amountOfPlayers != 0)
        {
            OnlineCameraScript.setPlayerPosition(lobbyPlayersArray[OnlineCameraScript.currentOtherPlayerCameraOn].transform);
        }

        //Called when everyone is dead
        if (amountOfPlayers == 0 && thePlayerDead == true)//and statment is used because without it, networked non-host users would have this function called right when the gamesatarted for some reason
        {
            finalRankTextEndScreen.text = finalRankTextGame.text;
            //The endscreen component allow user to leave and play again
            endScreen.SetActive(true);
            //Call to stop all audio currently playing play endscreen music
            stopAllGameAudioAndPlayEndScreenMusic();
        }
    }

    //For when user wants to restart and reload the menu
    public void RestartGame()
    {
        clickNoise.Play();
        Debug.Log("Attempt to restart game");
        //These two are needed as it allows the user to host a new game by closing the current one
        lobbyManager.StopHost();
        lobbyManager.StopMatchMaker();
        //destorys the current Network Manager so a new one can be created
        Destroy(GameObject.Find("Network Manager"));
        SceneManager.LoadScene(0);//loads the main scene
    }

    //Plays noise upon death
    public void playerDieNoiseCall()
    {
        playerDieNoise.Play();
    }

    //This function searches for all music in the game and ends it, while also playing endscreen music
    public void stopAllGameAudioAndPlayEndScreenMusic()
    {
        //This if statment is needed so it can only be called once and not ruin audio
        if (endScreenHasBeenEnabled == false)
        {
            //Finds all audiosources and puts them into an array
            audioSourcesArray = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            //stops all the audiosources
            foreach (AudioSource theAudio in audioSourcesArray)
            {
                theAudio.Stop();
            }
            endgameSoundTrack.Play();//music
            endScreenHasBeenEnabled = true;
        }
    }
}
