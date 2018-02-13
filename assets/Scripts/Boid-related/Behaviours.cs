using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Behaviours : MonoBehaviour {

    // To avoid division by zero
    private const float SAFEGUARD = .000001f;
    
    /**
     * Generates a velocity pointing towards a goal
     *  based on a boid's position.
     * 
     * @param Boid b, the boid instance
     * @return Vector2, the generated velocity vector
     */
    public static Vector2 Rule_SeekGoal(Boid b, Vector3 goalPos)
    {       
        // Maximum speed 
        float maxSpeed = .01f;

        // Vector from boid to goal
        Vector2 boidToGoal = (Vector2)goalPos - b._pos;

        // Calculate distance
        float distance = boidToGoal.magnitude;

        // Calculates the speed based off of distance, quadratic fall-off
        float baseSpeed = .01f;
        float speed = baseSpeed / (float)(Math.Pow((double)distance, 2) + SAFEGUARD);

        // Limit speed
        speed = System.Math.Min(speed, maxSpeed);

        return boidToGoal * speed;
    }

    /**
     * Generates a velocity pointing away from an obstacle
     *  based on a boid's position.
     * 
     * @param Boid b, the boid instance
     * @return Vector2, the generated velocity vector
     */
    public static Vector2 Rule_AvoidObstacle(Boid b, Vector3 obstaclePos)
    {
        // Vector from boid to obstacle
        Vector2 boidToObstacle = b._pos - (Vector2)obstaclePos;

        // Calculate distance
        float distance = boidToObstacle.magnitude;

        // If distance is smaller than diamater of obstacle sprite
        if (distance < Globals.obstacleRadius * 2)
        {
            // Change velocity based off of distance, quadratic fall-off
            return boidToObstacle / ((distance * distance * 100) + SAFEGUARD);
        }

        // If too far from obstacle return a zero velocity
        return Vector2.zero;
    }

    /**
     * Generates a velocity aligned with the mean velocity of a boid's neighbours.
     * 
     * @param Boid b, the boid 
     * @return Vector2, the generated velocity vector
     */
    public static Vector2 Rule_SteerToMean(Boid b, Boid[] boids, int size)
    {
        // Sum of neighbours velocities
        Vector2 neighbourVelocitySum = new Vector2(0f, 0f);
        float distance;

        int nrNeighbours = 0;
        
        Boid b_k;

        // For every boid k...
        for (int k = 0; k < size; k++)
        {
            b_k = boids[k];

            distance = (b_k._pos - b._pos).magnitude;

            // If distance is smaller than set radius -> add neigbour velocity
            if (b_k != b && distance < 5 * Globals.boidRadius)
            {
                neighbourVelocitySum += b_k._velocity;
                nrNeighbours++;
            }
        }

        // No nearby boids => sum is zero => no influence
        if (neighbourVelocitySum == Vector2.zero)
        {
            return Vector2.zero;
        }

        // Mean velocity of neighbours
        Vector2 meanVelocity = (neighbourVelocitySum) / (nrNeighbours);

        // Return mean velocity of neigbours, subtracting own velocity, divided by a factor to smoothen impact
        return (meanVelocity - b._velocity) / 8;
    }

    /**
     * Generates a vector away from neigbouring boids 
     *  so as to keep boids apart.
     *  
     * @param Boid b, the boid
     * @return Vector2, the generated velocity vector
     */
    public static Vector2 Rule_AvoidCrowding(Boid b, Boid[] boids, int size)
    {
        Boid b_k;
        Vector2 neigboursToBoid = Vector2.zero;
        float distance;
        int nrNeighbours = 0;

        // For every boid k...
        for (int i = 0; i < size; i++)
        {
            b_k = boids[i];

            distance = (b_k._pos - b._pos).magnitude;

            // If distance is smaller than set radius,
            //  add the vector going from boid k to boid b
            if (b_k != b && distance < 1 * Globals.boidRadius)
            {
                neigboursToBoid += ((b._pos - b_k._pos));
                nrNeighbours++;
            }

        }

        // No neighbours => no influence
        if (nrNeighbours == 0)
        {
            return Vector2.zero;
        }

        // Return sum of all vectors going from neighbour to boid, 
        // divided by a factor to smoothen impact
        return neigboursToBoid / (8);
    }
}
