using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly object _padlock = new object();

    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    //Prefabs/systems to spawn
    [SerializeField] private GameObject _playerPrefab;
    private GameObject playerObject;
    public GameObject PlayerOb
    {
        get { return playerObject; }
    }
    
    [SerializeField] private SmoothCamera _mainCamera;

    public static Action LoadingDone;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        playerObject = Instantiate(_playerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);

        yield return new WaitForSeconds(1);
        
        var go = Instantiate(_mainCamera.gameObject, Vector3.zero, Quaternion.identity);
        go.GetComponent<SmoothCamera>().Initialize(playerObject);

        yield return new WaitUntil(() => _mainCamera.Loaded);
        
        Debug.Log("Initialized!");
        LoadingDone.Invoke();
    }
}
