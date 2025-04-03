using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    // private because we don't need to have a box colllider i am going to assign them start or UPDATE state
    private BoxCollider2D boxCollider;
    // an array which holds data of what you hit in a frame 
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
       
    }
    protected virtual void Update()
    {
        //collison work

        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            OnCollide(hits[i]);
            // The array is not cleaned every time so i must clean it myself

             hits[i] = null; 
        }
    }
    // we can use inhertance to change what this does like granting pesos the npc talking to player
    // if it contact this boxcollier do this example
    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in "+ this.name);

    }
}
