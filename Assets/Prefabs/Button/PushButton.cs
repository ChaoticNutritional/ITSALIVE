using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    //an event to handle what happens when a button is pressed
    public UnityEvent onPressed = new UnityEvent();
    public UnityEvent onReset = new UnityEvent();
    public UnityEvent onPressStart = new UnityEvent();
    public UnityEvent onPressEnd = new UnityEvent();


    [Min(0.01f)] //the space between the max height and minimum height of the button's local y position
    public float pressDepth = 0.015f;
    [Min(0.0001f)] //the amount at which the button must be above the minimum to be considered pressed
    public float consideredPressedDepth = 0.001f;
    [Min(0.0001f)]
    public float resetThreshold = 0.001f;

    [SerializeField]
    [Min(0.001f)]
    public float returnSpeed = 1.0f;


    
    float m_currentPressDepth = 0.0f; //ivariable to get the current depth that the button has been pressed in onTriggerEnter and Update
    float m_yMax = 0.0f; //Starting or resting position
    float m_yMin = 0.0f; //Pressed or activated position
    bool m_wasPressed = false; //bool storing whether or not the button has been pressed already
    
    //list of colliders to track any and all finger digit colliders coming in contact with the button
    List<Collider> m_currentColliders = new List<Collider>();

    //initializes our interactor which will eventually be fur hands
    XRBaseInteractor m_interactor = null;

    // Start is called before the first frame update
    void Start()
    {
        //set max height to starting position of button y when scene loads
        m_yMax = transform.localPosition.y;
    }

    void SetMinRange()
    {
        //set min height to the max height - pressDepth variable
        m_yMin = m_yMax - pressDepth;
    }

    bool isPressed()
    {
        return transform.localPosition.y >= m_yMin && transform.localPosition.y <= m_yMin + consideredPressedDepth;
    }

    bool isReset()
    {
        return transform.localPosition.y >= m_yMax - resetThreshold && transform.localPosition.y <= m_yMax;
    }



    float GetPressDepth(Vector3 interactorWorldPosition)
    {
        //returns height of interactor in terms of this object's local space
        return transform.parent.InverseTransformPoint(interactorWorldPosition).y;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        if(interactor != null && !other.isTrigger)
        {
            m_currentColliders.Add(other);
            if(m_interactor == null)
            {
                m_interactor = interactor;
                //Placed here as a backup in case for whatever reason pressDepth var changed since last press
                SetMinRange();
                m_currentPressDepth = GetPressDepth(m_interactor.transform.position);
                onPressStart?.Invoke();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(m_currentColliders.Contains(other))
        {
            m_currentColliders.Remove(other);

            if(m_currentColliders.Count == 0)
            {
                onPressEnd?.Invoke();
                EndPress();
            }
        }
    }

    void EndPress()
    {
        m_currentColliders.Clear();
        m_currentPressDepth = 0.0f;
        m_interactor = null;
    }

    void SetHeight(float newHeight)
    {
        Vector3 currentPosition = transform.localPosition;
        currentPosition.y = newHeight;
        currentPosition.y = Mathf.Clamp(currentPosition.y, m_yMin, m_yMax);
        transform.localPosition = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_interactor != null)
        {
            //we are currently doing a button press
            float newPressHeight = GetPressDepth(m_interactor.transform.position);
            float deltaHeight = m_currentPressDepth - newPressHeight;
            float newPressedPosition = transform.localPosition.y - deltaHeight;

            SetHeight(newPressedPosition);

            if(!m_wasPressed && isPressed()) //we are being pressed now and haven't already been pressed
            {
                //button is pressed
                Debug.Log("Button is Pressed");

                onPressed?.Invoke(); //if there is something in the onPressed event, invoke it
                m_wasPressed = true; //the button is now considered pressed
            }

            m_currentPressDepth = newPressHeight;
        }
        else
        { 
            if(!Mathf.Approximately(transform.localPosition.y, m_yMax))
            {
                float returnHeight = Mathf.MoveTowards(transform.localPosition.y, m_yMax, Time.deltaTime * returnSpeed);
                SetHeight(returnHeight);
            }
        }

        if(m_wasPressed && isReset())
        {
            onReset?.Invoke();
            m_wasPressed = false;
        }
    }
}
