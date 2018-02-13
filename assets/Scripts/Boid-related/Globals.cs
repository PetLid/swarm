using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

    public static float boidRadius;
    public static float obstacleRadius;

    public GameObject obstacleObj;
    public GameObject boidObj;

	// Use this for initialization
	void Start () {

        boidRadius = boidObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
        obstacleRadius = obstacleObj.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
    }	
}
