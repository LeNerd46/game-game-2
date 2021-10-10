using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    public float throwForce = 40f;
    public float upAmount;

    public GameObject grenadePrefab;

    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);

            grenade.GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x, transform.forward.y + upAmount, transform.forward.z) * throwForce, ForceMode.VelocityChange);
        }
    }
}
