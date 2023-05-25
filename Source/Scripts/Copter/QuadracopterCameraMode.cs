using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class QuadracopterCameraMode : MonoBehaviour
{
    QuadcopterController _controller;
    [SerializeField] Camera _cam;
    [SerializeField] CinemachineVirtualCamera _vcam;
    [SerializeField] int _layer1;
    [SerializeField] int _layer2;
    [SerializeField] LayerMask _cullingMaskCamera1;
    [SerializeField] LayerMask _cullingMaskCamera2;
    [SerializeField] Transform _fpvPos;
    [SerializeField] Transform _tpsPos;
    private bool _fpv = false;
    private void Start()
    {
        _controller = GetComponent<QuadcopterController>();
        _vcam.transform.SetParent(null, false);
        _cam.transform.SetParent(null, false);
        if (_controller.PlayerID == 1)
        {
            _vcam.gameObject.layer = _layer1;
            _cam.cullingMask = _cullingMaskCamera1;
        }
        else
        {
            _vcam.gameObject.layer = _layer2;
            _cam.cullingMask = _cullingMaskCamera2;
        }
    }
    private void Update()
    {
        //if(_cam.Follow != transform || _cam.LookAt != transform)
        //{
        //    _cam.Follow = transform;
        //    _cam.LookAt = transform;
        //}
    }
    public void OnCameraChangeMode()
    {
        ChangeFPV();
    }
    private void ChangeFPV()
    {
        if(_fpv)
        {
            _fpv = false;
            _vcam.enabled = !_fpv;
            _cam.transform.SetParent(null, false);
            CamReset();
        }
        else
        {
            _fpv = true;
            _vcam.enabled = !_fpv;
            _cam.transform.SetParent(_fpvPos, false);
            CamReset();
        }
    }
    private void OnDestroy()
    {
        Destroy(_cam.gameObject);
        Destroy(_vcam.gameObject);
    }
    private void CamReset()
    {
        _cam.transform.localPosition = Vector3.zero;
        _cam.transform.localRotation = new Quaternion();

    }
}
