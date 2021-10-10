using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Transform sun;

    void Update()
    {
        sun.rotation = Quaternion.Euler(sun.rotation.x + 0.5f, sun.rotation.y, sun.rotation.z);
    }
}
