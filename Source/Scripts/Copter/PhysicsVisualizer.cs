using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuadcopterController))]
public class PhysicsVisualizer : MonoBehaviour
{
    [SerializeField] LineRenderer _FR;
    [SerializeField] LineRenderer _FL;
    [SerializeField] LineRenderer _BR;
    [SerializeField] LineRenderer _BL;
    [SerializeField] float _factor = 0.1f;
    QuadcopterController _quadcopter;
    public bool EnableVisualisation;
    private void Awake()
    {
        _quadcopter = GetComponent<QuadcopterController>();
    }
    private void FixedUpdate()
    {
        Visualize(
            _quadcopter.propellerForceFR,
            _quadcopter.propellerForceFL,
            _quadcopter.propellerForceBL,
            _quadcopter.propellerForceBR);

        _FR.gameObject.SetActive(EnableVisualisation);
        _FL.gameObject.SetActive(EnableVisualisation);
        _BL.gameObject.SetActive(EnableVisualisation);
        _BR.gameObject.SetActive(EnableVisualisation);
    }
    public void Visualize(float fr, float fl, float bl, float br)
    {
        _FR.SetPosition(1, new Vector3(0, fr * _factor, 0));
        _FL.SetPosition(1, new Vector3(0, fl * _factor, 0));
        _BL.SetPosition(1, new Vector3(0, bl * _factor, 0));
        _BR.SetPosition(1, new Vector3(0, br * _factor, 0));
    }
}
