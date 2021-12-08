using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMovement pm;
    private PlayerLockOn plo;

    [Space]
	[Header("Animation")]
    public Animator animator;

    [Space]
	[Header("Animation values")]
    private int isWalkingHash;
    private int isRunningHash;
    private int isFocusedHash;
    private int velocityZHash;
    private int velocityXHash;
    private bool isRunning;
    public Vector2 velocity = new Vector2(0f,0f);


    // Start is called before the first frame update
    void Start() {
        //get all the scripts
        pm = this.GetComponent<PlayerMovement>();
        plo = this.GetComponent<PlayerLockOn>();

        //set animator hash values
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isFocusedHash = Animator.StringToHash("isFocused");
        velocityZHash = Animator.StringToHash("VelocityZ");
        velocityXHash = Animator.StringToHash("VelocityX");
        
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

    public void AnimateFocus(){
        animator.SetBool(isFocusedHash,plo.GetHasLockedOn());
    }

    public void FocusRun(Vector2 direction){
        animator.SetFloat(velocityZHash,direction.x);
        animator.SetFloat(velocityXHash,direction.y);
    }

}
