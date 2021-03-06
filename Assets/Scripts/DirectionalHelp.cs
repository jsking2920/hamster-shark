﻿using System.Collections;
using System;
using UnityEngine;

/*
 * Author: cwilliams@filamentgames.com
 *
 * Purpose:
 * 
 *
 */
public class DirectionalHelp : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip[] keepGoing;

    [SerializeField]
    AudioClip[] turnAround;

    [SerializeField]
    AudioClip[] turnRight;
    [SerializeField]
    AudioClip[] slightRight;

    [SerializeField]
    AudioClip[] turnLeft;
    [SerializeField]
    AudioClip[] slightLeft;

    [SerializeField]
    AudioClip[] overHere;

    [SerializeField]
    AudioClip youHaventMoved;

    public bool Paused = false;
    public bool IsPlaying() => source.isPlaying;

    public void StopNow() 
    {
        source.Stop();
    }

    void Awake()
    {
        StartCoroutine(UpdateRoutine());
    }

    public void DoNothingCallout()
    {
        StartCoroutine(DoingNothingCallout());
    }

    IEnumerator DoingNothingCallout()
    {
        var pos = player.transform.position;
        while(true)
        {
            yield return new WaitUntil(() => !Paused);
            var stayingStill = true;
            for(int i = 0; i < 15;i++)
            {
                stayingStill &= player.transform.position == pos;
                yield return new WaitForSeconds(1);
            }
            if(stayingStill)
            {
                source.PlayOneShot(youHaventMoved);
                yield return new WaitForSeconds(60);
            }
        }
    }

    public void Stop()
    {
        Paused = true;
    }

    IEnumerator UpdateRoutine()
    {
        while(true)
        {
            yield return new WaitUntil(() => !Paused);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                var angle = Vector3.Angle(player.transform.forward,DirectionalZone.IntendedDirection);
                print($"player {player.transform.forward}, intended {DirectionalZone.IntendedDirection}, angle {angle}");

                if( angle < 45f/2f)
                {
                    var clip = keepGoing[(int)(UnityEngine.Random.value * keepGoing.Length)];
                    source.PlayOneShot(clip);
                }
                else if(angle > 135f)
                {
                    var clip = turnAround[(int)(UnityEngine.Random.value * turnAround.Length)];
                    source.PlayOneShot(clip);
                } else 
                {
                    var rightness = Vector3.Angle(-player.transform.right,DirectionalZone.IntendedDirection);
                    var leftness  = Vector3.Angle( player.transform.right,DirectionalZone.IntendedDirection);
                    
                    if(leftness <= rightness)
                    {
                        if(angle < 45)
                        {
                            var clip = slightRight[(int)(UnityEngine.Random.value * slightRight.Length)];
                            source.PlayOneShot(clip);
                        } else {
                            var clip = turnRight[(int)(UnityEngine.Random.value * turnRight.Length)];
                            source.PlayOneShot(clip);
                        }

                    } else 
                    {
                        if(angle < 45)
                        {
                            var clip = slightLeft[(int)(UnityEngine.Random.value * slightLeft.Length)];
                            source.PlayOneShot(clip);
                        } else {
                            var clip = turnLeft[(int)(UnityEngine.Random.value * turnLeft.Length)];
                            source.PlayOneShot(clip);
                        }
                    }
                }
                yield return new WaitForSeconds(3);
            }
        }
    }
}
