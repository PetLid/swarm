using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InitializeSlider : MonoBehaviour {

    public Text minText;
    public Text maxText;
    public Slider slider;

    private string _minValue = "", _maxValue = "";

    void Update ()
    {
        if(!slider.wholeNumbers)
        {
            _minValue = (slider.minValue).ToString() + " %";
            _maxValue = (slider.maxValue * 100).ToString() + " %"; 
        }

        else
        {
            _minValue = slider.minValue.ToString();
            _maxValue = slider.maxValue.ToString();
        }

        minText.text = _minValue.ToString();
        maxText.text = _maxValue.ToString();
    }
}
