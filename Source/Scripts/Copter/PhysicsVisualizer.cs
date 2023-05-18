using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsVisualizer : MonoBehaviour
{
    [SerializeField] LineRenderer _FR;
    [SerializeField] LineRenderer _FL;
    [SerializeField] LineRenderer _BR;
    [SerializeField] LineRenderer _BL;
    [SerializeField] float _factor = 0.1f;
    public void Visualize(float fr, float fl, float bl, float br, bool enabled)
    {
        _FR.SetPosition(1, new Vector3(0, fr * _factor, 0));
        _FL.SetPosition(1, new Vector3(0, fl * _factor, 0));
        _BL.SetPosition(1, new Vector3(0, bl * _factor, 0));
        _BR.SetPosition(1, new Vector3(0, br * _factor, 0));
        _FR.gameObject.SetActive(enabled);
        _FL.gameObject.SetActive(enabled);
        _BL.gameObject.SetActive(enabled);
        _BR.gameObject.SetActive(enabled);
    }
}
