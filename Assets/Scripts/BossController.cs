using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform Player;
    CameraController CameraScript;
    public float Speed = 1f;
    public float scaleX = 0.4f;
    public float scaleY = 0.4f;
    public bool facingRight = true;
    protected Animator m_Anim;
    //health
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public HealthBar healthBarPlayer;
    public GameObject bossWall;
    public float maxX, maxY, minX, minY, spawnPX, spawnPY;
    // public AudioSource BackgroundMusicSource;
    // public AudioClip BossClip;
    public bool BossDefeated = false;
    //particles
    public ParticleSystem smoke;
    public float Duration = 0.8f;
    public GameObject Canvas;    // Start is called before the first frame update
    bool executeFade = false;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();   
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraScript = MainCamera.GetComponent<CameraController>();   
        m_Anim = this.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        //particles
        smoke = GameObject.FindGameObjectWithTag("Smoke").GetComponent<ParticleSystem>();
        smoke.Stop();
    }

    // Update is called once per frame
    void Update() {

        healthBar.gameObject.SetActive(false); //deactivate boss health bar when not in boss zone
        bossWall.SetActive(false); //deactivate boss Wall bar when not in boss zone

        if(CameraScript.InBossArea){
            
            //enable health bar
            healthBar.gameObject.SetActive(true); //activate boss health bar when in boss zone
            bossWall.SetActive(true); // activate boss wall when in boss zone
            Vector3 Direction = Player.position - transform.position;
            if(Direction.x < 0) {
                Flip(facingRight);
                facingRight = true;
            }
            else {
                Flip(facingRight);
                facingRight = false;
            }
            // transform.position += Direction * Speed * Time.deltaTime;
            transform.transform.Translate(new Vector3(Direction.x*0.65f, Direction.y*0.65f,0)*Speed*Time.deltaTime);
        }
        
        while (currentHealth <= 0)
        {
            Destroy(gameObject);
            BossDefeated = true;
            break;
        }
        //play die particle 
        if(BossDefeated){
            smoke.Play();
            // healthBarPlayer.gameObject.SetActive(false);
            // healthBar.gameObject.SetActive(false);

            //StartCoroutine(EndScreen());
            executeFade = true;
        }
        if(executeFade) {
            executeFade = false;
            Fade();
        }
    }

    protected void Flip(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? -scaleX : scaleX, scaleY, 0);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Sword") {
            m_Anim.Play("Hit");
            TakeDamage(10);
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY,maxY);
            smoke.transform.position = transform.position;
            transform.position = new Vector2(x, y);
        }
        
    }
    protected void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void Fade(){
        CanvasGroup canvasGroup = Canvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        // StartCoroutine(DoFade(canvasGroup));
    }

    public IEnumerator DoFade(CanvasGroup canvGroup) {
        float start = canvGroup.alpha, end = 1f, counter = 0f;
        while(counter < Duration) {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            Debug.Log(canvGroup.alpha);

            yield return null;
        }
    } 

}
