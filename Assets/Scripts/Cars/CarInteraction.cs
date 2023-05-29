using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private Camera mainCamera;

    public CarEntrance carEntrance { get; private set; }
    public event Action<CarEntrance> OnEnter;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<CarEntrance>(out CarEntrance car)) { return; }

        carEntrance = car;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<CarEntrance>(out CarEntrance car)) { return; }


        carEntrance = null;
    }
}
