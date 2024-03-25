using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField]
    private List<PlayerWeapon> weapons;

    private PlayerWeapon leftHand;
    private PlayerWeapon rightHand;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            weapons[i].Disable();
            weapons[i].SetPlayer(gameObject);
            var weaponGameObject = Instantiate(weapons[i], transform.position, Quaternion.identity);
            weaponGameObject.transform.parent = transform;
            weapons[i] = (PlayerWeapon)weaponGameObject.GetComponent("PlayerWeapon");
        }

        if (weapons.Count >= 1)
        {
            leftHand = weapons[0];
        }
        if (weapons.Count >= 2)
        {
            rightHand = weapons[1];
        }

        UpdateWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            leftHand.Reload();
            rightHand.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapLeftRightHand();
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                RotateWeapons(weapons[i]);
            }
        }
    }

    private void UpdateWeapons()
    {
        leftHand.Enable("Fire1");
        rightHand.Enable("Fire2");
    }

    private void RotateWeapons(PlayerWeapon weapon)
    {
        if (weapon == leftHand)
        {
            return;
        }
        SwapLeftRightHand();
        ChangeLeftHand(weapon);
    }

    private void ChangeLeftHand(PlayerWeapon weapon)
    {
        if (weapon == leftHand)
        {
            return;
        }
        if (weapon == rightHand)
        {
            SwapLeftRightHand();
            return;
        }

        leftHand.Disable();
        leftHand = weapon;
        UpdateWeapons();
    }

    private void ChangeRightHand(PlayerWeapon weapon)
    {
        if (weapon == rightHand)
        {
            return;
        }
        if (weapon == leftHand)
        {
            SwapLeftRightHand();
            return;
        }

        rightHand.Disable();
        rightHand = weapon;
        UpdateWeapons();
    }

    private void SwapLeftRightHand()
    {
        PlayerWeapon weapon = leftHand;
        leftHand = rightHand;
        rightHand = weapon;
        UpdateWeapons();
    }
}
