// Boon Boonsiri
// Movement and like everything for AI
// Sometime in Deceember 2018 probably
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VillianMovement : NetworkBehaviour
{

    private Rigidbody2D villianBody;


    // Coordinates of where the guy is currently going to 
    Vector2 hunt = new Vector2(10f,10f);
    Vector2 rayCastCoordinate = new Vector2(0f, 0f);
    Vector2 curDirection = new Vector2(); // Final direction to go

    // Hunt times and actoin times
    float nextEventTime = 10.0f;
    float nextHuntTime = 0f;
    float period = 10;

    // For helping it get unstuck
    float stuckTime = 5;
    Vector3 stuckLocation;
    float stuckDistance = 5;
    bool isStuck = false;

    float houseTime = 1000f;
    bool inHouse = false;

    // D Day scenario stuck
    float doubleStuckTime = 0;

    // Helps with raycasting
    bool raycastOn = false;
    bool sightOn = false;
    bool turnOn = false;
    int raycastSide = 0;
    int turnDirection = 1;
    // Turning stuff on and off
    public bool raycast = true;  // Only for ghost really
    public bool escapeRoute = true; // Escape if stuff in house

    // Action stuff
    public float actionMultiplier;
    public float huntMultiplier;
    float hearingMultiplier;
    public float maxSpeed;
    public float acceleration; // Change to modify speed

    // Pre:
    // Post: Returns game object of closet player
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player"); // Find closest with tag
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
        return closest; // returns gameobject of closet item with tag player
    }

    // Pre
    // Post: returns position of closest escape
    public GameObject FindClosestEscape()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Escape"); // Does the same with game object tagged escape
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
        return closest; // use this for escape route
    }

    // Use this for initialization
    void Start()
    {
        // gets the villian body
        villianBody = GetComponent<Rigidbody2D>();
        GameObject closestPlayer = new GameObject();
        hunt = closestPlayer.transform.position; // Find closet player off start
        if (raycastOn == false)
        {
            curDirection = hunt; // Hunts player right away
        }

        //stuckLocation = transform.position;
        Debug.Log(name);

        if (name.Contains("Wraith"))
        {
            GetComponent<SpriteRenderer>().enabled = false; // Turn invisible off the start
        }

    }
 
    void RayCast()
    {
        //float cast = 1; // For math because I need cast rule apparently
        // Variables
        float distanceLeft = Mathf.Infinity; // Sets it if there is no hit
        float distanceRight = Mathf.Infinity;
        float rayCastDistance = 4; // Distance
        float angle = 90; // two difference angles
        float angle2 = 30;
        float offset = 2f;

        //if (villianBody.transform.position.x > hunt.x)
        //{
          //  cast = -1; // Change to negative one for cast
        //}
        int layermask = 3; // Mask out itself and things not to raycast so it doesn't raycast itself
        // Declaring various rays
        //Ray2D centerRaycast = new Ray2D(); 
        //centerRaycast.origin = villianBody.transform.position;
        //centerRaycast.direction = (new Vector3(hunt.x, hunt.y) - villianBody.transform.position).normalized * rayCastDistance*10;
        //RaycastHit2D centerRaycastHit = new RaycastHit2D();

        Ray2D forwardRaycast = new Ray2D(); // Declaring raycast
        forwardRaycast.origin = villianBody.transform.position; // Origin
        forwardRaycast.direction = villianBody.transform.up; // Directoin
        RaycastHit2D forwardRaycastHit = new RaycastHit2D(); // two different hits
        RaycastHit2D forwardRaycastHit2 = new RaycastHit2D();

        // THE COMMENTED OUT STUFF WAS MY ORIGINAL CODE AND IT REQUIRED SO MUCH THOUGHT THAT I COULDN"T BRING MYSELF TO DELETE IT.
        Ray2D leftRaycast = new Ray2D();
        leftRaycast.origin = villianBody.transform.position;
        //leftRaycast.direction = new Vector2(Mathf.Sin((Vector2.Angle(forwardRaycast.direction, Vector2.up) - angle*cast) * Mathf.PI *cast/ 180) * forwardRaycast.direction.magnitude, Mathf.Cos((Vector2.Angle(forwardRaycast.direction, Vector2.up) - angle*cast) * Mathf.PI *cast/ 180) * forwardRaycast.direction.magnitude); 
        leftRaycast.direction = Quaternion.AngleAxis(angle, Vector3.forward) * transform.up * rayCastDistance; //Raycast at 90 degree angle left
        RaycastHit2D leftRaycastHit = new RaycastHit2D();

        Ray2D rightRaycast = new Ray2D();
        rightRaycast.origin = villianBody.transform.position;
        //rightRaycast.direction = new Vector2(Mathf.Sin((Vector2.Angle(forwardRaycast.direction, Vector2.up) + angle*cast) * Mathf.PI *cast/ 180) * forwardRaycast.direction.magnitude, Mathf.Cos((Vector2.Angle(forwardRaycast.direction, Vector2.up) + angle*cast) * Mathf.PI*cast / 180) * forwardRaycast.direction.magnitude);
        rightRaycast.direction = Quaternion.AngleAxis(-angle, Vector3.forward) * transform.up * rayCastDistance; // right
        RaycastHit2D rightRaycastHit = new RaycastHit2D();

        Ray2D left30Raycast = new Ray2D();
        left30Raycast.origin = villianBody.transform.position;
        //left30Raycast.direction = new Vector2(Mathf.Sin((Vector2.Angle(forwardRaycast.direction, Vector2.up) - angle2*cast) * Mathf.PI * cast / 180) * forwardRaycast.direction.magnitude, Mathf.Cos((Vector2.Angle(forwardRaycast.direction, Vector2.up) - angle2*cast) * Mathf.PI*cast / 180) * forwardRaycast.direction.magnitude);
        left30Raycast.direction = Quaternion.AngleAxis(angle2, Vector3.forward) * transform.up * rayCastDistance; // left 30
        RaycastHit2D left30RaycastHit = new RaycastHit2D();

        Ray2D right30Raycast = new Ray2D();
        right30Raycast.origin = villianBody.transform.position;
        //right30Raycast.direction = new Vector2(Mathf.Sin((Vector2.Angle(forwardRaycast.direction, Vector2.up) + angle2*cast) * Mathf.PI *cast / 180) * forwardRaycast.direction.magnitude, Mathf.Cos((Vector2.Angle(forwardRaycast.direction, Vector2.up) + angle2*cast) * Mathf.PI *cast/ 180) * forwardRaycast.direction.magnitude);
        right30Raycast.direction = Quaternion.AngleAxis(-angle2, Vector3.forward) * transform.up * rayCastDistance;
        RaycastHit2D right30RaycastHit = new RaycastHit2D();

        //Debug.DrawRay(forwardRaycast.origin, Quaternion.AngleAxis(30, Vector3.forward) * transform.up * rayCastDistance, Color.green);
        //Debug.Log(Raycast.direction + " center");
        //Debug.Log(leftRaycast.direction + " left");
        //Debug.Log(rightRaycast.direction + " right");

        // Sends the two raycast, at slightly off from the actual transform
        forwardRaycastHit = Physics2D.Raycast(forwardRaycast.origin + new Vector2(transform.right.x, transform.right.y), forwardRaycast.direction, rayCastDistance, layermask);
        forwardRaycastHit2 = Physics2D.Raycast(forwardRaycast.origin - new Vector2(transform.right.x, transform.right.y), forwardRaycast.direction, rayCastDistance, layermask);

        // Debugging
        Debug.DrawRay(forwardRaycast.origin + new Vector2(transform.right.x, transform.right.y), forwardRaycast.direction.normalized * rayCastDistance, Color.blue);
        Debug.DrawRay(forwardRaycast.origin - new Vector2(transform.right.x, transform.right.y), forwardRaycast.direction.normalized * rayCastDistance, Color.blue);
        Debug.DrawRay(rightRaycast.origin - new Vector2(transform.up.x, transform.up.y)*offset, rightRaycast.direction.normalized * rayCastDistance, Color.cyan);
        Debug.DrawRay(leftRaycast.origin - new Vector2(transform.up.x, transform.up.y)*offset, leftRaycast.direction.normalized * rayCastDistance, Color.green);
        Debug.DrawRay(transform.position, right30Raycast.direction.normalized * rayCastDistance, Color.red);
        Debug.DrawRay(transform.position, left30Raycast.direction.normalized * rayCastDistance, Color.red);
        //Debug.DrawLine(forwardRaycast.origin, forwardRaycastHit.point);


        // If it hits something that isn't hte player
        if (forwardRaycastHit && forwardRaycastHit.collider.gameObject.tag != "Player") // why not if (!(false == centerRaycastHit))
        {
            //Debug.Log(forwardRaycastHit.collider);
            raycastOn = true;
        }
        if (forwardRaycastHit2 && forwardRaycastHit2.collider.gameObject.tag != "Player")
        {
            raycastOn = true;
        }
        if (raycastOn) {
            // Raycast all side rays
            if(turnDirection != 1) leftRaycastHit = Physics2D.Raycast(leftRaycast.origin - new Vector2(transform.up.x, transform.up.y)*offset, leftRaycast.direction, rayCastDistance, layermask);
            if(turnDirection != -1) rightRaycastHit = Physics2D.Raycast(rightRaycast.origin - new Vector2(transform.up.x, transform.up.y)*offset, rightRaycast.direction, rayCastDistance, layermask);
            left30RaycastHit = Physics2D.Raycast(left30Raycast.origin, left30Raycast.direction, rayCastDistance, layermask);
            right30RaycastHit = Physics2D.Raycast(right30Raycast.origin, right30Raycast.direction, rayCastDistance, layermask);

            //Debug.DrawLine(leftRaycast.origin, leftRaycastHit.point, Color.magenta);
            //Debug.DrawLine(rightRaycast.origin, rightRaycastHit.point, Color.cyan);

            if (left30RaycastHit && raycastOn && left30RaycastHit.collider.gameObject.tag != "Player")
            {
                // Gets distsance
                distanceLeft = GetDistance(villianBody.transform.position.x, villianBody.transform.position.y, left30RaycastHit.point.x, left30RaycastHit.point.y);
            }

            if (right30RaycastHit && raycastOn && right30RaycastHit.collider.gameObject.tag != "Player")
            {
                distanceRight = GetDistance(villianBody.transform.position.x, villianBody.transform.position.y, right30RaycastHit.point.x, right30RaycastHit.point.y);
            }
            // SEts which distance is farther or closer
            if (distanceLeft > distanceRight && raycastOn && !turnOn)
            {
                //villianBody.transform.Rotate(0, 0, angle2); // Correct
                //villianBody.transform.Rotate(0, 0, -angle2);
                //Debug.Log("Going left");
                // Figures out which tway to turn
                Debug.Log("Should be left");
                turnDirection = 1; // Turns only in nessesary direction
                turnOn = true;

            }
            else if (distanceRight >= distanceLeft && raycastOn && !turnOn)
            {
                //villianBody.transform.Rotate(0, 0, -angle2);
                //Debug.Log("Right");
                turnDirection = -1;
                turnOn = true;
            }
            if (turnDirection == 1) Debug.Log("Turning lefto");
            if (turnDirection == -1) Debug.Log("Turning Righto");
            villianBody.transform.Rotate(0, 0, angle2 * turnDirection); // negative is right

            // IF Raycast hits
            if (leftRaycastHit && leftRaycastHit.collider.gameObject.tag != "Player")
            {
                raycastOn = false; // If hit sides then it past obstsacles
            }

            if (rightRaycastHit && rightRaycastHit.collider.gameObject.tag != "Player")
            {
                raycastOn = false;
            }

            // One method of checking if monster is stuck
            if (forwardRaycastHit && forwardRaycastHit2 && leftRaycastHit && rightRaycastHit && left30RaycastHit && right30RaycastHit && isStuck)
            {
                escapeRoute = true;
                doubleStuckTime = Time.time + 10f;
            }

        }
    }
    //pre
    //post: calls wraith function if wraith, makes ai hunt mroe
    public void Hearing() {
        hearingMultiplier = 0.2f; // Hunts more often
        if (name.Contains("Wraith")) // If wraith then will turn invisble
        {
            Debug.Log("on 0.5");
            gameObject.GetComponent<VillianAttack>().Wraith();
        }
    }
    // pre: gameobject of player
    // post: Raycast and decides if there is something on the way. if not then sight on
    public void Sight(GameObject theCollidy) {
        if (theCollidy.tag == "Player")
        {
            RaycastHit2D hit = new RaycastHit2D();
            hit = Physics2D.Raycast(villianBody.transform.position, (theCollidy.transform.position - villianBody.transform.position), 100); // raycast to see if something is in the way
            Debug.DrawRay(transform.position, (theCollidy.transform.position - transform.position)*20f, Color.black);
            //Debug.Log(hit.collider.gameObject.tag);
            if (hit)
            {
                Debug.Log(hit.collider.gameObject.name);
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                // Make sure it sees player 
                if (hit.collider.gameObject.GetComponentInParent<OnlineMovement>() != null || hit.collider.gameObject.CompareTag("Player"))
                {
                    // if not sight on
                    //Debug.Log("SightON");
                    sightOn = true; // Sight overwrites everything
                    raycastOn = false;
                    escapeRoute = false;
                    isStuck = false;
                    if(!name.Contains("Ghost"))gameObject.GetComponent<VillianAttack>().StalkerInSight();
                    curDirection = theCollidy.transform.position;
                    rotateToward(); // Rotates towards player

                }
                else
                {
                    // something got in the way of stalker so turns off
                    if (name != "Ghost" || name != "Ghost(Clone)") gameObject.GetComponent<VillianAttack>().StalkerLeave(); // turns stalker off
                }

            }
        }

    }

    // pre:
    // post: Rotates towards vector direction
    void rotateToward()
    {
        float rotation; 
        Vector3 direction;
        direction = (new Vector3(curDirection.x, curDirection.y) - villianBody.transform.position).normalized;
        rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        villianBody.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
        
    }

    // Returns player distance from villian
    // Pre: px, py are player x player y. Vx vy are villian x villian y. dx dy is delta x and delta y 
    float GetDistance(float px, float py, float vx, float vy)
    {
        float distance, dx, dy;

        dx = px - vx;
        dy = py - vy;

        distance = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
        return distance;
    }
    // Update is called once per frame
    // pre:
    // Post: Calls actions for different monsters 
    void action()
    {
        // Where all the action is called. 
        if (name == "Slenderman" || name == "Slenderman(Clone)") // Slenderman no duh
        {
            if (!sightOn)
            {
                Vector3 teleport = FindClosestEnemy().transform.position; // Finds closest player and teleports them
                gameObject.GetComponent<VillianAttack>().Slenderman(teleport); // calls function
            }
        }
    }

    // Checks for if villain is in or out of house
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Entrance"))
        {
            Debug.Log("Enter House");
            houseTime = Time.time + 7;
            inHouse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Entrace")){
            Debug.Log("Exit house");
            houseTime = Time.time + 1000;
            inHouse = false;
        }
    }

    void FixedUpdate()
    {
        //curDirection = new Vector2(0, 0); //  Have this for now or else will crash
        GameObject closestPlayer = FindClosestEnemy();  // Find closest player
        hunt = closestPlayer.transform.position;

        if (GetDistance(transform.position.x, transform.position.y, FindClosestEscape().transform.position.x, FindClosestEscape().transform.position.y) < 3 || sightOn)
        {
            // Get's the monster free from being stuck in a house
            escapeRoute = false;
            isStuck = false;
            raycastOn = false;
            curDirection = FindClosestEnemy().transform.position; // finds closest player
        }
        // Prevents spinning
        if (Time.time > stuckTime)
        {
            stuckTime = Time.time + 3; // if spinning makes sure that it stops spinning essentially

            if (GetDistance(transform.position.x, transform.position.y, stuckLocation.x, stuckLocation.y) < 1)
            {
                isStuck = true;
            }
            stuckLocation = transform.position; // gets new previous location
        }

        // For houses
        if(inHouse && sightOn)
        {
            houseTime = Time.time + 7; // won't become stuck if player in sight
        }
        if(Time.time > houseTime && inHouse)
        {
            escapeRoute = true; // if can't find player and in house
            doubleStuckTime = Time.time + 10;
        }

        if (Time.time > nextHuntTime) // Hunting 
        {
            nextHuntTime += period * huntMultiplier * hearingMultiplier;
            // set next hunt time
            if (raycastOn == false)
            {
                curDirection = hunt; // sets direction of hunt
                hearingMultiplier = 1;
            }
        }

        if (Time.time > nextEventTime) // Actions 
        {   
            nextEventTime += period * actionMultiplier;
            action(); // action time
        }

        if (huntMultiplier == 0) { curDirection = hunt; } // For the ghost because apparently unity didn't like really small numbers

        if (!raycastOn) { rotateToward(); } // Rotates towards curDirection


        if (raycast && sightOn == false && !escapeRoute) RayCast(); // Raycast mainly there for ghost

        // Turn off turning
        RaycastHit2D Raycast = new RaycastHit2D();
        Raycast = Physics2D.Raycast(transform.position, transform.up, 1000,3);
        //Debug.DrawRay(transform.position, transform.up*1000);
        if (Raycast)
        {
            // if something is in the way, 
            if (Raycast.collider.gameObject.GetComponentInParent<OnlineMovement>() != null || Raycast.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("I am stopping turning"); // overall only allows for unidirectional turning until a player
                turnOn = false;
                escapeRoute = false;

                if (inHouse)
                {
                    houseTime = Time.time + 10;
                }
            }
        }
        if (raycastOn)
        {
            if (name.Contains("Stalker")) GetComponent<VillianAttack>().StalkerLeave();
        }

        // memory location        // Help Escape
        if (escapeRoute == true)
        {
            curDirection = FindClosestEscape().transform.position; 
            isStuck = false;
            raycastOn = false;
            turnOn = false;
        }


        // Go to location
        villianBody.AddRelativeForce(Vector2.up * acceleration); // Adds acceleration and moves monster
        villianBody.velocity = Vector2.ClampMagnitude(villianBody.velocity, maxSpeed); // Movement 
        //villianBody.transform.Translate(Vector2.up * speed* Time.deltaTime * speedMultiplier);
        sightOn = false; // sight on off after movement

        if (isStuck)
        {
            // turn everything off when stuck
            Debug.Log("Unstucker");
            sightOn = true;
            isStuck = false;
            raycastOn = false;
            turnOn = false;
            //villianBody.AddRelativeForce(-Vector2.up * acceleration*1000); // Let's not have this because it breaks sometimes

        }

        if (Time.time > doubleStuckTime)
        {
            // D Day aka really badly stuck
            transform.position = new Vector3(0, 0);
            raycastOn = false;
            escapeRoute = false;
            isStuck = false;
            turnOn = false;
            curDirection = hunt;
        }
        Debug.Log(inHouse);
        if (escapeRoute == false)
        {
            doubleStuckTime = Time.time + 100f;
        }

    }
}

