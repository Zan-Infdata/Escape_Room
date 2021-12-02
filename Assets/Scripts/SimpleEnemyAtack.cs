using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleEnemyAtack : MonoBehaviour
{

    public float attackDamage = 20f;
    private Collider[] playerInAtackRange;
    private Collider[] playersDamaged;
    public LayerMask playerLayer;
    public Transform attackPoint;
    private MeshRenderer mr;
    public float attackRange = 1f;
    public float attackDetectRange = 1.2f;
    public bool canAttack = true;
    public float attackDelay = 0.5f;
    private SimpleEnemyMovement sem;


    private void Awake() {


        playerLayer = (1 << 10);
        attackPoint = transform.GetChild(1).gameObject.transform;

        mr = attackPoint.gameObject.GetComponent<MeshRenderer>();
        sem = gameObject.GetComponent<SimpleEnemyMovement>();

        mr.enabled = false;
    }

    // Update is called once per frame
    void Update(){

        playerInAtackRange = Physics.OverlapSphere(attackPoint.position, attackDetectRange - 0.2f, playerLayer);
        foreach(Collider player in playerInAtackRange){
            if(canAttack){
                //stop the enemy but wait a little
                sem.StopMove();
                canAttack = false;
                StartCoroutine(EnemyAttack());
                
            }      
        }
        if(playerInAtackRange.Length == 0 && canAttack){
            sem.StartMove();
        }
        
    }

    //attack a player after a delay and stop the movement
    IEnumerator EnemyAttack(){
        

        Debug.Log("Preparing to attack...");
        //delay the attack
        yield return new WaitForSeconds(attackDelay);

        playersDamaged =  Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        foreach(Collider player in playersDamaged){
            //TESTING PURPOSES
            mr.enabled = true;
            Invoke("TestAttack", 0.2f);

            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
        
        playersDamaged = null;
        canAttack = true;
    }


    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackDetectRange);
    }

    private void TestAttack(){
        mr.enabled = false;
    }
}
