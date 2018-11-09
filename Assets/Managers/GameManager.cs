using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CoinCount = 0;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private LevelSettings settings;


    private AudioManager audioManager;

    public AudioManager Audio
    {
        get { return audioManager; }
    }

    private EventManager eventManager;
    public EventManager Event
    {
        get { return eventManager; }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadLevelSettings();
        InitializeManagers();
    }

    private void LoadLevelSettings()
    {
        settings = Resources.Load<LevelSettings>(string.Format("LevelSettings/{0}", SceneManager.GetActiveScene().name));
    }

    private void InitializeManagers()
    {
        eventManager = new EventManager();
        audioManager = settings.audioManager;
        audioManager.Attachlisteners();
        audioManager.GetCamera();
    }

    public void PlayFootStepsSound()
    {
        StartCoroutine("FootstepsSound");

    }

    public void StopPlayFootStepsSound()
    {
        StopCoroutine("FootstepsSound");
    }



    private IEnumerator FootstepsSound()
    {
        for (; ; )
        {
            Audio.FootSteps.source.PlayOneShot(Audio.FootSteps.clip);
            yield return new WaitForSeconds(settings.audioManager.footstepsDelay);
        }
    }
    public void DestroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }
}
