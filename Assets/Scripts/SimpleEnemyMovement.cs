using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyMovement : MonoBehaviour
{

    public LayerMask playerLayer;
    public NavMeshAgent agent;
    public float trackingRange = 20f;

    public bool canMove = true;
    private bool initDestination = true;
    Collider[] playersInRange = null;

    // Start is called before the first frame update
    void Awake(){
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(0f, transform.position.y, 0f));
    }

    // Update is called once per frame
    void Update(){
        //see if the enemy can move
        if(canMove){
            playersInRange = Physics.OverlapSphere(transform.position, trackingRange, playerLayer);
            if(playersInRange.Length == 0 && !initDestination){
                agent.SetDestination(transform.position);
                
                return;
            }
                
            foreach(Collider player in playersInRange){         
                
                initDestination = false;
                agent.SetDestination(player.gameObject.transform.position);
                break;
            }
            playersInRange = null;
        }


    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }

    public void StartMove(){
        canMove = true;
    }

    public void StopMove(){
        agent.SetDestination(transform.position);
        canMove = false;
    }

 
}
