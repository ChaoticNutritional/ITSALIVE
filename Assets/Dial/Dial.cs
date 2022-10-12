using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class Dial : MonoBehaviour
{
    Vector3 m_startRotation;

    MeshRenderer meshRenderer = null;

    public Material gooColor = null;

    public float activeColorInGoo;

    public bool[] ActiveColor = new bool[3];


    //segment rotation into 3 parts each corresponding to RGB values;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StartTurn()
    {
        m_startRotation = transform.localEulerAngles;
        //meshRenderer.material.SetColor("_Color", new Vector4(Mathf.Clamp01(219), Mathf.Clamp01(112), Mathf.Clamp01(147), 1));
    }

    public void StopTurn()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void DialUpdate(float angle)
    {
        Vector3 angles = m_startRotation;
        angles.z += angle;
        transform.localEulerAngles = angles;

        //Debug.Log(transform.eulerAngles.z);
       

        if (transform.eulerAngles.z <= 300 && transform.eulerAngles.z >= 180.0f)
        {
            meshRenderer.material.SetColor("_Color", Color.blue);
            //Debug.Log("Color Selected = Blue");
            //activeColorInGoo = gooColor.color.b;
            ActiveColor[1] = false;
            ActiveColor[0] = false;
            ActiveColor[2] = true;
        }

        if (transform.eulerAngles.z <= 60.0f || transform.eulerAngles.z >= 300 )
        {
            meshRenderer.material.SetColor("_Color", Color.green);
            //Debug.Log("Color Selected = Green");
            //activeColorInGoo = gooColor.color.g;
            ActiveColor[1] = true;
            ActiveColor[0] = false;
            ActiveColor[2] = false;
        }

        if (transform.eulerAngles.z <= 180.0f && transform.eulerAngles.z >= 60.0f)
        {
            meshRenderer.material.SetColor("_Color", Color.red);
            //Debug.Log("Color Selected = Red");
           // activeColorInGoo = gooColor.color.r;
            ActiveColor[1] = false;
            ActiveColor[0] = true;
            ActiveColor[2] = false;
        }
        
    }
}
