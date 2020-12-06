using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    #region Show In Inspector

    [Header("Movement Value")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpHeight;

    [Header("Component Reference")]
    [SerializeField] private Transform _gfxTransform;
    [SerializeField] private GroundCheck _groundCheck;

    #endregion

    #region Variables Globales

    private Transform _playerTransform;
    private Rigidbody2D _playerRb;
    private Vector2 _movementInputs;
    private bool _isJumping;

    #endregion

    #region Public Properties

    public bool IsJumping
    {
        get => _isJumping;
    }

    #endregion

    #region unity LifeCycle

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerTransform = transform;
    }

    private void Update()
    {
        GetMovementsInputs();

        //FlipSprite();
    }

    private void FixedUpdate()
    {
        //Application de la force pour le mouvement du player
        Vector2 force = _movementInputs * _movementSpeed * Time.fixedDeltaTime;
        _playerRb.AddForce(force, ForceMode2D.Impulse);

        //Clamp de la velocité du rigidbody du player
        Vector2 velocity = _playerRb.velocity;
        velocity.x = Mathf.Clamp(_playerRb.velocity.x, -_maxSpeed, _maxSpeed);
        _playerRb.velocity = velocity;

        PlayerJump();

        if (_playerRb.velocity.y > 0 && Input.GetButtonUp("Jump") || _playerRb.velocity.y < 0)
        {
            _playerRb.gravityScale = 3;
        }
    }

    #endregion

    #region Private Methods

    private void GetMovementsInputs()
    {
        //Horizontal Movements Inputs
        float horizontal = Input.GetAxis("Horizontal");
        _movementInputs = new Vector2(horizontal, 0);

        //Flip le Sprite
        if (!Mathf.Approximately(_movementInputs.x, 0))
        {
            _playerTransform.right = _movementInputs;
        }

        //Vertical Movements Inputs
        bool isGrounded = _groundCheck.ICantBelieveItsGround();

        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                _isJumping = true;
            }
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            _gfxTransform.localScale = new Vector2(Mathf.Sign(_playerRb.velocity.x), 1);
        }
    }

    private void PlayerJump()
    {
        if (_isJumping)
        {
            Vector2 jumpForce = Vector2.up * _jumpHeight * Time.fixedDeltaTime;
            _playerRb.gravityScale = 1;
            _playerRb.AddForce(jumpForce, ForceMode2D.Impulse);
            _isJumping = false;
        }
    }

    #endregion
}
