using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    public LayerCheck _groundCheck;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void Update()
    {
        _rigidbody.velocity = new Vector3(_direction.x * _speed, _rigidbody.velocity.y);

        bool isJumping = _direction.y > 0;
        if (isJumping)
        {
            if (IsGrounded() && _rigidbody.velocity.y <= 0)
            {
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
        } else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
    }
    bool IsGrounded()
    {
        return _groundCheck.isTouchingLayer;
    }
}
