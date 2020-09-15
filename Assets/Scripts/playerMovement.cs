using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(new Vector2(Input.GetAxis("Horizontal"), 0)*Time.deltaTime*speed);


    }
}
