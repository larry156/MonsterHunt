using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class assignRenderCamera : MonoBehaviour {

    public Canvas theCanvas;
    public Camera theMainCamera;
    string sceneName;

    private void Start()
    {
    }
    // Update is called once per frame
    void Update () {
        //in update so the camera remains used as screenspace even through things like restarting the game
        theMainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        theCanvas.worldCamera = theMainCamera;
        //Scene currentScene = SceneManager.GetActiveScene();
        //sceneName = currentScene.name;
        //if (sceneName != "MainScene")//if it's the main scene
        //{
        //    //in update so the camera remains used as screenspace even through things like restarting the game
        //    theMainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //    theCanvas.worldCamera = theMainCamera;
        //}

    }
}
