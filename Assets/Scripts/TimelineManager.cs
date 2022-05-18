/*
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

    public GameObject player;
    public Animator playerAnimator;
    public RuntimeAnimatorController playerAnim;

    public GameObject bone6;
    public GameObject bone7;
    public GameObject bone8;
    public GameObject bone9;

    // Bone 6
    private float bone6_default_rotation_z = 173.354f;

    // Bone 7
    private float bone7_default_rotation_z = 177.756f;

    // Bone 8
    private float bone8_default_rotation_z = 5f;

    // Bone 9
    private float bone9_default_rotation_z = -18.269f;

    /*
    void OnEnable()
    {
        dialogueMover = FindObjectOfType<DialogueMover>();
        playerAnim = playerAnimator.runtimeAnimatorController;
        //playerAnimator.runtimeAnimatorController = null;
    }

    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnimator.runtimeAnimatorController = playerAnim;
        }
    }
    */

/*
    void OnEnable()
    {
        playerAnimator = player.GetComponent<Animator>();
        playerAnim = playerAnimator.runtimeAnimatorController;

        dialogueMover = FindObjectOfType<DialogueMover>();
        //playerAnimator.runtimeAnimatorController = null;
    }

    void OnDisable()
    {
        //player.GetComponent<Animator>() = playerAnimator;
        playerAnimator.runtimeAnimatorController = playerAnim;
        UnlockPlayerTextboxY();

        //playerAnimator.SetBool("Animation_Sit_Idle", false);
        bone6.transform.eulerAngles = new Vector3(0f, 0f, bone6_default_rotation_z);
        bone7.transform.eulerAngles = new Vector3(0f, 0f, bone7_default_rotation_z);
        bone8.transform.eulerAngles = new Vector3(0f, 0f, bone8_default_rotation_z);
        bone9.transform.eulerAngles = new Vector3(0f, 0f, bone9_default_rotation_z);
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
}
*/


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

    public GameObject bone1;
    public GameObject bone2;
    public GameObject bone3;
    public GameObject bone4;
    public GameObject bone5;

    public GameObject bone6;
    public GameObject bone7;
    public GameObject bone8;
    public GameObject bone9;

    private float bone1_default_rotation_z = 92.705f;
    private float bone2_default_rotation_z = -1.091f;
    private float bone3_default_rotation_z = -2.3f;
    private float bone4_default_rotation_z = -137.23f;
    private float bone5_default_rotation_z = 152.277f;

    private float bone6_default_rotation_z = 173.354f;
    private float bone7_default_rotation_z = 177.756f;
    private float bone8_default_rotation_z = 5f;
    private float bone9_default_rotation_z = -18.269f;

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
        FixPlayerBones();
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

    void FixPlayerBones()
    {
        /*
        bone1.transform.rotation = Quaternion.identity;
        bone2.transform.rotation = Quaternion.identity;
        bone3.transform.rotation = Quaternion.identity;
        bone4.transform.rotation = Quaternion.identity;
        bone5.transform.rotation = Quaternion.identity;
        */

        bone6.transform.rotation = Quaternion.identity;
        bone7.transform.rotation = Quaternion.identity;
        bone8.transform.rotation = Quaternion.identity;
        bone9.transform.rotation = Quaternion.identity;


        //default rotation is 267.295
        bone6.transform.eulerAngles = new Vector3(0f, 0f, 92.705f + bone6_default_rotation_z);
        bone7.transform.eulerAngles = new Vector3(0f, 0f, 92.705f + bone7_default_rotation_z);
        //bone8.transform.eulerAngles = new Vector3(0f, 0f, bone8_default_rotation_z);
        //bone9.transform.eulerAngles = new Vector3(0f, 0f, bone9_default_rotation_z);
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