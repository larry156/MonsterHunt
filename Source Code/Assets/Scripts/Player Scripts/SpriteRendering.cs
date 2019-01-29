//Ben Boonsiri
//This sets a players colour for their player model when they enter a game and syncs it with the network
//Helps identify players in the game
//Works by having a colour section of the sprite on top of the basic player sprite
//Top layer changes colour which makes the player look like a new colour, and syncs it to the network so that your colour is the same on everyone elses screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpriteRendering : NetworkBehaviour
{
    //syncs the variable on the network
    [SyncVar] Color theColourToUse;

    //Called when this script is activated in the gamescene
    void Start()
    {
        //changes the colour of sprite to the random colour
        GetComponent<SpriteRenderer>().color = theColourToUse;
    }

    //Called immidiatly when clients network is activated
    public override void OnStartClient()
    {
        if (isServer)//server sets a colour
        {
            //Changes random values to create any colour
            //This used to be a function which picked a number from 0-10 and give that a colour but this way is much cooler
            theColourToUse = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}
