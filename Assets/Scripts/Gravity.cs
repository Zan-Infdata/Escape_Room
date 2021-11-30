using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour{

    public CharacterController cr;

    [Header("Gravity constants")]
    public float gravity;
    public float currGravity;
    public float constantGravity;
    public float maxGravity;

    private Vector3 gravityDirection;
    private Vector3 gravityMovement;

    void Awake() {
        cr = GetComponent<CharacterController>();    
    }

    #region - Gravity -

    private bool isGrounded(){

        return cr.isGrounded;
    }

    private void CalculateGravity(){

        if(isGrounded()){
            currGravity = constantGravity;
        }
        else{
            if(currGravity > maxGravity){
                currGravity -= gravity * Time.deltaTime;

            }

        }


        gravityMovement = gravityDirection * -currGravity;

    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){

        CalculateGravity();
        
    }
}
