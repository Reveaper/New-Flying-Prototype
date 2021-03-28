using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitchController : MonoBehaviour
{
    private static SceneSwitchController _instance;

    public static SceneSwitchController Instance { get { return _instance; } }

    [SerializeField] private GameObject _container;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!_container.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                _container.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _container.SetActive(false);
            }
        }

        if(_container.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneManager.LoadScene(1);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
