using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public GameObject sprite;
    private float radius;
    
    // Use this for initialization
    void Awake()
    {
        radius = (sprite.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2);
    }
    

    public float getRadius()
    {
        return radius;
    }
}
