using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLockOn : MonoBehaviour{

    public GameObject targetFlag;
    private MeshRenderer targetFlagMesh;
    private Renderer targetFlagRenderer;
    private LockOnTarget lot;

    public Image focusBar_0;
    public Image focusBar_1;

    public float lockOnDelay = 2f;
    private float nextLockOn;
    private bool hasLockedOn = false;

    [Header("Range and Layers")]
    public LayerMask enemyLayers;
    private bool onPlayer;
    public bool isFocused = false;
    
    [Space]
    [Header("Current selected enemy")]
    public GameObject currEnemy;
    private GameObject dump;

    public LinkedList<GameObject> viableEnemies = new LinkedList<GameObject>();

    // Start is called before the first frame update
    void Awake(){
        lot = targetFlag.GetComponent<LockOnTarget>();
        targetFlagMesh = targetFlag.GetComponent<MeshRenderer>();
        targetFlagRenderer = targetFlag.GetComponent<Renderer>();

        dissableFocusBars();
    }

    // Update is called once per frame
    void Update(){
        //check if there is a viable target and it is not already being followed
        if(viableEnemies.Count > 0){
            GameObject first = viableEnemies.First.Value;
            if(first != currEnemy){

                lot.Follow(first);
                currEnemy = first;
                viableEnemies.AddLast(first);
                viableEnemies.RemoveFirst();
                onPlayer = false;
                //show flag
                Invoke("EnableMesh", 0.05f);

            }
        }
        else{
            if(!onPlayer){
                lot.Follow(gameObject);
                currEnemy = null;
                onPlayer = true;
                //hide the flag
                targetFlagMesh.enabled = false;
            }

        }   

        Debug.Log(Time.time >= nextLockOn);
        
    }

    private void EnableMesh(){
        targetFlagMesh.enabled = true;
    }

    private void OnTriggerEnter(Collider enemy) {

        //check if it is an enemy
        if(enemyLayers == (enemyLayers | (1 << enemy.gameObject.layer))){
            
            viableEnemies.AddLast(enemy.gameObject);
        }
   
    }

    private void OnTriggerExit(Collider enemy) {

        //check if it is an enemy
        if(enemyLayers == (enemyLayers | (1 << enemy.gameObject.layer))){
            
            viableEnemies.Remove(enemy.gameObject);      
        }
   
    }

    //if the target died remove it from the list
    public void TargetDied(GameObject go){
        if(viableEnemies.Contains(go))
            viableEnemies.Remove(go);
        //if you killed a focused enemy focus others
        if(isFocused){
            Invoke("LockedOn", Time.deltaTime*2);
        }
    }

    //callback function that changes targets
    public void OnSwap(){
        //prevent spamming
        if(Time.time >= nextLockOn && hasLockedOn){

            if(viableEnemies.Count > 1){

                dump = viableEnemies.First.Value;

                viableEnemies.AddLast(dump);
                viableEnemies.RemoveFirst();

                lot.Follow(viableEnemies.First.Value);
                currEnemy = viableEnemies.First.Value;
            
            }

        }


    }

    public void LockedOn(){

        if(Time.time >= nextLockOn){
            isFocused = true;
            targetFlagRenderer.material.SetColor("_Color", Color.blue);
            enableFocusBars();
            //delay next lock on to prevent spaming
            nextLockOn = Time.time + 1f / lockOnDelay;
            hasLockedOn = true;

        }



    }

    public void NotLockedOn(){
        
        isFocused = false;
        targetFlagRenderer.material.SetColor("_Color", Color.green);
        dissableFocusBars();
        //reset spam prevention
        hasLockedOn = false;
    }

    private void enableFocusBars(){
        focusBar_0.enabled = true;
        focusBar_1.enabled = true;
    }
    private void dissableFocusBars(){
        focusBar_0.enabled = false;
        focusBar_1.enabled = false;

    }

    //return if the target is focused
    public bool IsFocused(){
        return isFocused;
    }
    //return game object you are focusing on
    public GameObject FocusTarget(){
        if (currEnemy == null)
            return null;
        else
            return currEnemy;
    }


}
