using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] BallMovement Ball;
    [SerializeField] GameObject winMenu;

    void Awake() {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
	    Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ShowMenu(bool state) {
        winMenu.SetActive(state);
    }
}
