using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName1;
    [SerializeField] private string sceneName2;
    [SerializeField] private string sceneName3;
    [SerializeField] private string sceneName4;

    public void LoadScene1()
    {
        FadeManager.Instance.FadeAndLoadScene(sceneName1); 
    }

    public void LoadScene2()
    {
        FadeManager.Instance.FadeAndLoadScene(sceneName2);
    }

    public void LoadScene3()
    {
        FadeManager.Instance.FadeAndLoadScene(sceneName3);
    }

    public void LoadScene4()
    {
        FadeManager.Instance.FadeAndLoadScene(sceneName4);
    }

    public void QuitGame()
    {
        Debug.Log("ゲームを終了します");
        Application.Quit();
    }
}