using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour{

    public GameObject follow;
    // Start is called before the first frame update
    void Awake(){

    }

    // Update is called once per frame
    void Update(){
        if(follow != null)
            transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y + 3, follow.transform.position.z);
        
    }
    //starts following a given object
    public void Follow(GameObject g){
        Detach();
        follow = g;
        
    }

    //stops folowing an object
    public void Detach(){
        follow = null;
    }
    //return the object you are currently following
    public GameObject Folowing(){
        return follow;
    }



}
