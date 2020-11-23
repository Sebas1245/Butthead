﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swordman : PlayerController
{


    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public float spawnPointX;
    public float spawnPointY;
    CameraController CameraScript;
    private bool attackable = true;
    public AudioClip HitAudio, AttackAudio, DieAudio, JumpAudio;


    private void Start()
    {

        m_CapsulleCollider  = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();

        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraScript = MainCamera.GetComponent<CameraController>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Apple"){
            Heal(5);
        }
        else if(collision.gameObject.tag == "Enemy" && attackable)
        {
            TakeDamage(7, collision.transform.position, 1.2f);       
        }
        else if(collision.gameObject.tag == "Boss1" && attackable){
            TakeDamage(20, collision.transform.position, 0.8f);       
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" && attackable) 
        {
            TakeDamage(7, other.transform.position, 0.8f);
        }
        else if(other.gameObject.tag == "BossWall") {
            TakeDamage(0, other.transform.position, 1f);
        }   
    }
    private void Update()
    {

        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);

        }

        //Player dies
        while (currentHealth <= 0)
        {
            StartCoroutine(NotAttackable(1.2f));
            StartCoroutine(spawn());
            AudioSource.PlayClipAtPoint(DieAudio, transform.position);
            m_Anim.Play("Die");
            healthBar.SetHealth(100);
            currentHealth = 100;
        }


    }

    public void checkInput()
    {

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))  //아래 버튼 눌렀을때. 
        {

            IsSit = true;
            m_Anim.Play("Sit");
            attackable = true;

        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {

            m_Anim.Play("Idle");
            IsSit = false;
            attackable = true;
        }


        // sit나 die일때 애니메이션이 돌때는 다른 애니메이션이 되지 않게 한다. 
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.W) ||  Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                if (currentJumpCount < JumpCount)  // 0 , 1
                {
                    DownJump();
                }
            }

            return;
        }


        m_MoveX = Input.GetAxis("Horizontal");


   
        GroundCheckUpdate();


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.X))
            {
                StartCoroutine(NotAttackable(1f));
                AudioSource.PlayClipAtPoint(AttackAudio, transform.position);
                m_Anim.Play("Attack");
            }
            else
            {

                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");

                }
                else
                {

                    m_Anim.Play("Run");
                }

            }
        }



        // 기타 이동 인풋.

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            attackable = true;
            if (isGrounded)  // 땅바닥에 있었을때. 
            {

                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    //StartCoroutine(NotAttackable(1f));
                    return;

                }

                transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);

            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                //StartCoroutine(NotAttackable(1f));
                return;
            }

            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.LeftArrow))
                Flip(false);

        }
        else if (Input.GetKey(KeyCode.A) ||  Input.GetKey(KeyCode.LeftArrow))
        {
            attackable = true;
            if (isGrounded)  // 땅바닥에 있었을때. 
            {

                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    //StartCoroutine(NotAttackable(1f));
                    return;
                }


                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);

            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }


            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                //StartCoroutine(NotAttackable(1f));
                return;
            }

            if (!Input.GetKey(KeyCode.D) ||  Input.GetKey(KeyCode.RightArrow))
                Flip(true);


        }


        if (Input.GetKeyDown(KeyCode.W) ||  Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            attackable = true;
            AudioSource.PlayClipAtPoint(JumpAudio, transform.position);
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                //StartCoroutine(NotAttackable(1f));
                return;
            }


            if (currentJumpCount < JumpCount)  // 0 , 1
            {

                if (!IsSit)
                {
                    prefromJump();


                }
                else
                {
                    DownJump();

                }

            }


        }



    }


  


    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");
    }

    protected void TakeDamage(int damage, Vector2 damagePos, float delay)
    {
        StartCoroutine(NotAttackable(delay));
        AudioSource.PlayClipAtPoint(HitAudio, transform.position);
        m_Anim.Play("Die");
        float direction = Mathf.Ceil(transform.position.x-damagePos.x) == 1 ? (1) : (-1);
        transform.transform.Translate(new Vector3((direction*MoveSpeed*0.3f), 0, 0));
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    protected void Heal(int percentage)
    {
        if(currentHealth + percentage <= 100) {
            currentHealth += percentage;
            
        }
        else {
            currentHealth = 100;
        }
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(.68f);
        transform.position = new Vector2(spawnPointX, spawnPointY);
        CameraScript.InBossArea = false;
        attackable = false;
    }

    IEnumerator NotAttackable(float delay){
        attackable = false;
        yield return new WaitForSeconds(delay);
    }

}
