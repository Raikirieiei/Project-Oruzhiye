using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public int iLevelToLoad;
    public string sLevelToLoad;

    public bool useIntegerToLoadLevel = false;

    public GameObject player;
    private GameObject playerSet;

    void Start()
    {
        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    private void Find_player()
    {
        try
        {
            player = GameObject.FindWithTag("Player");
        }
        catch (NullReferenceException)
        {
            Debug.Log("target gameObjects is not present in hierarchy ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collision.gameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (useIntegerToLoadLevel)
        {
            if (iLevelToLoad == 0) { // fix this later
                player = GameObject.FindWithTag("Player");
                Destroy(player);
                SceneManager.LoadScene(iLevelToLoad);
            }else{
            SceneManager.LoadScene(iLevelToLoad);
            }
        }
        else
        {
            if (sLevelToLoad == "MainMenu") { // fix this later
                player = GameObject.FindWithTag("Player");
                Destroy(player);
                SceneManager.LoadScene(sLevelToLoad);
            }else{
            SceneManager.LoadScene(sLevelToLoad);
            }
        }
    }
}
