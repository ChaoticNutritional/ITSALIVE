using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    Vector3 m_startRotation;

    MeshRenderer meshRenderer = null;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StartTurn()
    {
        m_startRotation = transform.localEulerAngles;
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void StopTurn()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void DialUpdate(float angle)
    {
        Vector3 angles = m_startRotation;
        angles.z += angle;
        transform.localEulerAngles = angles;
    }
}
