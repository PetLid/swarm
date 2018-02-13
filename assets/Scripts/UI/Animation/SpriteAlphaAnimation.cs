using UnityEngine;
using System.Collections;

public class SpriteAlphaAnimation : MonoBehaviour {

    // Use this for initialization
    public float speed;
    private float alpha = 0;
    public GameObject sprite;
    	
	// Update is called once per frame
	void OnMouseOver () {

        Color color = sprite.GetComponent<SpriteRenderer>().color;

        if(alpha < 1f)
            alpha += speed;

        color.a = alpha;

        sprite.GetComponent<SpriteRenderer>().color = color;
	}

    void OnMouseExit ()
    {
        Color color = sprite.GetComponent<SpriteRenderer>().color;

        if (alpha > .4f)
            alpha = .4f;

        color.a = alpha;

        sprite.GetComponent<SpriteRenderer>().color = color;
    }
}
