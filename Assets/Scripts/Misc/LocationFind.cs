using UnityEngine;
using TMPro;
using System.Collections;

public class LocationFind : MonoBehaviour
{
    public string locationName;
    public float radius;

    public TextMeshProUGUI locationText;
    public LayerMask playerMask;

    private bool locationFound;

    void Update()
    {
        locationFound = Physics.CheckSphere(transform.position, radius, playerMask);

        if (locationFound)
        {
            locationText.text = $"{locationName} Found";
            locationText.gameObject.GetComponent<Animator>().Play("LocationFound");

            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
