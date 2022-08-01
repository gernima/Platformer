using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    public LayerCheck _groundCheck;
    private Vector2 _direction;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isGrounded;
    private bool _allowDoubleJump;

    private static readonly int isGroundKey = Animator.StringToHash("isGround");
    private static readonly int verticalVelocityKey = Animator.StringToHash("verticalVelocity");
    private static readonly int isRunningKey = Animator.StringToHash("isRunning");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _isGrounded = IsGrounded();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        _animator.SetBool(isGroundKey, _isGrounded);
        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
        _animator.SetBool(isRunningKey, _direction.x != 0);

        UpdateSpriteDirection();
    }
    float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        bool isJumpPressing = _direction.y > 0;

        if (_isGrounded) _allowDoubleJump = true;
        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
            
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }
    float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (_isGrounded)
        {
            yVelocity += _jumpSpeed;
        } else if (_allowDoubleJump)
        {
            yVelocity = _jumpSpeed;
            _allowDoubleJump = false;
        }

        return yVelocity;
    }
    void UpdateSpriteDirection()
    {
        if (_direction.x > 0) {
            _spriteRenderer.flipX = false;
        } else if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
        } 
    }
    bool IsGrounded(){
        return _groundCheck.isTouchingLayer;
    }
     
}
