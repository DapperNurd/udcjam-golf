using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioSource music;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        music.Play();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
