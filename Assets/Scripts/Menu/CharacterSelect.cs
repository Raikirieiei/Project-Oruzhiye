using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    private string mainMenuScene = "MainMenu";
    // Start is called before the first frame update
    public void StartGame(){
        SceneManager.LoadScene(2);
    }
 
    public void ReturnToMainmenu(){
        SceneManager.LoadScene(mainMenuScene);
    }
}
