using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterList;
    public Transform spawnPoint;

    void Start(){
        int selectedCharacter = PlayerPrefs.GetInt("selectedOption");
        GameObject prefab = characterList[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

}