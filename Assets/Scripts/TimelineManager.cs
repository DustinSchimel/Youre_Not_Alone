using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    public Animator playerAnimator;
    public AnimationClip idle;
    public RuntimeAnimatorController playerAnim;
    public PlayableDirector director;
    private DialogueMover dialogueMover;

    void OnEnable()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
        dialogueMover = FindObjectOfType<DialogueMover>();
        //playerAnimator.runtimeAnimatorController = null;
    }

    void OnDisable()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
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
