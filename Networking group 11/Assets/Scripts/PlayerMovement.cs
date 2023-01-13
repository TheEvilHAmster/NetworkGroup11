using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 7f;

    private Alteruna.Avatar avatar;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    
    void Update()
    {
        if (avatar.IsMe)
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * Speed * Time.deltaTime;

            spriteRenderer.color = Color.green;
        }

        
    }
}
