using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Net;

public class Swarm : MonoBehaviour
{
    public int maximumCapacity;
    private int numActiveBoids;

    public GameObject boidPrefab;

    private GameObject[] boidGameObject;

    // Array for the boid class instances
    private Boid[] boids;

    public GameObject spawnPointObj;
    public static GameObject goalObj;
    public static GameObject obstacleObj;

    private static Rect ScreenBoundary;

    private System.Random rand = new System.Random();

    private enum BehaviouralRules : int
    {
        seekGoal,
        steerToMean,
        avoidCrowding,
        avoidObstacle,
        SIZE
    };
     


    // Slider references set in Unity
    public Slider sliderGoal,
                  sliderMeanVelocity,
                  sliderPopulation,
                  sliderAvoidCrowding,
                  sliderAvoidObstacle;

    private Slider[] ruleSliders;
    private int numSliders;
    
    // Adherence to each rule
    private float[] adherences;

    // Velocity returned from each rule
    private Vector2[] velocitiesFromRules;
    

    /**
     * Initializes values and reserves space for Boid GameObjects and instantiates boids
     *  with values for position and velocity.
     *  
     */
    void Start()
    {
        // Initialize values for numbers of boids
        maximumCapacity = (int) sliderPopulation.maxValue;
        numActiveBoids = (int) sliderPopulation.value;

        // Reserve space for maximum amount of boids
        boidGameObject = new GameObject[maximumCapacity];
        boids = new Boid[maximumCapacity];
        
        // Initial position and velocity for Boid
        Vector3 pos = new Vector3();
        Vector2 vel = new Vector2();

        // Instantiate all boids
        for (int i = 0; i < maximumCapacity; i++)
        {
            GenerateInitialPositionAndVelocity(i, ref pos, ref vel);

            boidGameObject[i] = Instantiate(boidPrefab, pos, new Quaternion())
                                                as GameObject;

            // Store the boid class instances
            boids[i] = (boidGameObject[i].GetComponent<Boid>());

            // Give instances a velocity
            boids[i]._velocity = vel;

            // Inactivate boids with an index higher than that of the population limit
            if (i >= numActiveBoids)
            {
                boidGameObject[i].SetActive(false);
            }
        }

        // Reserve space
        velocitiesFromRules    = new Vector2[(int)BehaviouralRules.SIZE];
        adherences = new float[(int)BehaviouralRules.SIZE];
        

        // Configure sliders
        numSliders = (int) BehaviouralRules.SIZE;

        ruleSliders = new Slider[numSliders];
        ruleSliders[(int) BehaviouralRules.seekGoal]       = sliderGoal;
        ruleSliders[(int) BehaviouralRules.avoidCrowding]  = sliderAvoidCrowding;
        ruleSliders[(int) BehaviouralRules.avoidObstacle]  = sliderAvoidObstacle;
        ruleSliders[(int) BehaviouralRules.steerToMean]    = sliderMeanVelocity;
        
        // Add listeners to sliders
        for (int i = 0; i < numSliders; i++)
        {
            int id = i;
            ruleSliders[i].onValueChanged.AddListener((value) =>
              {
                  adherences[id] = value;
              }
            );
        }

        // Set adherences
        for (int i = 0; i < numSliders; i++)
        {
            adherences[i] = ruleSliders[i].value;
        }

        // Define screen boundaries
        ScreenBoundary.xMax = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMax, 0, 0)).x;
        ScreenBoundary.xMin = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, 0, 0)).x;
        ScreenBoundary.yMin = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.yMin + .21f, 0, 0)).x;
        ScreenBoundary.yMax = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.yMax - .21f, 0, 0)).x;
    }


    /**
     * Updates the boids according to certain rules. 
     * 
     */
    void Update()
    {   
        // Boid handle
        Boid b;

        float angle;

        // Behaviour update loop
        for (int i = 0; i < numActiveBoids; i++)
        {
            b = boids[i];

            // Update position from game object location
            b._pos = new Vector2(boidGameObject[i].transform.position.x, boidGameObject[i].transform.position.y);

            // Store velocities generated from different rules
            velocitiesFromRules[(int)BehaviouralRules.seekGoal]       = adherences[(int)BehaviouralRules.seekGoal]      == 0 || goalObj     == null ? Vector2.zero : Behaviours.Rule_SeekGoal(b, goalObj.transform.position);
            velocitiesFromRules[(int)BehaviouralRules.avoidObstacle]  = adherences[(int)BehaviouralRules.avoidObstacle] == 0 || obstacleObj == null ? Vector2.zero : Behaviours.Rule_AvoidObstacle(b, obstacleObj.transform.position);
            velocitiesFromRules[(int) BehaviouralRules.steerToMean]   = adherences[(int)BehaviouralRules.steerToMean]   == 0                        ? Vector2.zero : Behaviours.Rule_SteerToMean(b, boids, numActiveBoids);
            velocitiesFromRules[(int) BehaviouralRules.avoidCrowding] = adherences[(int)BehaviouralRules.avoidCrowding] == 0                        ? Vector2.zero : Behaviours.Rule_AvoidCrowding(b, boids, numActiveBoids);
                            
            
            for (int j = 0; j < (int)BehaviouralRules.SIZE; j++)
            {
                b._velocity += (velocitiesFromRules[j] * adherences[j]) * Time.deltaTime * 100;
            }
            

            LimitVelocity(ref b._velocity);

            boidGameObject[i].gameObject.transform.Translate(b._velocity);
            
            // Keeps boids within the screen
            StayWithinLimits(ref b);


            // Get angle from velocity 
            angle = Mathf.Atan2(b._velocity.y, b._velocity.x) * Mathf.Rad2Deg - 90;
            
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Rotate sprite according to velocity, with smoothing
            float smoothing = .2f; // 1 - No smoothing, 0 - Infinite
            b._sprite.transform.rotation = Quaternion.Slerp(b._sprite.transform.rotation, rotation, smoothing);
            
        }
    }


    /**
     * Adjusts the number of active boids.
     * 
     * @param float t, the population limit
     */
    public void ChangePopulation(float nr)
    {
        // Avoid attempts at changing population before swarm is instantiated
        if (isActiveAndEnabled == true)
        {
            int poplimit = (int)nr;

            // Inactivate all boids above population limit
            if (poplimit < numActiveBoids)
            {
                for (int i = poplimit; i < numActiveBoids; i++)
                {
                    (boidGameObject[i]).SetActive(false);
                }
            }
            // Reactivate boids and give new positions and velocities, "respawning" them
            else if (poplimit > numActiveBoids)
            {
                Vector3 pos = new Vector3();
                Vector2 vel = new Vector2();

                for (int i = numActiveBoids; i < poplimit; i++)
                {
                    boidGameObject[i].SetActive(true);

                    GenerateInitialPositionAndVelocity(i, ref pos, ref vel);

                    boidGameObject[i].transform.position = pos;
                    boids[i]._velocity = vel;
                }
            }

            // Finally, update the value for number of active boids
            numActiveBoids = poplimit;
        }
    }

    /**
     * Generates values for position based on the index so that boids
     *  start in a circle around the spawn point position.
     * Also generates values for initial velocity.
     * 
     * @param int index, the index of the boid.
     * @param ref Vector3 pos, store position generated in here.
     * @param ref Vector2 vel, store velocity generated in here.
     */
    private void GenerateInitialPositionAndVelocity(int index, ref Vector3 pos, ref Vector2 vel)
    {
        Vector2 spawnPos = spawnPointObj.transform.position;

        pos = new Vector3((float)(spawnPos.x + (Math.Cos((index * 360) / (numActiveBoids + 1)) * rand.Next(0, 100) * .01f * (spawnPointObj.GetComponent<SpawnPoint>().getRadius() - Globals.boidRadius))),
                          (float)(spawnPos.y + (Math.Sin((index * 360) / (numActiveBoids + 1)) * rand.Next(0, 100) * .01f * (spawnPointObj.GetComponent<SpawnPoint>().getRadius() - Globals.boidRadius))),
                          Camera.main.farClipPlane);

        vel = new Vector2(rand.Next(-100, 100) * .002f, rand.Next(-100, 100) * .002f);
    }

    /**
     * If a boid flies near a screen border, accelerate it away fromborder 
     * 
     * @param Boid b, the boid
     */
    private void StayWithinLimits(ref Boid b)
    {
        Vector3 position = b.transform.position;
        Vector2 velocityX = new Vector2();
        Vector2 velocityY = new Vector2();
        float strenght = .001f;

        float margin = 5 * Globals.boidRadius;
        float limVelocity = .0005f;

        float distanceX = 0f;
        float distanceY = 0f;
        
        // Left side of screen
        if ((distanceX = position.x - ScreenBoundary.xMin) < margin)
        {
            velocityX = new Vector2(1, 0);
        }

        // Right side of screen
        else if ((distanceX = ScreenBoundary.xMax - position.x) < margin)
        {
            velocityX = new Vector2(-1, 0);
        }

        // Bottom side of screen
        if ((distanceY = position.y - ScreenBoundary.yMin) < margin)
        {
            velocityY = new Vector2(0, 1);
        }

        // Top side of screen
        else if ((distanceY = ScreenBoundary.yMax - position.y) < margin)
        {
            velocityY = new Vector2(0, -1); 
        }

        if(velocityX != Vector2.zero)
        {
            distanceX = distanceX < 0 ? .1f : distanceX;           
            velocityX *= strenght / ((distanceX * distanceX) + limVelocity);
        }

        if(velocityY != Vector2.zero)
        {
            distanceY = distanceY < 0 ? .1f : distanceY;
            velocityY *= strenght / ((distanceY * distanceY) + limVelocity);
        }

        b._velocity += (velocityX + velocityY) / 4;
    }
    

   /**
    * If a velocity is too high, decrease it to a certain limit
    * 
    * @param Vector2 velocity, the velocity to limit
    */
    private void LimitVelocity(ref Vector2 velocity)
    {
        float maxSpeed = .25f;

        if(velocity.magnitude > maxSpeed)
        {
            velocity = (velocity / velocity.magnitude) * maxSpeed; 
        }
    }

    public static void SetGoalObj(GameObject obj)
    {
        goalObj = obj;
    }

    public static void SetObstacleObj(GameObject obj)
    {
        obstacleObj = obj;
    }   
}
