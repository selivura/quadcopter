using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _settingsGO;
    public void ToggleSettings()
    {
        _settingsGO.SetActive(!_settingsGO.activeSelf);
    }
    public void LoadLevel(int level)
    {
        MapLoader.LoadLevel(level);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
