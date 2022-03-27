using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{

    [Space]
    [Header("Layers")]

    public LayerMask enemyLayers;

    [Space]
    [Header("Attack point and radius")]

    public Transform attackPoint;
    public Vector3 attackRange = new Vector3(2f,0.5f,1.5f);

    [Space]
    [Header("Combat stats")]
    public float baseDamage = 20f;
    public float attackDamage;
    public float attackSpeed = 1.5f;
    private float nextAttack = 0f;

    private MeshRenderer mr;
    private PlayerLockOn plo;

    // Start is called before the first frame update
    void Start(){
        //debug attack mesh 
        mr = attackPoint.GetComponent<MeshRenderer>();
        plo = gameObject.GetComponent<PlayerLockOn>();
        attackDamage = baseDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Attack(){
        //check if attack is off cooldown
        if(Time.time >= nextAttack){
            //debug attack mesh
            mr.enabled = true;
            Invoke("DissableAttackPoint", 0.2f);

            //get all enemies hit and deal damage to them
            int isDead = 0;
            Collider[] hitEnemyes = Physics.OverlapBox(attackPoint.position, attackRange, attackPoint.rotation, enemyLayers);
            foreach(Collider enemy in hitEnemyes){
                //TODO: change the script later
                isDead = enemy.GetComponent<EnemyHealth>().TakeDammage(attackDamage);
                //if the target is dead tell that to the lock on system
                if(isDead == 1){
                    plo.TargetDied(enemy.gameObject);
                }
            }
            //delay next attack
            nextAttack = Time.time + 1f / attackSpeed;
        }

        
    }
    //debuga ttack mesh
    private void DissableAttackPoint(){
        mr.enabled = false;
    }



    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(attackPoint.position,attackRange*2);
    }

}
