using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEntrance : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;

    public event Action<CarEntrance> OnDestroyed;

    private void Start()
    {
        vehicle = transform.parent.GetComponent<Vehicle>();
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public Vehicle GetVehicle()
    {
        return vehicle;
    }
}
