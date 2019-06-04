using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePrefab : MonoBehaviour
{
    //public Transform prefab;
    // Start is called before the first frame update

    void OnCollisionEnter(Collision other)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "shape")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("this is a shape");
        }
    }
}
