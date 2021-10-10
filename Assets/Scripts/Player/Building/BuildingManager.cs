using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject foundation;

    public BuildingSystem buildingSystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            buildingSystem.BuildNew(foundation);
            Debug.Log("Buliding");
        }
    }
}
