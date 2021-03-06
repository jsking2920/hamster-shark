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
public class TutorialScript : MonoBehaviour
{
    [SerializeField]
    DirectionalHelp helpVoice;
    [SerializeField]
    Player.MouseControls mouseControls;

    [SerializeField]
    Player.Echolocation echolocation;
    [SerializeField]
    Player.WallSounds wallSounds;


    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip pressAnyKey;

    [SerializeField]
    AudioClip click;
    [SerializeField]
    AudioClip whoAreYou;

    [SerializeField]
    AudioClip youMustBeLost;

    [SerializeField]
    AudioClip pressSpace;

    [SerializeField]
    AudioClip youPressedSpace;

    [SerializeField]
    AudioClip proceedByPressMouse;
    [SerializeField]
    AudioClip findTheWall;

    [SerializeField]
    AudioClip reachWallClip;
    [SerializeField]
    AudioClip reachWallClip2;
    [SerializeField]
    AudioClip reachWallClip3;

    [SerializeField]
    AudioClip youCanTurn;

    [SerializeField]
    AudioClip turnRight;

    [SerializeField]
    AudioClip thisIsACheckpoint;
    [SerializeField]
    private AudioClip pleaseLeave;
    [SerializeField]
    AudioClip firePing;
    [SerializeField]
    AudioClip didPing;
    [SerializeField]
    private AudioClip youGotIt;


    [SerializeField]
    BoxCollider prewall1;
    [SerializeField]
    BoxCollider prewall2;
    [SerializeField]
    BoxCollider prewall3;
    [SerializeField]
    BoxCollider prewall4;
    
    bool hasReachedFirstCheckPoint = false;
    bool hasReachedSecondCheckPoint = false;

    bool hasReachedThirdCheckPoint = false;
    bool hasReachedFourthCheckPoint = false;

    bool hasReachedWall = false;


    void Awake()
    {
        //Disable all controls & reative sounds
        helpVoice.Paused = true;
        mouseControls.state = Player.MouseControls.State.Offline;
        echolocation.IsPaused = true;
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    { //TODO add repeats for clarity
        yield return new WaitForSeconds(2);

        source.PlayOneShot(pressAnyKey);
        yield return new WaitUntil(() => Input.anyKey);
        yield return new WaitUntil(() => !source.isPlaying);

        source.PlayOneShot(click);

        yield return new WaitForSeconds(3);

        source.PlayOneShot(whoAreYou);
        yield return new WaitUntil(() => !source.isPlaying);

        yield return new WaitForSeconds(1);

        source.PlayOneShot(youMustBeLost);
        yield return new WaitUntil(() => !source.isPlaying);


        yield return new WaitForSeconds(1);
        source.PlayOneShot(pressSpace);

        var time = Time.time;
        var pressedSpace = true;
        while(source.isPlaying || !Input.GetKey(KeyCode.Space))
        {
            yield return null;
            if(Time.time > time + 10)
            {
                pressedSpace = false;
                break;
            }
            if(source.isPlaying && Input.GetKey(KeyCode.Space))
            {
                break;
            }
        }

        yield return new WaitUntil(() => !source.isPlaying);
    
        yield return new WaitForSeconds(.25f);

        if(pressedSpace)
            source.PlayOneShot(youPressedSpace, 0.75f);
        
        yield return new WaitUntil(() => !source.isPlaying);


        yield return new WaitForSeconds(1);

        yield return Play(proceedByPressMouse);

        mouseControls.state = Player.MouseControls.State.OnlyMovement;

        time = Time.time;
        while(! Input.GetKey(KeyCode.Mouse0))
        {
            yield return null;
            if(Time.time - time > 5)
            {        
                helpVoice.Paused = true;
                yield return new WaitUntil(() => !helpVoice.IsPlaying() ||  Input.GetKey(KeyCode.Mouse0));
                source.PlayOneShot(proceedByPressMouse);
                yield return new WaitUntil(() => !source.isPlaying ||  Input.GetKey(KeyCode.Mouse0));
                helpVoice.Paused = false; 
                time = Time.time;
            }
        }

        yield return new WaitUntil(() => !source.isPlaying);        
        
        wallSounds.Activate();

        yield return Play(findTheWall);

        yield return new WaitUntil(() => hasReachedWall);
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => !source.isPlaying); 
        
        yield return Play(reachWallClip);
        yield return Play(reachWallClip2);

        //yield return Play(reachWallClip3); move this somewhere else

        yield return new WaitForSeconds(1f);

        yield return Play(youCanTurn);
        mouseControls.state = Player.MouseControls.State.Online;
        yield return new WaitUntil(() => Vector3.Distance(mouseControls.transform.forward,Vector3.forward) > .25f);

        yield return Play(turnRight);

        prewall1.enabled = false;

        yield return Play(thisIsACheckpoint);
        yield return new WaitUntil(() => hasReachedFirstCheckPoint);
        


        prewall2.enabled = false;
        yield return new WaitUntil(() => hasReachedSecondCheckPoint);

        helpVoice.DoNothingCallout();

        yield return Play(pleaseLeave);
        prewall3.enabled = false;

        yield return new WaitUntil(() => hasReachedThirdCheckPoint);
        yield return new WaitForSeconds(1);

        echolocation.IsPaused = false;
        yield return Play(firePing);

        time = Time.time;
        while(Input.GetKey(KeyCode.Mouse1))
        {
            yield return null;
            if(Time.time - time > 5)
            {
                helpVoice.Paused = true;
                yield return new WaitUntil(() => !helpVoice.IsPlaying() ||  Input.GetKey(KeyCode.Mouse0));
                source.PlayOneShot(firePing);
                yield return new WaitUntil(() => !source.isPlaying ||  Input.GetKey(KeyCode.Mouse0));
                helpVoice.Paused = false; 
                time = Time.time;
            }
        }

        yield return new WaitForSeconds(1);

        yield return Play(didPing);


        prewall4.enabled = false;
        yield return new WaitUntil(() => hasReachedFourthCheckPoint);

        yield return Play(youGotIt);

        //Done.
    }

    public void ReachWall()
    {
        hasReachedWall = true;
    }

    public void ReachFirstCheckPoint()
    {
        hasReachedFirstCheckPoint = true;
    }    
    public void ReachSecondCheckPoint()
    {
        hasReachedSecondCheckPoint = true;
    }    
    public void ReachThirdCheckPoint()
    {
        hasReachedThirdCheckPoint = true;
    }
    public void ReachFourthCheckPoint()
    {
        hasReachedFourthCheckPoint = true;
    }

    [ContextMenu("AutoComplete")]
    public void Finish()
    {
        StopAllCoroutines();

        prewall1.enabled = false;
        prewall2.enabled = false;
        prewall3.enabled = false;
        prewall4.enabled = false;
        helpVoice.Paused = false; 
        echolocation.IsPaused = false;
        mouseControls.state = Player.MouseControls.State.Online;
    }

    IEnumerator Play(AudioClip clip)
    {
        helpVoice.Paused = true;
        yield return new WaitUntil(() => !helpVoice.IsPlaying());
        source.PlayOneShot(clip);
        yield return new WaitUntil(() => !source.isPlaying);
        helpVoice.Paused = false; //this is so gross I can't even :(
    }
}