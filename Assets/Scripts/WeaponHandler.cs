using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponsLogic;

    public void EnableWeapon()
    {
        foreach (var weapon in weaponsLogic)
        {
            weapon.SetActive(true);
        }
    }

    public void DisableWeapon()
    {
        foreach (var weapon in weaponsLogic)
        {
            weapon.SetActive(false);
        }
    }
}
