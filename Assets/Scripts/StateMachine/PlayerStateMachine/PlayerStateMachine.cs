using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Camera MainCameraTransform { get; private set; }
    [field: SerializeField] public CarInteraction CarInteraction { get; private set; }
    [field: SerializeField] public GameObject CameraFocus { get; set; }
    [field: SerializeField] public GameObject debugTransform { get; set; }
    [field: SerializeField] public GameObject ProjectilePrefab { get; set; }
    [field: SerializeField] public Transform BulleSpawnPosition { get; set; }
    [field: SerializeField] public GameObject Crosshair { get; set; }
    [field: SerializeField] public LayerMask AimColliderLayerMask { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float FreeLookWalkingSpeed { get; private set; }
    [field: SerializeField] public float FreeLookSprintingSpeed { get; private set; }
    [field: SerializeField] public float SpeedChangeRate { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float TopClamp { get; private set; }
    [field: SerializeField] public float BottomClamp { get; private set; }
    [field: SerializeField] public float CameraAngleOverride { get; private set; }
    [field: SerializeField] public MeleeAttack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage[] MeleeWeapons { get; private set; }



    public Transform PlayerTransform { get; private set; }
    public float CinemachineTargetYaw { get; private set; }

    private void Start()
    {
        PlayerTransform = transform;
        CinemachineTargetYaw = CameraFocus.transform.rotation.eulerAngles.y;

        SwitchState(new PlayerFreeLookState(this));
    }

    public void SpawnBullet(Vector3 mousePosition)
    {
        Vector3 aimDir = (mousePosition - BulleSpawnPosition.position).normalized;
        Instantiate(ProjectilePrefab, BulleSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
