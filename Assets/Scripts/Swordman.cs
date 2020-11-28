using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public GameObject Boss;
    public GameObject BackgroundMusic;
    public GameObject BossMusic;
    //public GameObject sword;
    //public Collider2D sword;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public GameObject GameOver;
    public AudioClip GameOverMusic;
    private int Lives=3;

    private void Start()
    {

        m_CapsulleCollider  = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();

        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraScript = MainCamera.GetComponent<CameraController>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        GameOver.SetActive(false);
        
        Time.timeScale = 1f;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Apple"){
            Heal(5);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" && attackable) 
        {
            TakeDamage(7, other);
        }
        else if(other.gameObject.tag == "BossWall") {
            TakeDamage(0, other);
        }
        else if(other.gameObject.tag == "Boss1") {
            TakeDamage(20, other);
        }
    }
    private void Update()
    {

        if(m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            attackable = false;
        }

        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);

        }

        //Player dies
        while (currentHealth <= 0)
        {
            StartCoroutine(spawn());
            AudioSource.PlayClipAtPoint(DieAudio, transform.position);
            m_Anim.Play("Die");
            healthBar.SetHealth(100);
            currentHealth = 100;
            Lives--;
            if(Lives == 2) {
                Heart3.SetActive(false);
            }
            else if(Lives == 1) {
                Heart2.SetActive(false);
            }
            else {
                Heart1.SetActive(false);
                // trigger start over menu
                BackgroundMusic.SetActive(false);
            }
        }
        // sword.GetComponent<Collider2D>().enabled = false;
        if(Lives == 0){
            // new WaitForSeconds(1f);
            StartCoroutine(GameOverScreen());
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
                    return;
                }
                transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
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
                    return;
                }
                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
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
    public void TakeDamage(int damage, Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(HitAudio, transform.position);
        m_Anim.Play("Die");
        float bounce = 420f; //amount of force to apply
        m_rigidbody.AddForce(collision.contacts[0].normal * bounce);
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
        attackable = false;
        yield return new WaitForSeconds(.68f);
        transform.position = new Vector2(spawnPointX, spawnPointY);
        Boss.transform.position = new Vector2(Boss.GetComponent<BossController>().spawnPX, Boss.GetComponent<BossController>().spawnPY);
        CameraScript.InBossArea = false;
        attackable = false;
        BossMusic.SetActive(false);
        BackgroundMusic.SetActive(true);
    }
    
    IEnumerator GameOverScreen(){
        yield return new WaitForSeconds(.7f);
        Time.timeScale = 0f;
        BackgroundMusic.SetActive(false);
        GameOver.SetActive(true);
        healthBar.gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(GameOverMusic, transform.position);
    }
}
