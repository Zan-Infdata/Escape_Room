using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour{

    private float maxHealth = 100;
    public float currHealth;

    public Button btn;

    public Slider slider;


    // Start is called before the first frame update
    void Start(){
        
        slider.maxValue = maxHealth;
        ResetPlayerHealth();
        
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void TakeDamage(float damageTaken){
        Debug.Log("Player was hurt for " + damageTaken);
        currHealth -= damageTaken;
        SetHealthBar();
        if(currHealth <= 0){
            PlayerDied();
        }
    }

    private void PlayerDied(){
        Debug.Log("You died");
        Time.timeScale = 0;
        btn.gameObject.SetActive(true);
    }

    public void ResetPlayerHealth(){

        //REMOVE ONLY FOR PLAYTESTING
        transform.position = new Vector3(0f,7f,0f);


        currHealth = maxHealth;
        Time.timeScale = 1;
        btn.gameObject.SetActive(false);
        SetHealthBar();
    }

    void SetHealthBar(){
        slider.value = currHealth;
        
    }

}
