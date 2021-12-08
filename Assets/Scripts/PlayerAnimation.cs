using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMovement pm;

    [Space]
	[Header("Animation")]
    public Animator animator;
    private int isWalkingHash;
    private int isRunningHash;
    private bool isRunning;

    // Start is called before the first frame update
    void Start() {
        //get all the scripts
        pm = this.GetComponent<PlayerMovement>();

        //set animator hash values
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        
    }

    // Update is called once per frame
    void Update() {

        
    }

    public void AnimateRun(){
        isRunning = pm.GetIsRunning();
        animator.SetBool(isRunningHash, isRunning);
    }

    public void IsWalking() {
        animator.SetBool(isWalkingHash, true);
    }
    public void IsNotWalking() {
        animator.SetBool(isWalkingHash, false);
    }
}
