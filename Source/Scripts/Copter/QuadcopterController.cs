using UnityEngine;
using System;

public class QuadcopterController : MonoBehaviour
{
    public int PlayerID = -1;
    //propeller
    public Propeller propellerFR;
    public Propeller propellerFL;
    public Propeller propellerBL;
    public Propeller propellerBR;
    public DroneInputs Inputs;
    //Quadcopter parameters
    [Header("Internal")]
    public float maxPropellerForce = 100; //100
    public float maxTorque = 1; //1
    public float throttle = 0;
    public int Score;
    public int RestartAmount;

    public float moveFactor = 5; //5
    //PID
    public Vector3 PID_pitch_gains = new Vector3(2,3,2); //(2, 3, 2)
    public Vector3 PID_roll_gains = new Vector3(2, 0.2f, .5f); //(2, 0.2, 0.5)
    public Vector3 PID_yaw_gains = new Vector3(1, 0, 0); //(1, 0, 0)
    public Vector3 PID_throttleGains = new Vector3(1, 0, 0); //(1, 0, 0)
    [SerializeField] private float _maxThorttle = 200;

    //External parameters
    [Header("External")]
    public float windForce;
    //0 -> 360
    public float forceDir;

    Rigidbody _quadcopterRB;

    //The PID controllers
    private PIDController PID_pitch;
    private PIDController PID_roll;
    private PIDController PID_yaw;
    private PIDController PID_throttle;
    [SerializeField] private PhysicsVisualizer _visualizer;
    //Movement factors
    float _pitch;
    float _roll;
    float _yawDir;
    float _targetHeight;
    bool _dirtyPos = false;
    public Transform lastSpawnpoint;
    public Transform NextCheckpoint;
    private Map _map ;
    public void PassCheckpoint(int value)
    {
        Score += value; 
        NextCheckpoint = _map.GetNextCheckpoint(this);
    }
    void Awake()
    {
        _visualizer = GetComponent<PhysicsVisualizer>();
        _quadcopterRB = GetComponent<Rigidbody>();
        PID_pitch = new PIDController();
        PID_roll = new PIDController();
        PID_yaw = new PIDController();
        PID_throttle = new PIDController();
        Inputs.onReset += () => ResetDrone();
        _map = FindObjectOfType<Map>();
        lastSpawnpoint = FindAnyObjectByType<PlayerSpawner>().GetSpawnPoint();
        if (_map)
            NextCheckpoint = _map.GetNextCheckpoint(this);
        ResetDrone(false);
    }

    private void ResetDrone(bool countAsDeath = true)
    {
        transform.position = lastSpawnpoint.position;
        transform.rotation = lastSpawnpoint.rotation;
        _quadcopterRB.velocity = Vector3.zero;
        _quadcopterRB.angularVelocity = Vector3.zero;
        propellerBL.health = 100;
        propellerBR.health = 100;
        propellerFL.health = 100;
        propellerFR.health = 100;
        _targetHeight = transform.position.y;
        if(countAsDeath)
        RestartAmount++;
    }

    void FixedUpdate()
    {
        AddControls();
        AddMotorForce();
    }

