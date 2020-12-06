using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetinputX(float inputX)
    {
        _animator.SetFloat("InputX", inputX);
        //if(inputX < 0)
        //{
        //    _spriteRenderer.flipX = true;
        //}
        //else if (inputX > 0)
        //{
        //    _spriteRenderer.flipX = false;
        //}
    }
    public void SetLeft(bool value)
    {
        _animator.SetBool("Left", value);
    }
    public void SetIdle(bool value)
    {
        _animator.SetBool("Idle", value);
    }
    public void SetRight(bool value)
    {
        _animator.SetBool("Right", value);
    }
    public void SetGrounded(bool value)
    {
        _animator.SetBool("Grounded", value);
    }
    public void SetFalling(bool value)
    {
        _animator.SetBool("Falling", value);
    }
    public void SetJumping(bool value)
    {
        _animator.SetBool("Jumping", value);
    }
    public void SetClimbing(bool value)
    {
        _animator.SetBool("Climbing", value);
    }

    public void SetFlipX(bool value)
    {
        _spriteRenderer.flipX = value;
    }

}
