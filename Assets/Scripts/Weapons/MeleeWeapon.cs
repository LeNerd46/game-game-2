using UnityEngine;
using TMPro;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 5f;
    public Camera cam;

    public TextMeshProUGUI text;
    public GameObject normalCrosshair;
    public GameObject enemyCrosshair;

    public AudioClip swordSwing;

    private Animator animator;

    void Start()
    {
        // animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                normalCrosshair.SetActive(false);
                enemyCrosshair.SetActive(true);
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
    }

    void Attack()
    {
        // animator.Play("SwordSwing");
        AudioSource.PlayClipAtPoint(swordSwing, transform.position);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name);

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
        {
            Debug.Log("Not a target");
        }
    }
}
