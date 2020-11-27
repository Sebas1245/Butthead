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
    //private Queue Hearts = new Queue.<GameObject>();
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
        else if(collision.gameObject.tag == "Enemy" && attackable)
        {
            TakeDamage(7, collision.transform.position, 1.2f, true);       
        }
        else if(collision.gameObject.tag == "Boss1" && attackable){
            TakeDamage(20, collision.transform.position, 0.8f, true);       
        }
        else if(collision.gameObject.tag == "Spikes" && attackable){
            TakeDamage(7, collision.transform.position, 2, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" && attackable) 
        {
            TakeDamage(7, other.transform.position, 0.8f, true);
        }
        else if(other.gameObject.tag == "BossWall") {
            TakeDamage(0, other.transform.position, 1f, true);
        }
        else if(other.gameObject.tag == "Boss1") {
            TakeDamage(20, other.transform.position, 0.8f, true);
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
        // Debug.Log(m_Anim.GetCurrentAnimatorStateInfo(0));
        if(Input.GetKeyDown(KeyCode.Alpha1)){   //BORRAR
            TakeDamage(50, transform.position, 2, false);
        }
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
                //sword.GetComponent<Collider2D>().enabled = true;
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
                    // StartCoroutine(NotAttackable(1f));
                    return;
                }
                transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                // StartCoroutine(NotAttackable(1f));
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
                    // StartCoroutine(NotAttackable(1f));
                    return;
                }
                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                // StartCoroutine(NotAttackable(1f));
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

    protected void TakeDamage(int damage, Vector2 damagePos, float delay, bool moveX)
    {
        StartCoroutine(NotAttackable(delay));
        AudioSource.PlayClipAtPoint(HitAudio, transform.position);
        float direction = Mathf.Ceil(transform.position.x-damagePos.x) == 1 ? (1) : (-1);
        if(moveX) {
            //transform.transform.Translate(new Vector3((direction*MoveSpeed*0.3f), 0, 0));
            m_Anim.Play("Die");
            m_rigidbody.MovePosition(new Vector3(transform.position.x+direction, 0, 0));
        }
        else {
            // transform.transform.Translate(new Vector3((direction*MoveSpeed*0.3f), MoveSpeed*0.5f, 0));
            m_rigidbody.MovePosition(new Vector3(transform.position.x+direction, 2f, 0));
        }
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
        Boss.transform.transform.Translate(new Vector2(Boss.GetComponent<BossController>().spawnPX, Boss.GetComponent<BossController>().spawnPY));
        CameraScript.InBossArea = false;
        attackable = false;
        BossMusic.SetActive(false);
        BackgroundMusic.SetActive(true);
    }

    IEnumerator NotAttackable(float delay){
        Debug.Log("Not attackable");
        attackable = false;
        yield return new WaitForSeconds(delay);
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
