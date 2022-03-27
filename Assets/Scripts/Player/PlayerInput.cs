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
    private PlayerAnimation pa;

    
    private void Awake() {

        //get all the scripts
        pm = this.GetComponent<PlayerMovement>();
        pc = this.GetComponent<PlayerCombat>();
        plo = this.GetComponent<PlayerLockOn>();
        pa = this.GetComponent<PlayerAnimation>();
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
        runAction.started += ctx => pa.AnimateRun();
        runAction.canceled += ctx => pm.StopRun();
        runAction.canceled += ctx => pa.AnimateRun();

        moveAction.performed += ctx => pm.OnMove(ctx.ReadValue<Vector2>());
        moveAction.performed += ctx => pa.FocusRun(ctx.ReadValue<Vector2>());
        moveAction.started += ctx => pa.IsWalking();
        moveAction.canceled += ctx => pm.OnMove(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => pa.FocusRun(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => pa.IsNotWalking();

        lockOnAction.started += ctx => plo.LockedOn();
        lockOnAction.started += ctx => pa.AnimateFocus();
        lockOnAction.canceled += ctx => plo.OnSwap();
        lockOnAction.canceled += ctx => plo.NotLockedOn();
        lockOnAction.canceled += ctx => pa.AnimateFocus();


    }


    
}
