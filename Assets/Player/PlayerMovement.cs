using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    Vector2 _movement;
    Rigidbody2D _rb;

    #region Run

    [Header("Run")]
    public float _speed = 500f;
    public float _acceleration = 1f;
    public float _decceleration = 1f;
    public float _velPower = 1f;
   
    private bool bFacingRight = true;

    #endregion

    #region Friction

    [Header("Friction")]
    public float _frictionAmount;
    public float _lastGroundedTime;

    #endregion

    #region Jump

    [Header("Jump")]
    public float _jumpForce = 300f;
    public int _maxjumpCount;
    
    public Transform _cellingCheck;
    public Transform _groundCheck;
    public LayerMask _groundObjects;
    public float _checkRadius;
   
    private int _jumpCount;
    private bool bIsGrounded;
    private bool bIsJumping = true;

    #endregion

    #region Dash

   
    private bool bCanDash = true;
    private bool bIsDashing;
    [Header("Dash")]
    public float _dashingPower = 2;
    public float _dashingTime = 2;
    public float _dashingCooldown = 2;

    public TrailRenderer _tr;

    #endregion


    void Awake() => _rb = GetComponent<Rigidbody2D>();

    private void Start() => _jumpCount = _maxjumpCount;


    // Update is called once per frame
    void FixedUpdate()
    {
     
        //Get movement input
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (bIsDashing)
        {
            return;
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

        #region Jump

        bIsGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundObjects);

        if (bIsGrounded)
        {
            _jumpCount = _maxjumpCount;
            bIsJumping = true;
        }

        #endregion


        //Flip Sprite
        if (_movement.x > 0 && !bFacingRight)
        {
            FlipCharacter();
        }
        else if (_movement.x < 0 && bFacingRight)
        {
            FlipCharacter();
        }

        // DebugVariables();

    }


    public void OnMove(InputValue value) => _movement = value.Get<Vector2>();

    public void OnJump(InputValue value)
    {

    
        if(bIsJumping && _jumpCount > 0)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpCount--;
        }
        bIsJumping = false;
       
    }

    public void OnDash(InputValue value)
    {
        if(bCanDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        bCanDash = false;
        bIsDashing = true;
        float _originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        if(_movement.x == 0)
        {
            if(bFacingRight)
            {
                _rb.velocity = new Vector2(transform.localScale.x * _dashingPower,0f);
            }
            if(!bFacingRight)
            {
                _rb.velocity = new Vector2(-transform.localScale.x * _dashingPower, 0f);
            }
        }
        else
        {
            _rb.velocity = new Vector2(_movement.x * _dashingPower, 0f);
        }
       
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = _originalGravity;
        bIsDashing= false;
        yield return new WaitForSeconds(_dashingCooldown);
        bCanDash= true;
    }

    private void FlipCharacter()
    {

        //probably change when adding sprite to player
        if (bFacingRight)
            transform.Rotate(0f, 200f, 0f);
        else 
            transform.Rotate(0f, -200f, 0f);
        //

        bFacingRight = !bFacingRight;
      
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _checkRadius);
    }

    private void DebugVariables()
    {

        Debug.Log("IsGrounded? " + bIsGrounded);
        Debug.Log("IsOnAir? " + bIsJumping);
        Debug.Log("JumpCount? " + _jumpCount);
             

    }

}
