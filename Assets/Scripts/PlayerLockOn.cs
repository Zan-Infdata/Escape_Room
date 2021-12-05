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
    public float detectRange = 15f;
    public float loseDetectRange = 18f;
    public bool onPlayer;
    public bool isFocused = false;
    private Collider[] enemiesInRange;
    
    [Space]
    [Header("Current selected enemy")]
    public GameObject currEnemy;
    private GameObject dump;

    public GameObject closest = null;
    public GameObject secondClosest = null;

    public LinkedList<GameObject> viableEnemies = new LinkedList<GameObject>();
    private LinkedList<GameObject> enemiesToRemove = new LinkedList<GameObject>();
    private bool clearList = false;

    // Start is called before the first frame update
    void Awake(){
        lot = targetFlag.GetComponent<LockOnTarget>();
        targetFlagMesh = targetFlag.GetComponent<MeshRenderer>();
        targetFlagRenderer = targetFlag.GetComponent<Renderer>();

        dissableFocusBars();
    }

    void Update(){
        
        CheckEnemiesInRange();
        
    }

    void LateUpdate(){
        ClearList();
        CheckEnemiesOutOfRange();
        HandleMesh();
        
    }

    void HandleMesh(){
        if(onPlayer){
            DissableMesh();
        }
        else{
            Invoke("EnableMesh", 0.05f);
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        //check if there is a viable target and it is not already being followed
        if(viableEnemies.Count > 0){
            /*GameObject first = viableEnemies.First.Value;
            if(first != currEnemy){

                lot.Follow(first);
                currEnemy = first;
                viableEnemies.AddLast(first);
                viableEnemies.RemoveFirst();
                onPlayer = false;
                //show flag
                Invoke("EnableMesh", 0.05f);

            }*/
            if(currEnemy == null){
                currEnemy = viableEnemies.First.Value;
            }
            lot.Follow(currEnemy);
            onPlayer = false;
        }
        else{
            if(!onPlayer){
                lot.Follow(gameObject);
                currEnemy = null;
                onPlayer = true;
            }

        }   
        
    }

    private void EnableMesh(){
        targetFlagMesh.enabled = true;
    }

    private void DissableMesh(){
        targetFlagMesh.enabled = false;
    }

    /*private void OnTriggerEnter(Collider enemy) {

        //check if it is an enemy
        if(enemyLayers == (enemyLayers | (1 << enemy.gameObject.layer))){
            
            viableEnemies.AddLast(enemy.gameObject);
        }
   
    }*/

    //check if there are enemies in range
    private void CheckEnemiesInRange(){
        enemiesInRange = Physics.OverlapSphere(transform.position, detectRange, enemyLayers);
        if(enemiesInRange.Length > 0){
            foreach(Collider enemy in enemiesInRange){
                if(!viableEnemies.Contains(enemy.gameObject)){
                    viableEnemies.AddLast(enemy.gameObject);
                    Debug.Log(enemy + " in range");
                }
            }
            
        }

    }

    private void CheckEnemiesOutOfRange(){
        LinkedListNode<GameObject> enemy = viableEnemies.First;
        
        //check if enemies are out of range
        for(int i = viableEnemies.Count - 1; i >= 0; i--){
            if(enemy == null)
                continue;
            if(Vector3.Distance(enemy.Value.transform.position, transform.position) > loseDetectRange){
                viableEnemies.Remove(enemy);
                Debug.Log(enemy.Value.gameObject.name + " out of range");
            }
            enemy = enemy.Next;

        }



    }

    /*private void OnTriggerExit(Collider enemy) {

        //check if it is an enemy
        if(enemyLayers == (enemyLayers | (1 << enemy.gameObject.layer))){
            
            viableEnemies.Remove(enemy.gameObject);      
        }
   
    }*/

    //if the target died remove it from the list
    public void TargetDied(GameObject go){
        
        clearList = true;
       /* if(viableEnemies.Contains(go))
            viableEnemies.Remove(go);
        //if you killed a focused enemy focus others
        if(isFocused){
            Invoke("LockedOn", Time.deltaTime*2);
        }*/
    }

    private void ClearList(){  
        if(clearList){
            viableEnemies.Clear();
            clearList = false;
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


                closest = new GameObject();
                secondClosest = new GameObject();
                closest.transform.position = new Vector3(transform.position.x + loseDetectRange, transform.position.y, transform.position.z);
                secondClosest.transform.position = new Vector3(transform.position.x + loseDetectRange, transform.position.y, transform.position.z);
                //find the closest and second closest to the player
                foreach(GameObject enemy in viableEnemies){
                    if(Vector3.Distance(enemy.gameObject.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position)){
                        secondClosest = closest;
                        closest = enemy;
                        
                    }
                    else if(Vector3.Distance(enemy.gameObject.transform.position, transform.position) < Vector3.Distance(secondClosest.transform.position, transform.position)){
                        secondClosest = enemy;
                    }
                }

                if(currEnemy.Equals(closest)){
                    currEnemy = secondClosest;
                }
                else{
                    currEnemy = closest;
                }

                lot.Follow(currEnemy);
            
            }

        }


    }

    public void LockedOn(){

        if(Time.time >= nextLockOn){
            isFocused = true;
            targetFlagRenderer.material.SetColor("_Color", Color.black);
            enableFocusBars();
            //delay next lock on to prevent spaming
            nextLockOn = Time.time + 1f / lockOnDelay;
            

        }

        hasLockedOn = true;


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

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(transform.position, loseDetectRange);
    }

    private void PrintViableEnemies() {

        foreach (GameObject obj in viableEnemies){
            Debug.Log(obj.name);
        }

    }


}
