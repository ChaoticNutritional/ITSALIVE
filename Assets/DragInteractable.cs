using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class DragEvent : UnityEvent<float> { }


public class DragInteractable : XRBaseInteractable
{
    
    public Transform startDragPos = null;
    public Transform endDragPos = null;

    [HideInInspector]
    public float dragPercent = 0.0f; //percentage between 0 and 1

    protected XRBaseInteractor m_interactor = null;

    public UnityEvent onDragStart = new UnityEvent();
    public UnityEvent onDragEnd = new UnityEvent();
    public DragEvent onDragUpdate = new DragEvent();

    Coroutine m_drag = null;

    void StartDrag()
    {
        if(m_drag != null)
        {
            StopCoroutine(m_drag);
        }
        m_drag = StartCoroutine(CalculateDrag());
        onDragStart?.Invoke();
    }

    void EndDrag()
    {
        if(m_drag != null)
        {
            StopCoroutine(m_drag);
            m_drag = null;
            onDragEnd?.Invoke();
        }
    }

    //Inverted Lerp Function
    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 currentValue)
    {
        //a new vector whose length is a distance from a to b
        Vector3 AB = b - a;

        //a new vector whose length is a distance we have traveled from the start to currentValue
        Vector3 AV = currentValue - a;
        //the dot of A

        //returns the percentage of VALUE between positions a and b
        return Mathf.Clamp01(Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB));
    }

    IEnumerator CalculateDrag()
    {
        while(m_interactor != null)
        {
            // get a line in local space
            Vector3 line = startDragPos.localPosition - endDragPos.localPosition;
            Debug.DrawLine(line, startDragPos.localPosition);

            // convert our itneractor position to local space
            Vector3 interactorLocalPosition = startDragPos.parent.InverseTransformPoint(m_interactor.transform.position);

            // project the interactor position onto the line
            Vector3 projectedPoint = Vector3.Project(interactorLocalPosition, line.normalized);

            // reverse interpolate that position on the line to get a percentage equal to how far the drag has moved
            dragPercent = InverseLerp(startDragPos.localPosition, endDragPos.localPosition, projectedPoint);

            onDragUpdate?.Invoke(dragPercent);

            yield return null;
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        m_interactor = interactor;
        StartDrag();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        EndDrag();
        m_interactor = null;
        base.OnSelectExited(interactor);
    }

}
