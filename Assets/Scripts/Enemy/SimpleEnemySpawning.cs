using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleEnemySpawning : MonoBehaviour{

    public Transform enemyPrefab;
    public Transform spawnPoint0;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SpawnEnemy(){
        int spawnGround = (int) Random.Range(0,3.4f);
        Debug.Log(spawnGround);

        switch (spawnGround){
            case 0:
                Instantiate(enemyPrefab, spawnPoint0.position, Quaternion.identity); 
                break;
            case 1:
                Instantiate(enemyPrefab, spawnPoint1.position, Quaternion.identity); 
                break;
            case 2:
                Instantiate(enemyPrefab, spawnPoint2.position, Quaternion.identity); 
                break;
            case 3:
                Instantiate(enemyPrefab, spawnPoint3.position, Quaternion.identity); 
                break;
            default: 
                break;
        }


    }

}
