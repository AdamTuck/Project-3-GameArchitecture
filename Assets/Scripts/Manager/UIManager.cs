using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject cutsceneCanvas;

    public TMP_Text txtHealth;
    public GameObject gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.SetActive(false);
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

    void OnHealthUpdate (float health)
    {
        txtHealth.text = "Health: " + Mathf.Ceil(health).ToString();
    }

    void OnDeath ()
    {
        gameOverText.SetActive(true);
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
