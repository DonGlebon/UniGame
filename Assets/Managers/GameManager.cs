using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance { get; set; }
    private AudioManager audioManager { get; set; }
    private UIManager uiManager { get; set; }
    private LevelManager levelManager { get; set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }

    private void OnSceneWasLoaded(Scene scene,LoadSceneMode mode)
    {
        SetupGameManager();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        audioManager = GetComponent<AudioManager>();
        uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
        DontDestroyOnLoad(gameObject);
    }

    private void SetupGameManager()
    {
        uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
        levelManager = (LevelManager)FindObjectOfType(typeof(LevelManager));
        audioManager.Data = levelManager.AudioData;
    }

    public void PickUpCoin()
    {
        audioManager.PlayCoinSound();
        uiManager.UpdateCoins();
    }

    public void Jump()
    {
        audioManager.PlayJumpSound();
    }
}
