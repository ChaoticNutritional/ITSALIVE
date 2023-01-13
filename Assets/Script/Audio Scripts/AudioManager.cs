using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip start = null;
    public AudioClip idle = null;
    public AudioClip stop = null;
    public AudioSource source;



    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetNonLoopSound(AudioClip aClip)
    {
        source.loop = false;
        source.Stop();
        source.clip = aClip;
        source.PlayOneShot(aClip, 0.2f);

    }

    //backup in case coroutine doesn't work
    public void SetLoopingSound(AudioClip aClip)
    {
        source.loop = true;
        source.clip = aClip;
        source.volume = 0.2f;
        source.Play();
    }

    public void SoundFollow(AudioClip[] aClip)
    {
        StartCoroutine(FollowSound(aClip[0], aClip[1]));
    }

    IEnumerator FollowSound(AudioClip aClip, AudioClip secondClip)
    {
        float clipTime = aClip.length;
        source.clip = aClip;
        source.PlayOneShot(aClip, 0.2f);
        yield return new WaitForSeconds(clipTime);
        source.clip = secondClip;
        source.PlayOneShot(secondClip, 0.2f);
    }
}
