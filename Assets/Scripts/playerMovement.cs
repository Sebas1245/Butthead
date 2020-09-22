using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 5f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(new Vector2(Input.GetAxis("Horizontal"), 0)*Time.deltaTime*speed);
        anim.SetFloat("MoveX",Input.GetAxis("Horizontal"));

    }
}
