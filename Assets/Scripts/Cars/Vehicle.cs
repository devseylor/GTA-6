using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private WheelInfo frontRight;
    [SerializeField] private WheelInfo backRight;
    [SerializeField] private WheelInfo frontLeft;
    [SerializeField] private WheelInfo backLeft;

    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float breakingForce = 300f;
    [SerializeField] private float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;
    private float verticalDirection = 0f;
    private float horizontalDirection = 0f;

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * verticalDirection;
        currentBreakingForce = Input.GetKey(KeyCode.Space) ? breakingForce : 0f;
        currentTurnAngle = maxTurnAngle * horizontalDirection;

        ApplyMotorTorque(frontRight, currentAcceleration);
        ApplyMotorTorque(frontLeft, currentAcceleration);

        ApplyBrakeTorque(frontRight, currentBreakingForce);
        ApplyBrakeTorque(backLeft, currentBreakingForce);
        ApplyBrakeTorque(frontLeft, currentBreakingForce);
        ApplyBrakeTorque(backRight, currentBreakingForce);

        ApplySteerAngle(frontLeft, currentTurnAngle);
        ApplySteerAngle(frontRight, currentTurnAngle);

        UpdateWheel(frontLeft);
        UpdateWheel(frontRight);
        UpdateWheel(backLeft);
        UpdateWheel(backRight);
    }

    public void PlayerInputSetY(float movementValueY)
    {
        verticalDirection = movementValueY;
    }

    public void PlayerInputSetX(float movementValueX)
    {
        horizontalDirection = movementValueX;
    }

    private void ApplyMotorTorque(WheelInfo wheel, float torque)
    {
        wheel.collider.motorTorque = torque;
    }

    private void ApplyBrakeTorque(WheelInfo wheel, float torque)
    {
        wheel.collider.brakeTorque = torque;
    }

    private void ApplySteerAngle(WheelInfo wheel, float angle)
    {
        wheel.collider.steerAngle = angle;
    }

    private void UpdateWheel(WheelInfo wheel)
    {
        wheel.collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheel.transform.position = position;
        wheel.transform.rotation = rotation;
    }
}