using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharacterDatabase characterDB;

    // public SpriteRenderer artworkSprite;
    public Button selectButton;

    public Image imgSprite;
    public GameObject pleaseSelectTitle;

    private int selectedOption = 0;

    private string mainMenuScene = "MainMenu";
    private bool isSelected = false;

    public void SwordOption(){
        selectedOption = 0;
        isSelected = true;
        UpdateCharacter(selectedOption);
        Save();
    }

    public void SpearOption(){
        selectedOption = 1;
        isSelected = true;
        UpdateCharacter(selectedOption);
        Save();
    }

    public void GreatSwordOption(){
        selectedOption = 2;
        isSelected = true;
        UpdateCharacter(selectedOption);
        Save();
    }

    private void UpdateCharacter(int selectedOption){
        Character character = characterDB.getCharacter(selectedOption);
        imgSprite.sprite = character.characterSprite;
        if(isSelected){
            imgSprite.enabled = true;
            pleaseSelectTitle.SetActive(false);
            selectButton.interactable = true; 
        }
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
