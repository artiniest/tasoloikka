using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Controller _playerActions;
    
    [SerializeField] private float damping = 1;
    
    private GameObject _pivot;
    [SerializeField] private float speed = 3.0f;

    [SerializeField] private Transform pitch;
    [SerializeField] private Transform yaw;
    
    private Vector2 _moveInput;

    public bool Loaded = false;
    
    public void Initialize(GameObject go)
    {
        Loaded = true;
        
        _playerActions = new Controller();
        _playerActions.Camera.Enable();
        
        _pivot = GameManager.Instance.PlayerOb;
    }

    private void Update()
    {
        _moveInput = _playerActions.Camera.Move.ReadValue<Vector2>();
        
        yaw.Rotate(0, _moveInput.x * Time.deltaTime * speed, 0);
        pitch.Rotate(_moveInput.y, 0, 0);
    }

    void LateUpdate()
    {
        if (Loaded)
        {
            Debug.Log(_moveInput.x);
            
            transform.Rotate(pitch.localRotation.x, yaw.localRotation.y, 0);
            
            //transform.Rotate(_moveInput.y * Time.deltaTime * speed, _moveInput.x * Time.deltaTime * speed, 0);
            
            // _moveInput = _playerActions.Camera.Move.ReadValue<Vector2>();
            // // _pivot.transform.Rotate(Vector3.up, _moveInput.x * 3);
            //
            // Vector3 currentAngle = transform.eulerAngles;
            // float angleY = Mathf.LerpAngle(currentAngle.y, _pivot.transform.rotation.y, 1);//Time.deltaTime * damping);
            //
            // Quaternion rotation = Quaternion.Euler(0, angleY, 0);
            transform.position = _pivot.transform.position;
            //
            // transform.LookAt(GameManager.Instance.PlayerOb.transform);
        }
    }
}
