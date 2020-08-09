using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{

    private static AudioSource sfx;
    public AudioClip[] paddleHits;
    public AudioClip[] brickBreaks;
    public AudioClip brickHit;
    public AudioClip brickMetal;
    public AudioClip[] countDown;
    public AudioClip[] lazerFire;
    public AudioClip[] pause;
    public AudioClip buttonClick;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPaddleHit()
    {
        sfx.PlayOneShot(paddleHits[Random.Range(0,paddleHits.Length)], .5f);
    }

    public void PlayBrickBreak()
    {
        sfx.PlayOneShot(brickBreaks[Random.Range(0, brickBreaks.Length)], .35f);
    }

    public void PlayBrickHit()
    {
        sfx.PlayOneShot(brickHit, .5f);
    }

    public void PlayBrickMetal()
    {
        sfx.PlayOneShot(brickMetal, .5f);
    }

    public void PlayCountDown(int x)
    {
        sfx.PlayOneShot(countDown[x], .5f);
    }

    public void PlayLazerFire()
    {
        sfx.PlayOneShot(lazerFire[Random.Range(0, lazerFire.Length)], .5f);
    }

    public void PlayPause(int x)
    {
        sfx.PlayOneShot(pause[x], .5f);
    }

    public void PlayButtonClick()
    {
        sfx.PlayOneShot(buttonClick, .5f);
    }

}
