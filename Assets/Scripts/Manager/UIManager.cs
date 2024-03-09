using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject cutsceneCanvas;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private float tutorialTimeout;

    public TMP_Text txtHealth;
    public GameObject gameOverText;
    public GameObject victoryText;

    private float tutorialTimer;
    private bool tutorialShowing;

    public static UIManager instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    void Start()
    {
        gameOverText.SetActive(false);
    }

    private void Update()
    {
        if (tutorialShowing)
            TutorialTimeout();
    }

    private void OnEnable()
    {
        playerHealth.OnHealthUpdated += OnHealthUpdate;
        playerHealth.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthUpdated -= OnHealthUpdate;
        playerHealth.OnDeath -= OnDeath; // Maybe don't need this line
    }

    public void ShowTutorial (string tutorialType)
    {
        switch (tutorialType)
        {
            case "shootTutorial":
                tutorialText.text = "Click to fire Bullets at Crates.\nPress 1 to fire Bullets, and 2 to fire Rockets.\nBullets destroy Grey Boxes, Rockets destroy Black Boxes.";
                break;
            case "enemyTutorial":
                tutorialText.text = "Don't let the enemy robot see you.\nYour attacks cannot hurt enemy robots.";
                break;
            case "robotTutorial":
                tutorialText.text = "Press F while aiming at the floor to queue commands for your assistant robot.\nThe assistant robot can help block laser beams for you.";
                break;
            case "grabTutorial":
                tutorialText.text = "Press E to Pick Up/Drop Boxes.";
                break;
            case "laserTutorial":
                tutorialText.text = "Boxes block laser beams from turrets.";
                break;
        }

        tutorialText.gameObject.SetActive(true);
        tutorialTimer = 0;
        tutorialShowing = true;
    }

    private void TutorialTimeout ()
    {
        tutorialTimer += Time.deltaTime;

        if (tutorialTimer >= tutorialTimeout)
        {
            tutorialShowing = false;
            tutorialTimer = 0;
            tutorialText.gameObject.SetActive(false);
        }
    }

    void OnHealthUpdate (float health)
    {
        txtHealth.text = "Health: " + Mathf.Ceil(health).ToString();
    }

    void OnDeath ()
    {
        gameOverText.SetActive(true);
    }

    public void OnVictory ()
    {
        victoryText.SetActive(true);
    }

    public void OnRespawnUI ()
    {
        gameOverText.SetActive(false);
    }

    public void OnCutsceneBegin ()
    {
        cutsceneCanvas.SetActive(true);
    }

    public void OnCutsceneEnd ()
    {
        cutsceneCanvas.SetActive(false);
    }
}
