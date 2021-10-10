using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public static int selectedWeapon = 0;

    private float currentTime;
    public static bool weaponHidden;

    public GameObject normalCrosshair;
    public GameObject enemyCrosshair;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (weaponHidden)
        {
            normalCrosshair.SetActive(true);
            enemyCrosshair.SetActive(false);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            weaponHidden = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
            weaponHidden = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
            weaponHidden = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
            weaponHidden = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            selectedWeapon = 4;
            weaponHidden = false;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        if (Input.GetKey(KeyCode.R))
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 0.5f)
            {
                transform.GetChild(selectedWeapon).gameObject.SetActive(false);
                weaponHidden = true;
            }
        }
        else
        {
            currentTime = 0f;

            // Reload
            // We reload in Gun.cs
        }

        if (Input.GetMouseButtonDown(0) && weaponHidden)
        {
            transform.GetChild(selectedWeapon).gameObject.SetActive(true);
            weaponHidden = false;
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }
}