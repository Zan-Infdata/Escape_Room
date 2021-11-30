using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Space]
	[Header("Input master")]	
    public InputActionAsset actionAsset;

    private InputActionMap playerMap;
    //all the actions
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction lockOnAction;
    private InputAction runAction;

    //all other scripts
    private PlayerMovement pm;
    private PlayerCombat pc;
    private PlayerLockOn plo;
    
    private void Awake() {

        pm = this.GetComponent<PlayerMovement>();
        pc = this.GetComponent<PlayerCombat>();
        plo = this.GetComponent<PlayerLockOn>();
        //set up all th input methods
        playerMap = actionAsset.FindActionMap("Player");

        actionAsset.Enable();
        //get all the Input Actions
        jumpAction = playerMap.FindAction("Jump");
        attackAction = playerMap.FindAction("Attack");
        moveAction = playerMap.FindAction("Move");
        lockOnAction = playerMap.FindAction("LockOn");
        runAction = playerMap.FindAction("Run");

        jumpAction.started += ctx => pm.OnJump(ctx.ReadValueAsButton());
        jumpAction.canceled += ctx => pm.OnJump(ctx.ReadValueAsButton());

        attackAction.started += ctx => pc.Attack();

        runAction.started += ctx => pm.StartRun();
        runAction.canceled += ctx => pm.StopRun();

        moveAction.performed += ctx => pm.OnMove(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => pm.OnMove(ctx.ReadValue<Vector2>());

        lockOnAction.performed += ctx => plo.LockedOn();
        lockOnAction.canceled += ctx => plo.OnSwap();
        lockOnAction.canceled += ctx => plo.NotLockedOn();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
