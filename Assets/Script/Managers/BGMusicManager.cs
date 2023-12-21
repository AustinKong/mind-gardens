using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicManager : MonoBehaviour
{
    public static BGMusicManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource track1, track2;

    private void Start()
    {
        StartCoroutine(bgMusicPlayer(1));
    }

    int currTrack = 1;

    public IEnumerator bgMusicPlayer(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        Debug.LogFormat("AZ");
        if( currTrack == 1)
        {
            currTrack = 2;
            track2.Play();
            StartCoroutine(bgMusicPlayer(track2.clip.length + Random.Range(15, 45)));
        }
        else
        {
            currTrack = 1;
            track1.Play();
            StartCoroutine(bgMusicPlayer(track1.clip.length + Random.Range(15, 45)));
        }
    }

}
