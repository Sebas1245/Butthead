using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public float speed;
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    public float scaleX = 2;
    public float scaleY = 2;
    bool facingRight = true;
    private int hitCount = 0;
    protected Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        m_Anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate() {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            Flip(facingRight);
            facingRight = !facingRight;

        }

        Vector2 position = rigidbody2D.position;

        position.x = position.x + direction * speed * Time.deltaTime;
        
        rigidbody2D.MovePosition(position);
    }

    protected void Flip(bool bLeft)
    {

        transform.localScale = new Vector3(bLeft ? -scaleX : scaleX, scaleY, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if(other.tag == "Sword") {
            if(hitCount == 3) {
                Destroy(gameObject);
            }
            else {
                m_Anim.Play("rockHit");
                hitCount++;
            }
        }
    }
}
