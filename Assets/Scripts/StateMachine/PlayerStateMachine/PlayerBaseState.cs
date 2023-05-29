using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    private const float AnimatorDampTime = 0.1f;
    private const float InputMagnitude = 1f;
    private const float Threshold = 0.01f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private bool _rotateOnMove = true;


    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero,deltaTime);
    }

    protected void Moving(float deltaTime)
    {
        float targetSpeed = stateMachine.InputReader.SprintValue ? stateMachine.FreeLookSprintingSpeed : stateMachine.FreeLookMovementSpeed;

        if (stateMachine.InputReader.MovementValue == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(stateMachine.Controller.velocity.x, 0.0f, stateMachine.Controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {

            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * InputMagnitude,
                Time.deltaTime * stateMachine.SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * stateMachine.SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(stateMachine.InputReader.MovementValue.x, 0.0f, stateMachine.InputReader.MovementValue.y).normalized;

        if (stateMachine.InputReader.MovementValue != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              stateMachine.MainCameraTransform.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(stateMachine.PlayerTransform.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                stateMachine.RotationDamping);

            if (_rotateOnMove)
            {
                stateMachine.PlayerTransform.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        stateMachine.Controller.Move((targetDirection.normalized + stateMachine.ForceReceiver.Movement) * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    protected void CameraRotation()
    {
        if (stateMachine.InputReader.LookValue.sqrMagnitude >= Threshold)
        {
            //float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            float deltaTimeMultiplier = 2.0f;

            _cinemachineTargetYaw += stateMachine.InputReader.LookValue.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += stateMachine.InputReader.LookValue.y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, stateMachine.BottomClamp, stateMachine.TopClamp);

        stateMachine.CameraFocus.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + stateMachine.CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    protected void SetRotateOnMove(bool newRotateOnMove)
    {
        _rotateOnMove = newRotateOnMove;
        
    }

  

    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
