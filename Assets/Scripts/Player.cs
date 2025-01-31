using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    private void Start()
    {
       boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {

        // using the input manager to declare wasd or arrow keys
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //reset MoveDelta 
        moveDelta = new Vector3(x,y,0);

        // swap sprite direction , wether you're going right or left
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        // make sure we can move in this direction by casting a box there first, f the box returns null, we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            // make player move also allows that computer speed doesnt matter so nomater cpu speed game runs the same speed
            transform.Translate(0,moveDelta.y * Time.deltaTime, 0);

        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // make player move also allows that computer speed doesnt matter so nomater cpu speed game runs the same speed
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

        }


    }
}