using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField] TMP_Text _playerIdText;
    [SerializeField] TMP_Text _throttleText;
    [Header("Drone health")]
    [SerializeField] TMP_Text _FRStateText;
    [SerializeField] TMP_Text _FLStateText;
    [SerializeField] TMP_Text _BRStateText;
    [SerializeField] TMP_Text _BLStateText;
    [SerializeField] TMP_Text _PIDpitch;
    [SerializeField] TMP_Text _PIDyaw;
    [SerializeField] TMP_Text _PIDroll;
    [SerializeField] TMP_Text _PIDHeight;
    [SerializeField] TMP_Text _scoreText;
    public QuadcopterController Copter;
    private int _playerId = 0;
    public void Initialize(int playerId)
    {
        _playerId = playerId;
        _playerIdText.text = "Player " + _playerId;
    }
    private void Update()
    {
        UpdateStats();
    }
    public void UpdateStats()
    {
        _FRStateText.text = "FR\n" + Copter.propellerFR.health;
        _FLStateText.text = "FL\n" + Copter.propellerFL.health;
        _BRStateText.text = "BR\n" + Copter.propellerBR.health;
        _BLStateText.text = "BL\n" + Copter.propellerBL.health;
        _PIDpitch.text = "P" + Copter.PID_pitch_gains.ToString("F1");
        _PIDyaw.text = "Y" + Copter.PID_yaw_gains.ToString("F1");
        _PIDroll.text = "R" + Copter.PID_roll_gains.ToString("F1");
        _PIDHeight.text = "T" + Copter.PID_throttleGains.ToString("F1");
        _throttleText.text = "Throttle: " + Copter.throttle.ToString("F1");
        _scoreText.text = "Score: " + Copter.Score.ToString();
    }
}
