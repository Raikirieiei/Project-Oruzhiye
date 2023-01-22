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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
}
