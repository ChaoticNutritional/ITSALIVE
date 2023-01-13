using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonObject : MonoBehaviour
{
    public string scene;

    public void LoadGame()
    {
        Debug.Log("Clicked on button");
        SceneLoader.Instance.LoadNewScene(scene);
    }
}