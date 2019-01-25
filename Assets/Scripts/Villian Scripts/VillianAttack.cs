// Boon Boonsiri
// AI abilities and monsters
// Sometime in Deceember 2018 probably
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VillianAttack : NetworkBehaviour {

    private Rigidbody2D villianBody;

    //public CircleCollider2D attackRadius;
    public GameObject attackedPlayer;
    float nextEventTime = 10;
    float period = 4;

    float stalkerTime = 10000; // For stalker
    bool stalkOn = false;
    bool sightOn = false;


    Vector3 toTeleport; // for slenderman
    bool teleportOn = false;

    // Use this for initialization
    void Start() {
        villianBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update() {

    }
    // Finds closets enemy
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    // Sets teleportation time dand phase along with positoin
    public void Slenderman(Vector3 direction)
    {
        nextEventTime = Time.time + period;
        toTeleport = direction;
        teleportOn = true;
        Debug.Log("phase 2");
    }


    // Turns on renderer so becomes visible
    public void Wraith() {

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Turns off renderer
    public void WraithLeave()
    {
        if (name.Contains("Wraith"))
        { gameObject.GetComponent<SpriteRenderer>().enabled = false; }
    }
    // Stalking by setting  aperiod
    public void Stalker()
    {
        if(name.Contains("Stalker"))
        {
            stalkerTime = Time.time + 8; // Stalker Period
            stalkOn = true;
            Debug.Log("Swiggity swooty I'm stalking that booty");
            Debug.Log("You're in my sight");
            // OUTPUT SOMEONE IS BEING STALKED. MAY HAVE BUGS IF THE STALKER KILLS SOMEONE
        }


    }
    // Make sure to keep stalkOn
    public void StalkerInSight()
    {
        if(name.Contains("Stalker")) stalkOn = true;
    }
    // Leave line of sight
    public void StalkerLeave()
    {
        if(name.Contains("Stalker"))
        {
            stalkOn = false;
            Debug.Log("You left");

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }




    public void FixedUpdate()
    {

        if (Time.time > nextEventTime && teleportOn) // Teleportation for Slenderman 
        {
            transform.position = toTeleport;
            teleportOn = false;
        }


        if (stalkOn) // Makes guy stop
        {
            villianBody.velocity = Vector2.zero;
        }

        if (Time.time > stalkerTime && stalkOn) // Will kill if stalk time gets elapsed
        {
            transform.position = FindClosestEnemy().transform.position;
            stalkOn = false;
            stalkerTime += 100;
        }
    }
}