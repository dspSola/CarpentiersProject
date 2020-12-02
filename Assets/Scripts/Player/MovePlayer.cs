using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private float _speed, _lengt, _coefTimeFall, _timeFall, _forceJump, _timeInJump, _timeInJumpMax;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _isGrounded, _enterGround, _pressJump, _isInJump;

    [SerializeField] private Transform _worldParrent;

    [SerializeField] private List<RayCastFloor> _rayCastFloors;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _velocity.x = Input.GetAxisRaw("Horizontal") * _speed;
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

        if (_isGrounded || _isInJump)
        {
            _timeFall = 0;
        }
        else
        {
            _timeFall += Time.deltaTime * _coefTimeFall;
        }
    }

    private void FixedUpdate()
    {
        AllRayCastFloors();
        //RayCastTouchFloor();
        Fall();
        Jump();
        _rigidbody2D.velocity = _velocity;
    }

    private void Fall()
    {
        if (!_isGrounded && !_enterGround)
        {
            _velocity.y = Physics2D.gravity.y * _timeFall;
            Mathf.Clamp(_velocity.y, -10, 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ZIZI");
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("ZI2ZI2");

            //transform.SetParent(collision.gameObject.transform);
            transform.parent = collision.transform;
        }

        //_isGrounded = true;
        //if (!_enterGround)
        //{
        //    _velocity.y = 0;
        //    _enterGround = true;
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //_isGrounded = false;
            //_enterGround = false;
            //transform.SetParent(null);
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void Jump()
    {
        if (_isGrounded && _pressJump)
        {
            _isInJump = true;
            //_rigidbody2D.AddForce( new Vector2(0, _forceJump), ForceMode2D.Force);
        }

        if (_isInJump && _pressJump)
        {
            if (_timeInJump < _timeInJumpMax)
            {
                _timeInJump += Time.deltaTime;
                _rigidbody2D.AddForce(new Vector2(0, _forceJump - _timeInJump / 100), ForceMode2D.Force);
            }
            else
            {
                _timeInJump = 0;
                _isInJump = false;
            }
        }
        else
        {
            _timeInJump = 0;
            _isInJump = false;
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

            if (distance < 0.01f)
            {
                //if(_velocity.y < 0)
                //{
                //    _velocity.y += Time.deltaTime;
                //}
                _isGrounded = true;
                if (!_enterGround)
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

    private void AllRayCastFloors()
    {
        if (!_isInJump)
        {
            bool oneTouchFloor = false;
            for (int i = 0; i < _rayCastFloors.Count; i++)
            {
                if (_rayCastFloors[i].RayCastTouchFloor())
                {
                    oneTouchFloor = true;
                }

                if (oneTouchFloor)
                {
                    _isGrounded = true;
                    if (!_enterGround)
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
        }
        else
        {
            _isGrounded = false;
            _enterGround = false;
        }
    }
}