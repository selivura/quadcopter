using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene(level + 1, LoadSceneMode.Additive);
    }
    public static void LoadLevel(string level)
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
    }
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
