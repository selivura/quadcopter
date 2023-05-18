using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetuper : MonoBehaviour
{
    [SerializeField] PlayerGUI[] _GUIs;
    [SerializeField] GameObject _SelectPlayerLabel;
    private void Start()
    {
        foreach (var gui in _GUIs)
        {
            gui.gameObject.SetActive(false);
        }
    }
    public PlayerGUI SpawnGUIForPlayer(QuadcopterController copter)
    {
        _SelectPlayerLabel.SetActive(false);
        foreach (var gui in _GUIs)
        {
            if (gui.Copter == null)
            {
                gui.Copter = copter;
                gui.gameObject.SetActive(true);
                return gui;
            }
        }
        throw new System.Exception("No GUIS");
    }
    public void ToggleGUI(int id)
    {
        for (int i = 0; i < _GUIs.Length; i++)
        {
            if (i == id - 1)
                _GUIs[i].gameObject.SetActive(!_GUIs[i].isActiveAndEnabled);
        }
    }
}
