using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{

    public Animator animator;

    public void FadeToLevel(int levelIndex)
    {
        animator.SetTrigger("Fade2Black");
    }

    public void OnFadeFinish()
    {
        SceneManager.LoadScene(0);
    }
}
