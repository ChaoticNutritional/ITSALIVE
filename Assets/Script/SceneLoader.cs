using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public UnityEvent OnLoadBegin = new UnityEvent();
    public UnityEvent OnLoadEnd = new UnityEvent();
    //public ScreenFader screenFader = null;
    public Animator screenFade;
    public GameObject sceneStuffToEnable;
    public GameObject rayControllers;
    public GameObject directControllers;

    private GameObject temp;
    private bool isLoading = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    public void LoadNewScene(string sceneName)
    {
        if (sceneName.CompareTo("MenuScene") == 0 && !rayControllers.activeInHierarchy)
        {
            directControllers.SetActive(false);
            rayControllers.SetActive(true);
        }

        if (!isLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string SceneName)
    {
        isLoading = true;

        OnLoadBegin?.Invoke();
        screenFade.SetTrigger("Fade2Black");
        yield return new WaitForSeconds(1.0f);
        
        yield return StartCoroutine(UnloadCurrent());
        //stuffToDisable.SetActive(false);
        rayControllers.SetActive(false);
        directControllers.SetActive(true);

        
        //TESTING PURPOSES
        yield return new WaitForSeconds(3.0f);

        yield return StartCoroutine(LoadNew(SceneName));
        OnLoadEnd?.Invoke();

        isLoading = false;
    }

    private IEnumerator UnloadCurrent()
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //Debug.Log(unloadOperation.progress);

        while(!unloadOperation.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator LoadNew(string SceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
        Debug.Log("FADING BACK IN!");
        screenFade.SetTrigger("Fade2In");

        if (!sceneStuffToEnable.activeSelf)
        {
            sceneStuffToEnable.SetActive(true);
        }
        else
        {
            sceneStuffToEnable.SetActive(false);
        }
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
}
