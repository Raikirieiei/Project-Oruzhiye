using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState State;

    public static event Action<GameState> OnGameStateChanged;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (instance == null) {
           instance = this;
       } else if (instance != this)
     {
         Destroy (gameObject);
       }
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        UpdateGameState(GameState.Normal);
    }

    void Update() {

    }

    // Update is called once per frame
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        Debug.Log(State);
        switch (newState)
        {
            case GameState.Normal:
                break;
            case GameState.RewardSelect:
                HandleRewardSelect();
                break;
            case GameState.AdjustStat:
                break;
            case GameState.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleRewardSelect() {
        Time.timeScale = 0;
    }
}

public enum GameState
{
    Normal,
    RewardSelect,
    AdjustStat,
    Dead,
}
