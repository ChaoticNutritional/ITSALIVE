using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

[Serializable]
public class TurnEvent : UnityEvent<float> { }


public class TurnInteractable : XRBaseInteractable
{

    XRBaseInteractor m_interactor = null;

    Coroutine m_turn = null;

    Vector3 m_startingRotation = Vector3.zero;

    public UnityEvent onTurnStart = new UnityEvent();
    public UnityEvent onTurnStop = new UnityEvent();
    public TurnEvent onTurnUpdate = new TurnEvent();

    public float turnAngle = 0.0f;
    
    Quaternion GetLocalRotation(Quaternion targetWorldRotation)
    {
        //intvert the world rotation and multiply it by the Object's world rotation.
        // gives us the rotation of the target with respect to the object's transform.
        return Quaternion.Inverse(targetWorldRotation) * transform.rotation;
    }

    void StartTurn()
    {
        if(m_turn != null)
        {
            StopCoroutine(m_turn);
        }
        
        //Gets the local rotation in respect to the interactor's rotation
        Quaternion localRotation = GetLocalRotation(m_interactor.transform.rotation);
        m_startingRotation = localRotation.eulerAngles;
        onTurnStart?.Invoke();
        m_turn = StartCoroutine(UpdateTurn());
    }

    void EndTurn()
    {
        if (m_turn != null)
        {
            StopCoroutine(m_turn);
            onTurnStop?.Invoke();
            m_turn = null;
        }
    }


    IEnumerator UpdateTurn()
    {
        while (m_interactor != null)
        {
            Quaternion localRotation = GetLocalRotation(m_interactor.transform.rotation);
            turnAngle = m_startingRotation.z - localRotation.eulerAngles.z;
            onTurnUpdate?.Invoke(turnAngle);
            yield return null;
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        m_interactor = interactor;
        StartTurn();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        EndTurn();
        m_interactor = null;
        base.OnSelectExited(interactor);
    }
}
