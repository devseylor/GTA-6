using System;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    private readonly int AimingBlendTreeHash = Animator.StringToHash("AimingBlendTree");
    private readonly int AimingForwardSpeedHash = Animator.StringToHash("AimingForwardSpeed");
    private readonly int AimingRightSpeedHash = Animator.StringToHash("AimingRightSpeed");


    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
        stateMachine.InputReader.AimEvent += OnAim;
        stateMachine.Animator.CrossFadeInFixedTime(AimingBlendTreeHash, CrossFadeDuration);
        SetRotateOnMove(false);
        stateMachine.Crosshair.SetActive(true);
    }
    public override void Tick(float deltaTime)
    {
        Vector3 mousePosition = MousePosition();
        Moving(deltaTime);

        UpdateAnimator(deltaTime);
        RotatePlayer(mousePosition,deltaTime);
        if (stateMachine.InputReader.IsShooting)
        {
            stateMachine.SpawnBullet(mousePosition);
        }
    }

    private void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(AimingForwardSpeedHash, 0);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(AimingForwardSpeedHash, value);
        }
        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(AimingRightSpeedHash, 0);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(AimingRightSpeedHash, value);
        }
    }

    public override void LateTick()
    {
        CameraRotation();
    }

    public override void Exit()
    {
        stateMachine.InputReader.AimEvent -= OnAim;
        SetRotateOnMove(true);
        stateMachine.Crosshair.SetActive(false);
    }

    private void OnAim()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 MousePosition()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray,out RaycastHit raycastHit, 999, stateMachine.AimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        return mouseWorldPosition;
    }

    private void RotatePlayer(Vector3 mousePosition, float deltaTime)
    {
        Vector3 worldAimTarget = mousePosition;
        worldAimTarget.y = stateMachine.PlayerTransform.position.y;
        Vector3 aimDirection = (worldAimTarget - stateMachine.PlayerTransform.position).normalized;
        stateMachine.PlayerTransform.transform.forward = Vector3.Lerp(stateMachine.PlayerTransform.transform.forward, aimDirection, deltaTime * 20f);
    }
}
