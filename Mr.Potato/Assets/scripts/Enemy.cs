﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Sprite damageEnemy;
    public Sprite deadEnemy;
    public float HP = 2;
    public float minSpinForce = -200f;
    public float maxSpinForce = 200f;
    public GameObject UI100;
    public AudioClip[] Score;

    private Rigidbody2D enemyBody;
    private Transform frontCheck;
    private bool isDead = false;
    private SpriteRenderer curBody;


    private void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        frontCheck = transform.Find("frontCheck");
        curBody = transform.Find("body").GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        enemyBody.velocity = new Vector2(transform.localScale.x * moveSpeed, enemyBody.velocity.y);
        Collider2D[] colliders = Physics2D.OverlapPointAll(frontCheck.position);

        foreach(Collider2D c in colliders)
        {
            if (c.tag == "wall")
            {
                flip();
                break;
            }
        }

        if (HP == 1 && damageEnemy != null)
        {
            curBody.sprite = damageEnemy;
        }

        if (HP <= 0 && !isDead)
        {
            death();
        }
    }

    public void Hurt()
    {
        HP--;
    }

    void death()
    {
        curBody.sprite = deadEnemy;
        SpriteRenderer[] Sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in Sprites)
        {
            s.enabled = false;
        }
        curBody.enabled = true;
        isDead = true;

        Collider2D[] cols = GetComponents<Collider2D>();

        foreach(Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        enemyBody.freezeRotation = false;
        enemyBody.AddTorque(Random.Range(minSpinForce, maxSpinForce));

        Vector3 UI100Pos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Instantiate(UI100, UI100Pos, Quaternion.identity);
        int i = Random.Range(0, Score.Length - 1);
        AudioSource.PlayClipAtPoint(Score[i], transform.position);

        Debug.Log(Quaternion.identity);
    }

    void flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
