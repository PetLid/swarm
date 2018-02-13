using UnityEngine;
using System.Collections;

public class RotateAnimation : MonoBehaviour {

    public float speed;
    	
	// Update is called once per frame
	void Update () {       
        gameObject.transform.Rotate(new Vector3(0, 0, -1), speed);
	}
}
