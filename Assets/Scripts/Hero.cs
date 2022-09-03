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
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private int _coins;

    [SerializeField] private LayerMask _interactionLayer;

    [SerializeField] private SpawnComponent _footStepParticles;
    [SerializeField] private ParticleSystem _hitParticles;

    private Collider2D[] _interactionResult = new Collider2D[1];
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
    private static readonly int hit = Animator.StringToHash("hit");

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
            transform.localScale = Vector3.one;
            _spriteRenderer.flipX = false;
        } else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } 
    }
    bool IsGrounded(){
        return _groundCheck.isTouchingLayer;
    }
    public void TakeDamage()
    {
        _animator.SetTrigger(hit);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

        SpawnCoins();
    }

    public void AddCoins(int count)
    {
        _coins += count;
        Debug.Log(_coins);
    }

    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(_coins, 5);
        _coins -= numCoinsToDispose;

        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);

        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }
    public void Interact()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);
        for (int i=0; i<size; i++)
        {
            _interactionResult[i].GetComponent<InteractableComponent>()?.Interact();
        }
    }
    public void SpawnFootDust()
    {
        _footStepParticles.Spawn();
    }
}
