using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour{

    private PlayerLockOn plo;
    private float initStepOffset;
    private float currStepOffset;


	[Space]
	[Header("Movement")]	
    public CharacterController controller;
    public Slider slider;
    public Image fill;
    private bool staminaBar = false;
    private bool staminaBarExhausted = false;
    private float walkSpeed = 6f;
    private float runSpeed = 10f;
    private float exhaustSpeed = 4f;
    private bool isRunning = false;
    public bool isExhausted = false;
    private float exhaustTimeInit = 6f;
    public float exhaustTime;
    private float runTimerInit = 5f;
    public float runTimer;
    private float exhaustRecoverySpeed = 2f;
    private float recoverySpeed = 4f;
    private float staminaDecreaseSpeed = 3f;
    public float speed;
    public float turnSmoothTime = 0.1f;
    public float turnFocusTime = 0.05f;
    private float turnFocusTimeInit;
    private float turnSmoothVelocity;
    [Space]
    [Header("Focusing")]
    public bool isFocusing = false;
    public bool canFocus = false;
    public Transform focusTarget;

    private Vector2 moveDir;

	[Space]
	[Header("Jumping")]
    private bool isJumpingPressed = false;
    private bool canQueueJump = false;
    public Transform groundCheck;
    public float groundCheckRange = 0.4f;
    public LayerMask groundLayers;
    public bool isJumping = false;
    public float notFallingMultiplier = 0.8f;

    [Space]
    [Header("Gravity constants")]
    public float gravity = 40f;
    public float initGravity;
    public float currGravity;
    public float constantGravity = -0.6f;
    public float maxGravity = -80f;

    private Vector3 gravityDirection;
    private Vector3 gravityMovement;

    void Awake() { 
        //set up gravity
        gravityDirection = Vector3.down;
        initGravity = gravity;
        //set initial step offset
        initStepOffset = controller.stepOffset;
        //set initial speed
        speed = walkSpeed;
        slider.maxValue = runTimerInit;
        //set initial tutn time
        turnFocusTimeInit = turnFocusTime;
        //get scripts
        plo = gameObject.GetComponent<PlayerLockOn>();


    }

    //set move direction for Move() function
    public void OnMove(Vector2 dir){

        moveDir = dir;

    }

    private void Move(Vector2 direction){

        float horizontal = direction.x;
        float vertical = direction.y;

        Vector3 moveDir = new Vector3(0f,0f,0f);
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        //see if you are focusing
        if(canFocus){
            
            float focusAngle = Mathf.Atan2(focusTarget.position.x - transform.position.x, focusTarget.position.z - transform.position.z) * Mathf.Rad2Deg;
            float angleFocus = Mathf.SmoothDampAngle(transform.eulerAngles.y, focusAngle, ref turnSmoothVelocity, turnFocusTime);
            transform.rotation = Quaternion.Euler(0f, angleFocus, 0f);
            if(turnFocusTime > 0){
                turnFocusTime -= 0.02f * Time.deltaTime;
            }
            

        }
        

        if(dir.magnitude >= 0.1f){


            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if(!isFocusing){
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
        }

        controller.Move(moveDir.normalized*speed*Time.deltaTime + gravityMovement);

    }



    private bool isGrounded(){

        return controller.isGrounded;
    }

    //calcuate gravity
    private void CalculateGravity(){

        bool notFalling = !(currGravity <= 0.0f) && isJumpingPressed;
        

        //check if it is grounded
        if(isGrounded() && !isJumpingPressed){
            currGravity = constantGravity;
        }
        else{
            if(notFalling){
                gravity = initGravity*notFallingMultiplier;
            }
            else{
                gravity = initGravity;
            }


            if(currGravity > maxGravity){
                //fix Euler integration
                float previousVelocity = currGravity;
                float newVelocity = currGravity - gravity * Time.deltaTime;
                float nextVelocity = (previousVelocity + newVelocity)* 0.5f;
                currGravity = newVelocity;

            }

        }

        //set dirrection
        gravityMovement = gravityDirection * -currGravity * Time.deltaTime;

    }


    //Jumping
    //callback function
    public void OnJump(bool jump){
            isJumpingPressed = jump;
    }

    void Jump(){

        if(!isJumping && isGrounded() && isJumpingPressed){
            isJumping = true;
            currGravity = 10f;
        }
        
        else if(!isJumpingPressed && isJumping && canQueueJump){
            isJumping = false;
        }
        
    }

    void CheckStepOffset(){
        if(!isGrounded()){
            currStepOffset = 0;
        }
        else{
            currStepOffset = initStepOffset;
        }
        controller.stepOffset = currStepOffset;

    }

    void QueueJump(){

        Collider[] hitGround = Physics.OverlapSphere(groundCheck.position, groundCheckRange, groundLayers);

        if(hitGround.Length == 0){
            canQueueJump = false;
        }
        else{
            canQueueJump = true;
        }

        hitGround = new Collider[0];

    }
    //see if the player is focused
    void HandleFocus(){

        isFocusing = plo.IsFocused();
        

        if(isFocusing && plo.FocusTarget() != null){
            canFocus = true;
            focusTarget = plo.FocusTarget().transform;
        }
        else{
            canFocus = false;
            focusTarget = null;
            turnFocusTime = turnFocusTimeInit;
        }

    }


    //handle running

    public void StartRun(){

        if(!isExhausted && moveDir.magnitude >= 0.1f){
            speed = runSpeed;
            isRunning = true;           
        }  

    }

    public void StopRun(){
        isRunning = false;
        if(!isExhausted)
            speed = walkSpeed;
    }

    private void HandleRun(){

        if(moveDir.magnitude >= 0.1f){
            if(isRunning && runTimer >= 0){
                
                runTimer -= staminaDecreaseSpeed * Time.deltaTime;
                slider.value = runTimer;
            }
            else if(runTimer < 0 && !isExhausted){
                StopRun();
                isExhausted = true;
                slider.maxValue = exhaustTimeInit;
                speed = exhaustSpeed;
                exhaustTime = exhaustTimeInit;
            }
        }
        else{
            StopRun();
        }


    }

    private void RunRecharge(){

        if(runTimer < runTimerInit && (!isRunning || isExhausted)){
            runTimer += recoverySpeed * Time.deltaTime;
            if(!isExhausted)
                slider.value = runTimer;
        }

    }

    private void HandleExhaustion(){
        //check if you are still exhausted
        if(exhaustTime >= 0){
            exhaustTime -= exhaustRecoverySpeed * Time.deltaTime;
            slider.value = exhaustTimeInit - exhaustTime;
        }
        //set everything to normal
        else{
            if(!isRunning)
                speed = walkSpeed;

            isExhausted = false;
            slider.maxValue = runTimerInit;
            slider.value = runTimer;
        }
    }

    void HandleStaminaUI(){
        
        if(isRunning || isExhausted || (!isExhausted && runTimer < runTimerInit)){
            if(!staminaBar){
                slider.gameObject.SetActive(true);
                staminaBar = true;
            }
                
            if(isExhausted){
                if(!staminaBarExhausted){

                    staminaBarExhausted = true;
                    fill.color = Color.red;
                }
                
            }
            else{
                if(staminaBarExhausted){

                    staminaBarExhausted = false;
                    fill.color = Color.green;
                }
                
            }

        }
        else{
            if(staminaBar){
                slider.gameObject.SetActive(false);
                staminaBar = false;
            }
            
        }
        
    }

    // Update is called once per frame
    void Update(){

        HandleExhaustion();
        RunRecharge();
        HandleRun();
        HandleFocus();
        QueueJump();
        Move(moveDir);
        CalculateGravity();
        Jump();
        CheckStepOffset();
        HandleStaminaUI();
        

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRange);
    }

    public bool GetIsRunning() {
        return isRunning;
    }


}
