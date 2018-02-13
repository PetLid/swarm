using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderValueDialogue : MonoBehaviour {
    
    public Text handleText;
    public Slider slider;
    
    public void Start()
    {
        if (!slider.wholeNumbers)
            handleText.text = System.Math.Round((decimal)slider.value * 100, 0).ToString() + " %";
        else
            handleText.text = slider.value.ToString();
    }

    public void UpdateValue(float t)
    {
        if(!slider.wholeNumbers)
            handleText.text = System.Math.Round((decimal) t*100, 0).ToString() + " %";
        else
            handleText.text = t.ToString();
    }
}
