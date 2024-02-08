using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    [SerializeField] private LevelManager[] levels;

    private GameState currentState;
    private LevelManager currentLevel;
    private int currentLevelIndex = 0;

    public UnityEvent cutsceneStarted, cutsceneEnded;

    public static GameManager instance;

    public enum GameState { Briefing, LevelStart, LevelIn, LevelEnd, GameOver, GameEnd }

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (levels.Length > 0)
        {
            ChangeState(GameState.Briefing, levels[currentLevelIndex]);
        }

        playerHealth.OnDeath += GameOver;
    }

    public void ChangeState (GameState state, LevelManager level)
    {
        currentState = state;
        currentLevel = level;

        switch (currentState)
        {
            case GameState.Briefing:
                StartBriefing();
                break;
            case GameState.LevelStart:
                InitiateLevel();
                break;
            case GameState.LevelIn:
                RunLevel();
                break;
            case GameState.LevelEnd:
                CompleteLevel();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
            default:
                break;
        }
    }

    private void StartBriefing ()
    {
        Debug.Log("Briefing started");
        ChangeState(GameState.LevelStart, currentLevel);

        cutsceneStarted?.Invoke();
    }

    private void InitiateLevel ()
    {
        Debug.Log("Level start");

        currentLevel.StartLevel();
        ChangeState(GameState.LevelIn, currentLevel);
    }

    private void RunLevel()
    {
        Debug.Log("Level In: " + currentLevel.gameObject.name);
    }

    private void CompleteLevel ()
    {
        Debug.Log("Level End");

        ChangeState(GameState.LevelStart, levels[++currentLevelIndex]);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void GameEnd ()
    {
        Debug.Log("Game End - Winner");
    }

    public void CutsceneEnded ()
    {
        cutsceneEnded?.Invoke();
    }
}