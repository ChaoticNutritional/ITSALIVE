using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class Dial : MonoBehaviour
{
    Vector3 m_startRotation;

    MeshRenderer meshRenderer = null;

    //PROTOTYPE
    //public GameObject rayHolder = null;
    //public Material greenMaterial = null;



    //segment rotation into 3 parts each corresponding to RGB values;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StartTurn()
    {


        m_startRotation = transform.localEulerAngles;
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


        //PROTOTYPE 
        /*RaycastHit hit;
        Ray ray = new Ray(rayHolder.transform.position, rayHolder.transform.up);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                hit.collider.gameObject.GetComponent<Material>().SetColor("_Color", Color.green);
            }
        }*/
    }
}
