using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharacterDatabase characterDB;

    // public SpriteRenderer artworkSprite;

    public Image imgSprite;

    private int selectedOption = 0;

    private string mainMenuScene = "MainMenu";

    public void SwordOption(){
        selectedOption = 0;
        UpdateCharacter(selectedOption);
        Save();
    }

    public void SpearOption(){
        selectedOption = 1;
        UpdateCharacter(selectedOption);
        Save();
    }

    private void UpdateCharacter(int selectedOption){
        Character character = characterDB.getCharacter(selectedOption);
        imgSprite.sprite = character.characterSprite;
    }

    private void Load(){
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void Save(){
        PlayerPrefs.SetInt("selectedOption", selectedOption); 
    }

    public void StartGame(){
        if(!PlayerPrefs.HasKey("selectedOption")){
            PlayerPrefs.SetInt("selectedOption", 0);
        }
        SceneManager.LoadScene(2);
    }
 
    public void ReturnToMainmenu(){
        SceneManager.LoadScene(mainMenuScene);
    }
}
