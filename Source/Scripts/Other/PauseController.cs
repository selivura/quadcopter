using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    static bool _paused;
    public static void Pause(bool value = true)
    {
        if(value)
            Time.timeScale = 0;
        if (!value)
            Time.timeScale = 1;

            _paused = !_paused;
    }
}
