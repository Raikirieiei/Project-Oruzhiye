using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    [Header("For Loading Destination")]
    // Start is called before the first frame update
    public int iLevelToLoad;
    public string sLevelToLoad;
    public bool useIntegerToLoadLevel = false;

    [Header("For Checking States")]
    public GameObject enemy;
    public GameObject player;
    private GameObject playerSet;

    private bool isOpen = false;
    private Animator loaderAnim;
    [Header("For Others")]
    [SerializeField] AudioSource openAudio;

    void Start()
    {
        loaderAnim = GetComponent<Animator>();

        Invoke(nameof(Find_player), 1);
        if (player == null) return;

    }

    private void Find_player()
    {
        try
        {
            player = GameObject.FindWithTag("PlayerSet");
        }
        catch (NullReferenceException)
        {
            Debug.Log("target gameObjects is not present in hierarchy ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController();

        if (player == null) return;
        if (enemy == null)
        {
            isOpen = true;
            openAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        GameObject collisionGameObject = collision.gameObject;

        // do nothing if there's enemy in the map
        if (!isOpen) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (useIntegerToLoadLevel)
        {
            if (iLevelToLoad == 0) { // fix this later
                player = GameObject.FindWithTag("PlayerSet");
                Destroy(player);
                SceneManager.LoadScene(iLevelToLoad);
            }else{
            SceneManager.LoadScene(iLevelToLoad);
            }
        }
        else
        {
            if (sLevelToLoad == "MainMenu") { // fix this later
                player = GameObject.FindWithTag("PlayerSet");
                Destroy(player);
                SceneManager.LoadScene(sLevelToLoad);
            }else{
            SceneManager.LoadScene(sLevelToLoad);
            }
        }
    }

    void AnimationController()
    {
        loaderAnim.SetBool("isOpen", isOpen);
    }

}
