using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaypointMarker : MonoBehaviour
{
    public Transform target;
    public Image image;
    public TextMeshProUGUI distanceText;

    public Vector3 offset;
    
    void Update()
    {
        if (image == null)
            Destroy(this);

        Vector2 position = Camera.main.WorldToScreenPoint(target.position + offset);

        distanceText.text = ((int)Vector3.Distance(transform.position, target.position)).ToString() + "m";

        float minX = image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        if(Vector3.Dot(target.position - transform.position, transform.forward) < 0)
        {
            // Waypoint is behind the player

            if (position.x < Screen.width / 2)
                position.x = maxX;
            else
                position.x = minX;
        }

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        image.transform.position = position;

        if(Vector3.Distance(target.position, transform.position) < 3)
        {
            if (GameManager.instance != null)
                GameManager.instance.onLocationEntered.Invoke(image);
        }
    }
}
