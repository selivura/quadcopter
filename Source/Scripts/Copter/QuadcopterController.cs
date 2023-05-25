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

    public float propellerForceFR;
    public float propellerForceFL;
    public float propellerForceBR;
    public float propellerForceBL;
    //Quadcopter parameters
    [Header("Internal")]
    public float maxPropellerForce = 100; //100
    public float maxTorque = 1; //1
    private float _throttle = 0;

    public float moveFactor = 5; //5
    //PID
    public Vector3 PID_pitch_gains = new Vector3(2,3,2); //(2, 3, 2)
    public Vector3 PID_roll_gains = new Vector3(2, 0.2f, .5f); //(2, 0.2, 0.5)
    public Vector3 PID_yaw_gains = new Vector3(1, 0, 0); //(1, 0, 0)
    public Vector3 PID_throttleGains = new Vector3(1, 0, 0); //(1, 0, 0)
    [SerializeField] private float _maxThorttle = 200;

    Rigidbody _quadcopterRB;

    //The PID controllers
    private PIDController PID_pitch;
    private PIDController PID_roll;
    private PIDController PID_yaw;
    private PIDController PID_throttle;
    //Movement factors
    float _pitch;
    float _roll;
    float _yawDir;
    float _targetHeight;
    bool _dirtyPos = false;

    public float Pitch { get => _pitch; set => _pitch = value; }
    public float Roll { get => _roll; set => _roll = value; }
    public float YawDir { get => _yawDir; set => _yawDir = value; }
    public float TargetHeight { get => _targetHeight; set => _targetHeight = value; }
    public float Throttle { get => _throttle; set => _throttle = value; }

    public Action onDeath;
    bool _reset = false;
   
    void Awake()
    {
        _quadcopterRB = GetComponent<Rigidbody>();
        PID_pitch = new PIDController();
        PID_roll = new PIDController();
        PID_yaw = new PIDController();
        PID_throttle = new PIDController();
        ResetDrone(false);
    }

    public void ResetDrone(bool countAsDeath = true)
    {
        _quadcopterRB.velocity = Vector3.zero;
        _quadcopterRB.angularVelocity = Vector3.zero;
        propellerBL.health = 100;
        propellerBR.health = 100;
        propellerFL.health = 100;
        propellerFR.health = 100;
        SetTargetHeight(transform.position.y);
         _pitch = 0;
        _yawDir = 0;
        _roll = 0;
        _reset = true;
        if (countAsDeath)
            onDeath?.Invoke();
    }

    void FixedUpdate()
    {
        AddMotorForce();
    }
    public void SetTargetHeight(float value)
    {
        if (Mathf.Abs(value) > 0)
        {
            TargetHeight += value;
            _dirtyPos = true;
        }
        else if (_dirtyPos)
        {
            TargetHeight = transform.position.y;
            _dirtyPos = false;
        }
        _throttle += PID_throttle.GetFactorFromPIDController(PID_throttleGains, GetHeightError());
        _throttle = Mathf.Clamp(_throttle, 0, _maxThorttle);
    }

    void AddMotorForce()
    {
        if (_reset)
        {
            _reset = false;
            return;
        }
        //Pitch
        float pitchError = GetPitchError();

        //Roll
        float rollError = GetRollError() * -1f;

        Vector3 PID_pitch_gains_adapted = _throttle > 100f ? PID_pitch_gains * 2f : PID_pitch_gains;

        float PID_pitch_output = PID_pitch.GetFactorFromPIDController(PID_pitch_gains_adapted, pitchError);
        float PID_roll_output = PID_roll.GetFactorFromPIDController(PID_roll_gains, rollError);

        //FR
        propellerForceFR = _throttle + (PID_pitch_output + PID_roll_output);

        //Add steering
        propellerForceFR -= Pitch * _throttle * moveFactor;
        propellerForceFR -= Roll * _throttle;
        propellerForceFR *= propellerFR.health / 100;

        //FL
        propellerForceFL = _throttle + (PID_pitch_output - PID_roll_output);

        propellerForceFL -= Pitch * _throttle * moveFactor;
        propellerForceFL += Roll * _throttle;
        propellerForceFL *= propellerFL.health / 100;

        //BR
        propellerForceBR = _throttle + (-PID_pitch_output + PID_roll_output);

        propellerForceBR += Pitch * _throttle * moveFactor;
        propellerForceBR -= Roll * _throttle;
        propellerForceBR *= propellerBR.health / 100;

        //BL 
        propellerForceBL = _throttle + (-PID_pitch_output - PID_roll_output);

        propellerForceBL += Pitch * _throttle * moveFactor;
        propellerForceBL += Roll * _throttle;
        propellerForceBL *= propellerBL.health / 100;
        
        //Clamp
        propellerForceFR = Mathf.Clamp(propellerForceFR, 0f, maxPropellerForce);
        propellerForceFL = Mathf.Clamp(propellerForceFL, 0f, maxPropellerForce);
        propellerForceBR = Mathf.Clamp(propellerForceBR, 0f, maxPropellerForce);
        propellerForceBL = Mathf.Clamp(propellerForceBL, 0f, maxPropellerForce);


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
        _quadcopterRB.AddTorque(transform.up * YawDir * maxTorque * _throttle * propHealthFactor);
        _quadcopterRB.AddTorque(transform.up * _throttle * PID_yaw_output * -1f);
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
        float height = TargetHeight - transform.position.y;
        return height;
    }

    //Wrap between 0 and 360 degrees
    float WrapAngle(float inputAngle)
    {
        return inputAngle % 360f;
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