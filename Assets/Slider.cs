using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public Transform startPosition = null;
    public Transform endPosition = null;

    MeshRenderer meshRenderer = null;

    public Dial dial = null;

    public bool[] activeColorFromDial = null;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        activeColorFromDial = dial.ActiveColor;
    }

    public void OnSlideStart()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void OnSliderStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void UpdateSlider(float percent)
    {
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, percent);
        //Debug.Log("Percent = " + percent);
        float percentAsBit = percent * 255.0f;
        Mathf.Lerp(0.0f, 255.0f, percentAsBit);
        //Debug.Log("Percent as Bit: " + percentAsBit);

        float rValue = dial.gooColor.color.r;
        float gValue = dial.gooColor.color.g;
        float bValue = dial.gooColor.color.b;
        float aValue = dial.gooColor.color.a;

        if (activeColorFromDial[0] == true)
        {
            rValue = percentAsBit;
            aValue = percent;
        }
        else if (activeColorFromDial[1] == true)
        {
            gValue = percentAsBit;
            aValue = percent;
        }
        else if (activeColorFromDial[2] == true)
        {
            bValue = percentAsBit;
            aValue = percent;
        }

        dial.gooColor.SetColor("_EmissionColor", new Color(rValue, gValue, bValue, aValue));
        dial.gooColor.SetColor("_Albedo", new Color(rValue, gValue, bValue));

        Debug.Log(dial.gooColor.GetColor("_EmissionColor"));
    }
}
