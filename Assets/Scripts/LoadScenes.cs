using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    void Awake()
    {
        //SceneManager.LoadScene("Camera", LoadSceneMode.Additive);
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        SceneManager.LoadScene("Map", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Score", LoadSceneMode.Additive);
    }
}
