using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float _speed = 500f;
    public float _jumpForce = 300f;
    public float _acceleration = 1f;
    public float _decceleration = 1f;
    public float _velPower = 1f;
    public float _lastGroundedTime;
    public float _frictionAmount;


    Vector2 _movement;

    Rigidbody2D _rb;

    private bool bFacingRight = true;
    private bool bIsGrounded;



    void Awake() => _rb = GetComponent<Rigidbody2D>();



    // Update is called once per frame
    void FixedUpdate()
    {
        //Get input
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Flip Sprite
        if (_movement.x > 0 && !bFacingRight)
        {
            FlipCharacter();
        }
        else if (_movement.x < 0 && bFacingRight)
        {
            FlipCharacter();
        }


        #region Run
        //calculate the direction to move
        float _targetSpeed = _movement.x * _speed;

        //calculate difference between current velocity and desired velocity
        float _speedDif = _targetSpeed - _rb.velocity.x;

        //change acceleration rate depending on situation
        float _accelRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? _acceleration : _decceleration;

        //applies acceleration to speed difference, the raises to a set power so acceleration increases wioth higher speeds
        //multiplies by sign to  reapply direction
        float _moveTo = Mathf.Pow(Mathf.Abs(_speedDif) * _accelRate, _velPower) * Mathf.Sign(_speedDif);

        //Applies force
        _rb.AddForce(_moveTo * Vector2.right);

        #endregion

        #region Friction
        if(_lastGroundedTime > 0 && Mathf.Abs(_movement.x) < 0.01f) 
        {
            float _amount = Math.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));

            _amount *= Mathf.Sign(_rb.velocity.x);

            _rb.AddForce(Vector2.right * - _amount, ForceMode2D.Impulse);
        
        }
        #endregion


      //  if(Physics2D.OverlapBox(groundChec))

    }



    public void OnMove(InputValue value) => _movement = value.Get<Vector2>();

    public void OnJump(InputValue value)
    {
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void FlipCharacter()
    {
        if (bFacingRight)
            transform.Rotate(0f, 200f, 0f);
        else 
            transform.Rotate(0f, -200f, 0f);

        bFacingRight = !bFacingRight;
      
    }

}
