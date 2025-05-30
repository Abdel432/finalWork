using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience

    public int xpValue = 1;



    //Logic

    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;


    // Hitbox

    // have to make another collider logic because you cannot inherit from collider
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
        {
            if (GameManager.instance.player != null)
            {
                playerTransform = GameManager.instance.player.transform;
            }
            else
            {
                return; // Skip update if player not found
            }
        }



            if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            // we need chasing to be true here howvwer if it is outside trigger length it will false
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);

                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
                chasing = false;
            }

            // Check for overlaps copy code from collidable
            collidingWithPlayer = false;
            boxCollider.OverlapCollider(filter, hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;
                if (hits[i].tag == "Fighter" && hits[i].name == "Player")
                {
                    collidingWithPlayer = true;
                }
                // The array is not cleaned every time so i must clean it 

                hits[i] = null;
            }
        }



    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);

    }
}
