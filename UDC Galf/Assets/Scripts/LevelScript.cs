using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
