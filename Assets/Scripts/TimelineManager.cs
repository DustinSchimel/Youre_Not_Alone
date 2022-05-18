using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    public PlayableDirector director;
    private DialogueMover dialogueMover;

    //public GameObject player;

    public Animator playerAnimator;
    public RuntimeAnimatorController playerAnim;

    void OnEnable()
    {
        dialogueMover = FindObjectOfType<DialogueMover>();
        playerAnim = playerAnimator.runtimeAnimatorController;
        playerAnimator.runtimeAnimatorController = null;
    }

    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnimator.runtimeAnimatorController = playerAnim;
        }
    }




    /*
    void OnEnable()
    {
        playerAnimator = player.GetComponent<Animator>();
        //playerAnimController = playerAnimator.runtimeAnimatorController;

        dialogueMover = FindObjectOfType<DialogueMover>();
        //playerAnimator.runtimeAnimatorController = null;
    }

    void OnDisable()
    {
        player.GetComponent<Animator>() = playerAnimator;
        //playerAnimator.runtimeAnimatorController = playerAnim;
        UnlockPlayerTextboxY();

        //playerAnimator.SetBool("Animation_Sit_Idle", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnimator.runtimeAnimatorController = playerAnim;
        }
    }
    */

    public void LockPlayerTextboxY()
    {
        dialogueMover.lockPlayerTextboxY();
    }

    public void UnlockPlayerTextboxY()
    {
        dialogueMover.unlockPlayerTextboxY();
    }

    public void ParentCutscenePlaying()
    {
        dialogueMover.startedParentCutscene();
    }

    public void ParentCutscenePlaying2()
    {
        dialogueMover.startedParentCutscene2();
    }
}
