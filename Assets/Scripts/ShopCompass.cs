using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCompass : MonoBehaviour
{
    
    public int shopVisited = 0;
    public LoadLevel levelLoader;
    [SerializeField] string[] mapName = {
        "Map1-B",
        "Map2-B",
        "Map3-B",
    };

    void Start()
    {
        assignMap();
    }

    private void OnLevelWasLoaded(int level)
    {
        assignMap();
    }

    private void assignMap()
    {
        levelLoader = GameObject.FindWithTag("Loading Zone").GetComponent<LoadLevel>();
        
        if (levelLoader.sLevelToLoad == "")
        {
            levelLoader.sLevelToLoad = mapName[shopVisited];
            shopVisited += 1;
        }
    }
}
