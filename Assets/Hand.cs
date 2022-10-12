using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public enum HandType
{
    Left,
    Right
};

public class Hand : MonoBehaviour
{

    public HandType type = HandType.Left;
    public bool isHidden { get; private set; } = false;

    public InputAction trackedAction = null;

    public InputAction gripAction = null;
    public InputAction pullTriggerAction = null;

    public Animator handAnimator = null;

    int m_gripPercentParam = 0;
    int m_pullTriggerPercentParam = 0;

    List<Renderer> m_currentRenderers = new List<Renderer>();

    //currently tracked state
    bool m_isCurrentlyTracked = false;


    //Array of current colliders
    Collider[] m_colliders = null;

    public bool isCollisionEnabled { get; private set; } = false;

    public XRBaseInteractor interactor = null;

    private void Awake()
    {
        if(interactor == null)
        {
            interactor = GetComponentInParent<XRBaseInteractor>();
        }
    }

    private void OnEnable()
    {
        interactor.onSelectEntered.AddListener(OnGrab);
        interactor.onSelectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        interactor.onSelectEntered.RemoveListener(OnGrab);
        interactor.onSelectExited.RemoveListener(OnRelease);
    }

    void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        trackedAction.Enable();
        m_gripPercentParam = Animator.StringToHash("GripPercent");
        m_pullTriggerPercentParam = Animator.StringToHash("PointPercent");
        gripAction.Enable();
        pullTriggerAction.Enable();
        Hide();
    }

    void UpdateAnimation()
    {
        float pointPercent = pullTriggerAction.ReadValue<float>();
        float gripPercent = gripAction.ReadValue<float>();
        handAnimator.SetFloat(m_gripPercentParam, Mathf.Clamp01(gripPercent + pointPercent));
        handAnimator.SetFloat(m_pullTriggerPercentParam, pointPercent);
    }

    // Update is called once per frame
    void Update()
    {
        float isTracked = trackedAction.ReadValue<float>();
        if(isTracked == 1.0f && !m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = true;
            Show();
        }
        else if(isTracked == 0 && m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = false;
            Hide();
        }

        UpdateAnimation();
    }

    public void Show()
    {
        //Debug.Log("Show function executed");

        //loop through all Mesh Renderer components in the global Current Renderers list
        foreach (Renderer renderer in m_currentRenderers)
        {
            //enable that renderer
            renderer.enabled = true;
        }
        
        //Turn off the isHidden bool global variable
        isHidden = false;
        EnableCollisions(false);
    }

    public void Hide()
    {
        //Debug.Log("Hide function executed");

        //clear the list of current Renderers
        m_currentRenderers.Clear();

        //create a local variable array of all Mesh Renderers in child objects
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        //loop through all Mesh Renderer components in the renderers array
        foreach(Renderer renderer in renderers)
        {

            //disable that renderer
            renderer.enabled = false;

            //add that renderer to the global variable list containing all MeshRenderers
            m_currentRenderers.Add(renderer);
        }
        
        //enable the isHidden bool global variable
        isHidden = true;
        EnableCollisions(false);
    }

    public void EnableCollisions(bool enabled)
    {
        if (isCollisionEnabled == enabled) return;

        isCollisionEnabled = enabled;
        foreach(Collider collider in m_colliders)
        {
            collider.enabled = isCollisionEnabled;
        }
    }

    void OnGrab(XRBaseInteractable grabbedObject)
    {
        //Debug.Log("We have grabbed an object named " + grabbedObject.name);

        HandControl ctrl = grabbedObject.GetComponent<HandControl>();
        if(ctrl != null)
        {
            //Debug.Log("Grabbed object has HAND CONTROL script");

            if (!ctrl.hideHand)
            {
                //Debug.Log("We grabbed and should be hiding hands");
                ctrl.hideHand = true;
                Hide();
            }
        }
    }

    void OnRelease(XRBaseInteractable releasedObject)
    {
        HandControl ctrl = releasedObject.GetComponent<HandControl>();
        if(ctrl != null)
        {
            if (ctrl.hideHand)
            {
                ctrl.hideHand = false;
                Show();
            }
        }
    }
}
