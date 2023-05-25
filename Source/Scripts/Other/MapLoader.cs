using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    //��� ����� ������� ������ ����� ������ ����� MainMenu
    //����� �������� ���� ������ ����� ������ ����� MainScene
    public static int CurrentLevel { get; private set; } = -1; //-1 �� ������, ����� �� ��������� �������� ������� �����
    public static System.Action onLevelLoaded;
    public static void LoadLevel(int level)
    {
        var actualLevel = level + 1;
        if (level == -1)
            throw new System.Exception("������ ��������� ������� �����!");
        if (CurrentLevel != -1)
            SceneManager.UnloadSceneAsync(CurrentLevel);
        SceneManager.LoadScene(actualLevel, LoadSceneMode.Additive);
        CurrentLevel = actualLevel;
        onLevelLoaded?.Invoke();
    }

}
