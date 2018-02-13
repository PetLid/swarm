using UnityEngine;
using System.Collections;

public class FadeAll : MonoBehaviour {

    public CanvasGroup canvas_group;
    public bool fadeIn = false;
    public float speed;
    public float startAlpha = 1;

    void Start()
    {
        canvas_group.alpha = startAlpha;
    }
	

	public void Update()
    {
        if(!fadeIn && canvas_group.alpha > 0f)
        {
            canvas_group.alpha -= speed;
        }
        else if(fadeIn && canvas_group.alpha < 1f)
        {
            canvas_group.alpha += speed;
        }
    }

    public void setFadeIn(bool fadeIn)
    {
        this.fadeIn = fadeIn;
    }

    public void toggleFadeIn()
    {
        fadeIn = !fadeIn;
    }
}
