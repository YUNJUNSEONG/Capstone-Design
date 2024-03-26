using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic0 : MonoBehaviour
{
    Collider myCollider;
    Renderer myRenderer;
    
    void Awake()
    {
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();
    }
    
    void Start()
    {
        if(myCollider) {myCollider.enabled = false;}
        if(myRenderer) {myRenderer.enabled = false;}
    }
    
    public void EnableComponents()
    {
        if(myCollider) {myCollider.enabled = true;}

        if(myRenderer) {myRenderer.enabled = true;}
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (this.name == "Magic-0") 
        {
            collision.transform.position = new Vector3(5.15f, 0.1f, 116.11f);
        }
        if (this.name == "Magic-1_1") 
        {
            collision.transform.position = new Vector3(-98.98f, 1.27f, 159.98f);
        }
        if (this.name == "Magic-1_2") 
        {
            collision.transform.position = new Vector3(98.11f, 1.27f, 150.22f);
        }

    }

}
