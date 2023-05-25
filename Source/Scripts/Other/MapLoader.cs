using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    //Все сцены уровней должны идити строго после MainMenu
    //Сцена главного меню должна идити строго после MainScene
    public static int CurrentLevel { get; private set; } = -1; //-1 со старта, чтобы не выгрузить случайно главную сцену
    public static System.Action onLevelLoaded;
    public static void LoadLevel(int level)
    {
        var actualLevel = level + 1;
        if (level == -1)
            throw new System.Exception("Нельзя выгрузить главную сцену!");
        if (CurrentLevel != -1)
            SceneManager.UnloadSceneAsync(CurrentLevel);
        SceneManager.LoadScene(actualLevel, LoadSceneMode.Additive);
        CurrentLevel = actualLevel;
        onLevelLoaded?.Invoke();
    }

}