    void AddControls()
    {
        if (Mathf.Abs(Inputs.Throttle) > 0)
        {
            _targetHeight += Inputs.Throttle;
            _dirtyPos = true;
        }
        else if (_dirtyPos)
        {
            _targetHeight = transform.position.y;
            _dirtyPos = false;
        }
        throttle += PID_throttle.GetFactorFromPIDController(PID_throttleGains, GetHeightError());
        throttle = Mathf.Clamp(throttle, 0, _maxThorttle);

        _pitch = 0f;
        _pitch = Inputs.Pitch;

        _roll = 0f;
        _roll = Inputs.Roll;

        _yawDir = 0f;
        _yawDir = Inputs.Yaw;
    }
    void AddMotorForce()
    {
        //Pitch
        float pitchError = GetPitchError();

        //Roll
        float rollError = GetRollError() * -1f;

        Vector3 PID_pitch_gains_adapted = throttle > 100f ? PID_pitch_gains * 2f : PID_pitch_gains;

        float PID_pitch_output = PID_pitch.GetFactorFromPIDController(PID_pitch_gains_adapted, pitchError);
        float PID_roll_output = PID_roll.GetFactorFromPIDController(PID_roll_gains, rollError);

        //FR
        float propellerForceFR = throttle + (PID_pitch_output + PID_roll_output);

        //Add steering
        propellerForceFR -= _pitch * throttle * moveFactor;
        propellerForceFR -= _roll * throttle;
        propellerForceFR *= propellerFR.health / 100;

        //FL
        float propellerForceFL = throttle + (PID_pitch_output - PID_roll_output);

        propellerForceFL -= _pitch * throttle * moveFactor;
        propellerForceFL += _roll * throttle;
        propellerForceFL *= propellerFL.health / 100;

        //BR
        float propellerForceBR = throttle + (-PID_pitch_output + PID_roll_output);

        propellerForceBR += _pitch * throttle * moveFactor;
        propellerForceBR -= _roll * throttle;
        propellerForceBR *= propellerBR.health / 100;

        //BL 
        float propellerForceBL = throttle + (-PID_pitch_output - PID_roll_output);

        propellerForceBL += _pitch * throttle * moveFactor;
        propellerForceBL += _roll * throttle;
        propellerForceBL *= propellerBL.health / 100;
        
        //Clamp
        propellerForceFR = Mathf.Clamp(propellerForceFR, 0f, maxPropellerForce);
        propellerForceFL = Mathf.Clamp(propellerForceFL, 0f, maxPropellerForce);
        propellerForceBR = Mathf.Clamp(propellerForceBR, 0f, maxPropellerForce);
        propellerForceBL = Mathf.Clamp(propellerForceBL, 0f, maxPropellerForce);

        _visualizer.Visualize(propellerForceFR, propellerForceFL, propellerForceBL, propellerForceBR, Inputs.EnbleVisual);

        propellerFR.RotationSpeed = propellerForceFR;
        propellerFL.RotationSpeed = propellerForceFL;
        propellerBR.RotationSpeed = propellerForceBR;
        propellerBL.RotationSpeed = propellerForceBL;
        AddForceToPropeller(propellerFR.gameObject, propellerForceFR);
        AddForceToPropeller(propellerFL.gameObject, propellerForceFL);
        AddForceToPropeller(propellerBR.gameObject, propellerForceBR);
        AddForceToPropeller(propellerBL.gameObject, propellerForceBL);

        float yawError = _quadcopterRB.angularVelocity.y;
        float propHealthFactor = ((propellerFL.health + propellerBL.health + propellerFR.health + propellerBR.health) / 4) / 100;
        float PID_yaw_output = PID_yaw.GetFactorFromPIDController(PID_yaw_gains, yawError) * propHealthFactor;
        _quadcopterRB.AddTorque(transform.up * _yawDir * maxTorque * throttle * propHealthFactor);
        _quadcopterRB.AddTorque(transform.up * throttle * PID_yaw_output * -1f);
    }

    void AddForceToPropeller(GameObject propellerObj, float propellerForce)
    {
        Vector3 propellerUp = propellerObj.transform.up;
        Vector3 propellerPos = propellerObj.transform.position;
        _quadcopterRB.AddForceAtPosition(propellerUp * propellerForce, propellerPos);

        //Debug.DrawRay(propellerPos, propellerUp * 1f, Color.red);
    }

    private float GetPitchError()
    {
        float xAngle = transform.eulerAngles.x;

        xAngle = WrapAngle(xAngle);

        if (xAngle > 180f && xAngle < 360f)
        {
            xAngle = 360f - xAngle;

            xAngle *= -1f;
        }

        return xAngle;
    }
    private float GetRollError()
    {
        float zAngle = transform.eulerAngles.z;

        zAngle = WrapAngle(zAngle);

        if (zAngle > 180f && zAngle < 360f)
        {
            zAngle = 360f - zAngle;

            zAngle *= -1f;
        }

        return zAngle;
    }

    private float GetHeightError()
    {
        float height = _targetHeight - transform.position.y;
        return height;
    }

    //Wrap between 0 and 360 degrees
    float WrapAngle(float inputAngle)
    {
        return ((inputAngle % 360f) + 360f) % 360f;
    }
}
public class PIDController
{
    float _oldError = 0f;
    float _errorSum = 0f;

    public float _gainP = 0f;
    public float _gainI = 0f;
    public float _gainD = 0f;
    private float _errorSumMax = 20f;

    public float GetFactorFromPIDController(float error)
    {
        float output = CalculatePIDOutput(error);

        return output;
    }

    public float GetFactorFromPIDController(float gain_P, float gain_I, float gain_D, float error)
    {
        _gainP = gain_P;
        _gainI = gain_I;
        _gainD = gain_D;

        float output = CalculatePIDOutput(error);

        return output;
    }

    public float GetFactorFromPIDController(Vector3 gains, float error)
    {
        _gainP = gains.x;
        _gainI = gains.y;
        _gainD = gains.z;

        float output = CalculatePIDOutput(error);

        return output;
    }

    private float CalculatePIDOutput(float error)
    {
        float output = 0f;
        //P
        output += _gainP * error;
        //I
        _errorSum += Time.fixedDeltaTime * error;
        _errorSum = Mathf.Clamp(_errorSum, -_errorSumMax, _errorSumMax);
        output += _gainI * _errorSum;
        float d_dt_error = (error - _oldError) / Time.fixedDeltaTime;
        _oldError = error;
        output += _gainD * d_dt_error;
        return output;
    }
}