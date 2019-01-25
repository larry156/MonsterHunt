//Ben Boonsiri
//This script syncs when you die and when other players die
//Additionally, it also enables the GUI that is used to specate (controls for spectate in OnlineCameraScript)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerDie : NetworkBehaviour//work online
{
    //Declaring gameObjects to manipulate and scripts
    public Text statusText;
    public Text finalRankText;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject endScreen;
    public PlayerTracker PlayerTracker; //the scipt that counts the amount of players in the game
    public OnlineCameraScript OnlineCameraScript;//script for the individual's camera

    // Use this for initialization
    void Start()
    {
        //If it is not the current player
        if (!GetComponentInParent<OnlineMovement>().IsControlledPlayer())
        {
            return;
        }
        //If it is the controlled player
        else
        {
            //Assigns all the gameObjects from the scene to the script depending on the Canvas of the current scene
            statusText = GameObject.FindGameObjectWithTag("statusText").GetComponent<Text>();
            finalRankText = GameObject.FindGameObjectWithTag("finalRankText").GetComponent<Text>();
            rightButton = GameObject.FindGameObjectWithTag("rightButton");
            leftButton = GameObject.FindGameObjectWithTag("leftButton");
            endScreen = GameObject.FindGameObjectWithTag("endScreen");

            //Assign the scripts from the specific scenes
            PlayerTracker = GameObject.FindGameObjectWithTag("CameraCanvas").GetComponent<PlayerTracker>();
            OnlineCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OnlineCameraScript>();

            //Disables all the spectate GUI until a player dies
            statusText.gameObject.SetActive(false);
            finalRankText.gameObject.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            endScreen.SetActive(false);
        }


    }

    //calls to the kill command from OnlineMovement script when player runs into monster
    //Player has a seperate touch collider to kill them
    private void OnTriggerEnter2D(Collider2D theCollidy)
    {
        //If they touch a monster
        if (theCollidy.tag == "Monster")
        {
            //If a player died that wasn't you it does nothing
            if (!GetComponentInParent<OnlineMovement>().IsControlledPlayer())
            {
                return;
            }

            //Only called if you the local player is killed
            else
            {
                PlayerTracker.playerDieNoiseCall();
                //Below is the stuff which is displayed to the user when they die
                //Basically enables spectator HUD elements when dead
                endScreen.SetActive(false);
                statusText.gameObject.SetActive(true);
                finalRankText.gameObject.SetActive(true);
                rightButton.SetActive(true);
                leftButton.SetActive(true);

                //Tells you your rank and how well you did when you die
                finalRankText.text = "Final Position: " + PlayerTracker.amountOfPlayers;
                PlayerTracker.thePlayerDead = true;//bool for features in player tracker and online movement script
                GetComponentInParent<OnlineMovement>().CmdDie();//calls to die
            }
        }
    }
}
