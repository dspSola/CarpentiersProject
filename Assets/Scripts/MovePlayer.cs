using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private float _lengt, _coefTimeFall, _timeFall, _forceJump;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _isGrounded, _pressJump, _enterGround;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _velocity.x = Input.GetAxisRaw("Horizontal");
            //transform.position = new Vector2(transform.position.x + Input.GetAxisRaw("Horizontal") / 10f, 0);
        }
        else
        {
            _velocity.x = 0;
        }

        if (Input.GetAxisRaw("Jump") != 0)
        {
            _pressJump = true;
        }
        else
        {
            _pressJump = false;
        }

        if(_isGrounded)
        {
            _timeFall = 0;
        }
        else
        {
            _timeFall += Time.deltaTime * _coefTimeFall;
        }

        transform.position = new Vector2(Mathf.Round(transform.position.x * 100.0f) / 100.0f, Mathf.Round(transform.position.y * 100.0f) / 100.0f);
    }

    private void FixedUpdate()
    {
        RayCastTouchFloor();
        Fall();
        Jump();
        _rigidbody2D.velocity = _velocity;
    }

    private void Fall()
    {
        if(!_isGrounded)
        {
            _velocity.y = Physics2D.gravity.y * _timeFall;
            Mathf.Clamp(_velocity.y, -10, 10);
        }
    }

    private void Jump()
    {
        if (_isGrounded && _pressJump)
        {
            _rigidbody2D.AddForce( new Vector2(0, _forceJump), ForceMode2D.Force);
        }
    }

    private void RayCastTouchFloor()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _lengt, _layerMask);

        // If it hits something...
        if (hit.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            Debug.Log("Distance = " + distance);
            Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);

            if (distance < 0.02f)
            {
                _isGrounded = true;
                if(!_enterGround)
                {
                    _velocity.y = 0;
                    _enterGround = true;
                }
            }
            else
            {
                _isGrounded = false;
                _enterGround = false;
            }
        }
        else
        {
            _isGrounded = false;
            _enterGround = false;
            Debug.DrawRay(transform.position, Vector2.down * _lengt, Color.red);
        }
    }
}
