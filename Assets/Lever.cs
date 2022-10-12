using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform startOrientation = null;
    public Transform endOrientation = null;
    public Animator corpseAnim = null;
    public AnimationClip animationClip = null;


    MeshRenderer meshRenderer = null;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void OnLeverPullStart()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
        //Debug.Log("started pulling lever");
        corpseAnim.SetBool("IsIdle", false);

    }

    public void OnLeverPullStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
        //corpseAnim.StopPlayback();
        corpseAnim.speed = 0.0f;
        //Debug.Log("We have stopped pulling the lever");
    }

    public void UpdateLever(float percent)
    {
        transform.rotation = Quaternion.Slerp(startOrientation.rotation, endOrientation.rotation, percent);
        //Debug.Log(corpseAnim.GetCurrentAnimatorStateInfo(0));
        //Debug.Log("we should be playing the animation at frame " + percent);

        Debug.Log("The Percent is: " + percent);
        corpseAnim.Play("Base Layer.Raise and Turn", 0, percent * animationClip.length/2);
    }
}
