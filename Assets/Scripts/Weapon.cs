using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // damage structure

    // set new coded values and made them public and made them an array
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7};
    public float[] pushForce = { 2.0f, 2.2f, 2.5f, 3f, 3.2f, 3.5f, 4f };


    // Upgrade weapon 

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    // to moniter if player is able to swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        // filters player when it collides with enemy
       if(coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            // create a new damage object then i can send it to the fighter i hit
            Damage dmg = new Damage
            {
                // fixed this as it is an array now so added weaponLevel to it
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);

            Debug.Log(coll.name);

       
            
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }



    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // changing stats 
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

    }
  
}

