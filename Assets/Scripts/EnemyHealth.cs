using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour{

    [Space]
    [Header("Health")]
    public float maxHealth = 100f;
    public float currHealth;
    


    // Start is called before the first frame update
    void Awake(){
        currHealth = maxHealth;
        
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    //returns 1 if it died otherwise it returns 0
    public int TakeDammage(float damage){

        currHealth -= damage;
        if(currHealth <= 0){
            Die();
            return 1;
        }
        Debug.Log("Curr health" + currHealth);
        return 0;
    }

    void Die(){
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);

        //currHealth = maxHealth;
        //Debug.Log("Enemy health reset!");
    }




}
