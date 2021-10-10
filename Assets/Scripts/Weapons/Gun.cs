using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    [Tooltip("How long you have to wait to shoot")]
    public float fireRate = 10f;
    public float noise;
    [Tooltip("Sets the amount of time it takes to fire a bullet for auto fire guns")]
    public float nextTimeToFire;

    public int maxAmmo;
    private int currentAmmo;
    public float reloadTime;
    public float zoomFOV = 40;

    private bool isReloading;
    private bool isZooming;
    public static bool lookingAtEnemy;
    public bool autoFire;
    private bool canShoot = true;
    private bool shot;

    public TextMeshProUGUI text;
    public Camera cam;
    public GameObject normalCrosshair;
    public GameObject enemyCrosshair;
    public Animator animator;

    private float normalFOV;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        text.gameObject.SetActive(true);
    }

    void Update()
    {
        text.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();

        if (isReloading)
            return;

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("IsZoomedIn", true);
            StartCoroutine(WaitToZoom());
        }
        else if (Input.GetMouseButtonUp(1) )
        {
            animator.SetBool("IsZoomedIn", false);
            cam.fieldOfView = normalFOV;
        }

        if (!Input.GetMouseButton(1))
        {
            cam.fieldOfView = 60;
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                normalCrosshair.SetActive(false);
                enemyCrosshair.SetActive(true);

                lookingAtEnemy = true;
            }
            else
            {
                normalCrosshair.SetActive(true);
                enemyCrosshair.SetActive(false);
            }
        }
        else
        {
            normalCrosshair.SetActive(true);
            enemyCrosshair.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Reload());

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && !WeaponSwitching.weaponHidden && autoFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetMouseButtonDown(0) && !WeaponSwitching.weaponHidden && !autoFire && canShoot && !PauseMenu.paused)
        {
            Shoot();

            StartCoroutine(WaitToShoot());
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Not a target");
            }
        }
        else
            Debug.Log("Not a target");
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    IEnumerator WaitToShoot()
    {
        canShoot = false;

        yield return new WaitForSeconds(fireRate);

        canShoot = true;
    }

    IEnumerator WaitToZoom()
    {
        isZooming = true;

        yield return new WaitForSeconds(0.13f);

        normalFOV = cam.fieldOfView;
        cam.fieldOfView = zoomFOV;

        isZooming = false;
    }
}