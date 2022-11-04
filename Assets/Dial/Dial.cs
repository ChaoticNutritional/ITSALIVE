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
    }
}
