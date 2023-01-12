using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10.0f;
    public float RotationSpeed = 180.0f;
    public bool rotateing = false;

    private Alteruna.Avatar _avatar;
    private SpriteRenderer _renderer;

    void Start()
    {
        // Get components
        _avatar = GetComponent<Alteruna.Avatar>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Only let input affect the avatar if it belongs to me
        if (_avatar.IsMe)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                rotateing = true;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                rotateing = false;
            }
            // Set the avatar representing me to be green
            _renderer.color = Color.green;

            // Get the horizontal and vertical axis.
            float _translation = Input.GetAxis("Vertical") * Speed;
            float _rotation = -Input.GetAxis("Horizontal") * RotationSpeed;

            _translation *= Time.deltaTime;
            _rotation *= Time.deltaTime;

            transform.Translate(0, _translation, 0, Space.Self);
            transform.Rotate(0, 0, _rotation);
            if (rotateing)
            {
                 _translation = Speed;
                 _rotation = RotationSpeed;
                _translation *= Time.deltaTime;
                _rotation *= Time.deltaTime;

                transform.Translate(0, _translation, 0, Space.Self);
                transform.Rotate(0, 0, _rotation);
            }
        }
    }
}