using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform Player;
    CameraController CameraScript;
    public bool Vision, ClosenessLimit;
    public LayerMask PlayerLayer;
    public float VisionRadio = 2f, ClosenessRadio = 1f, Speed = 0.1f;
    public float scaleX = 0.4f;
    public float scaleY = 0.4f;
    public bool facingRight = false;
    protected Animator m_Anim;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public float maxX, maxY, minX, minY;
    public bool BossDefeated = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();   
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraScript = MainCamera.GetComponent<CameraController>();   
        m_Anim = this.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    // Update is called once per frame
    void Update() {
        if(CameraScript.InBossArea){
            
            Vision = Physics2D.OverlapCircle(transform.position,VisionRadio, PlayerLayer);
            Vector3 Direction = Player.position - transform.position;
            if(Direction.x < 0) {
                Flip(facingRight);
                facingRight = !facingRight;
            }
            else {
                Flip(facingRight);
                facingRight = !facingRight;
            }
            m_Anim.Play("Run_02");
            transform.position += Direction * Speed * Time.deltaTime;
        }
        while (currentHealth <= 0)
        {
            Destroy(gameObject);
            BossDefeated = true;
            break;
        }
    }
    void FixedUpdate()
    {
        
    }

    protected void Flip(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? -scaleX : scaleX, scaleY, 0);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if(other.tag == "Sword") {
            TakeDamage(15);
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY,maxY);
            transform.position = new Vector2(x, y);
        }
    }
    protected void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
