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
    [SerializeField] TMP_Text _scoreText;
    public QuadcopterController Copter;
    private CopterScore _score;
    private int _playerId = 0;
    public void Initialize(int playerId)
    {
        _playerId = playerId;
        if(_playerIdText)
        _playerIdText.text = "Player " + _playerId;
        _score = Copter.GetComponent<CopterScore>();
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
        _throttleText.text = "THROTTLE: " + Copter.Throttle.ToString("F1");
        _scoreText.text = _score.Score.ToString();
    }
}
