//Ben Boonsiri
//Controls the camera dedicated to each indivudal networked player.
//Furthurmore, it also controls the Spectate mode which allows dead players to watch their friends from the grave
//Spectate and it's scrolling will update as players drop like flies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineCameraScript : MonoBehaviour {

    //declaring
    private Transform playersTransformLocation;
    private Transform cameraTransform;
    public PlayerTracker PlayerTracker; //the scipt that counts the amount of players in the game
    public Button leftButton;
    public Button rightButton;
    public int currentOtherPlayerCameraOn = 0;
    public AudioSource clickNoise;


    void Start()
    {
        //get refrence for the transform on the camera gameobject
        cameraTransform = GetComponent<Transform>();
        //gets the PlayerTracker script from whatever Canvas is in the the scene
        PlayerTracker = GameObject.FindGameObjectWithTag("CameraCanvas").GetComponent<PlayerTracker>();
    }

    // Update is called once per frame
    void Update () {
		if (playersTransformLocation != null)//when the player is alive and well
        {
            //updates camera position to follow
            cameraTransform.position = new Vector3(playersTransformLocation.position.x, playersTransformLocation.position.y, cameraTransform.position.z);
        }
        //When everyone is dead but one player so you can't spectate anybody else
        if (PlayerTracker.amountOfPlayers == 0 || PlayerTracker.amountOfPlayers == 1)
        {
            rightButton.interactable = false;
            leftButton.interactable = false;
        }
        //Disables so you can't see the minus players who don't exist
        if (currentOtherPlayerCameraOn == 0)
        {
            leftButton.interactable = false;
        }
        //same thing but on the other end
        if (currentOtherPlayerCameraOn == PlayerTracker.lobbyPlayersArray.Length-1)
        {
            rightButton.interactable = false;
        }
        //Automatically sets you to view the lastman standing when there is one
        if (PlayerTracker.amountOfPlayers == 1)
        {
            setPlayerPosition(PlayerTracker.lobbyPlayersArray[0].transform);
        }
        //makes both of the buttons enabled so you can normally view players on the screen
        if (PlayerTracker.amountOfPlayers != 0 && PlayerTracker.amountOfPlayers != 1)
        {
            rightButton.interactable = true;
            leftButton.interactable = true;
        }
    }

    //called when the local player is setup from the onlineMovement script which is where it gets positional data from
    //also called to find other player positions
    public void setPlayerPosition(Transform playerPosition)
    {
        playersTransformLocation = playerPosition;
    }

    //Called when dead player wants to spectate someone else
    public void rightButtonClick()
    {
        //if statment ensures that a button can't be spammed which ruins the paging system of spectate
        if (currentOtherPlayerCameraOn != PlayerTracker.amountOfPlayers-1)
        {
            currentOtherPlayerCameraOn++;
            clickNoise.Play();//sound
        }
        //Assigns the camera to the other player
        setPlayerPosition(PlayerTracker.lobbyPlayersArray[currentOtherPlayerCameraOn].transform);
    }

    //Like rightbuttonclick, but is specating a different player this time
    public void leftButtonClick()
    {
        //if statment ensures that a button can't be spammed which ruins the paging system of spectate
        if (currentOtherPlayerCameraOn != 0)
        {
            currentOtherPlayerCameraOn--;
            clickNoise.Play();//sound
        }
        //Assigns the camera to the other player
        setPlayerPosition(PlayerTracker.lobbyPlayersArray[currentOtherPlayerCameraOn].transform);
    }
}
