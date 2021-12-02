using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyMovement : MonoBehaviour
{
    public bool playerSighted = false;
    public LayerMask playerLayer;
    public NavMeshAgent agent;
    public float trackingRange = 5f;
    Collider[] playersInRange = null;

    // Start is called before the first frame update
    void Awake(){
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update(){

        playersInRange = Physics.OverlapSphere(transform.position, trackingRange, playerLayer);
        if(playersInRange.Length == 0){
            agent.SetDestination(transform.position);
            playerSighted = false;
            return;
        }
            
        foreach(Collider player in playersInRange){         
            playerSighted = true;
            agent.SetDestination(player.gameObject.transform.position);
            break;
        }
        playersInRange = null;

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }


}
