using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Controller _playerActions;

    [SerializeField] private float maxSpeed = 3.5f;
    [SerializeField] private int maxJumps = 2;

    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpHeightModifier = 100f;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Range(1, 10)] [SerializeField] private float jumpVelocity;
    
    private bool _moving = false;
    private Vector3 _moveInput;

    private bool _grounded = true;
    private bool _jumpPressed = false;
    private int _jumps;

    private Transform _t;
    private Rigidbody _rb;
    
  private void Awake()
  {
      Initialize();
  }

  public void Initialize()
  {
      _playerActions = new Controller();
      // _playerActions.UI.Enable();
      _playerActions.Player.Enable();

      _t = transform;
      _rb = GetComponent<Rigidbody>();
  }

  private void Update()
  {
      _moveInput = _playerActions.Player.Move.ReadValue<Vector3>();

      if (_grounded)
      {
          _jumps = 0;
      }
      
      if (maxJumps > _jumps && _playerActions.Player.Jump.triggered)
      {
          _jumps++;
          _jumpPressed = true;
          _grounded = false;
      }
      
      if ((_moveInput.x != 0 || _moveInput.y != 0) && !_moving)
      {
          _moving = true;
      }
      else if (_moveInput is { x: 0, y: 0 } && _moving)
      {
          _moving = false;
      }
  }

  private void FixedUpdate()
  {
      if (_rb.velocity.y < 0)
      {
          _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
      }
      else if (_rb.velocity.y > 0 && !_jumpPressed)
      {
          _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
      }
      
      if (_jumpPressed)
      {
          _jumpPressed = false;
          _rb.velocity += Vector3.up * jumpVelocity;
      }
      
      Vector3 position = _rb.position + _moveInput * maxSpeed * Time.fixedDeltaTime;
      _t.LookAt(position);
      _rb.MovePosition(position);
  }

  private void OnCollisionEnter(Collision collision)
  {
      if (collision.gameObject.tag.Contains("Ground"))
      {
          _grounded = true;

          _rb.mass = 1;
      }
  }
}
