using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{

    private static AudioSource sfx;
    public AudioClip[] paddleHits;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        PlayPaddleHit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPaddleHit()
    {

        sfx.PlayOneShot(paddleHits[Random.Range(0,paddleHits.Length)], .5f);
    }
}
