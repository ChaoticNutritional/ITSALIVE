using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInDarkness : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator fadeAnimator;

    void Start()
    {
        StartCoroutine(startInDarkness());
    }

    private IEnumerator startInDarkness()
    {
        yield return new WaitForSeconds(3.0f);
        fadeAnimator.SetTrigger("Fade2In");
    }
}
