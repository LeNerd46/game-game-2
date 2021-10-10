using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public GameObject prefab;

    public MeshRenderer meshRenderer;
    public Material canBuild;
    public Material cantBuild;

    private BuildingSystem buildingSystem;
    public List<string> tags = new List<string>();

    public bool isSnapped;
    public bool isFoundation;

    void Start()
    {
        buildingSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<BuildingSystem>();
    }

    public void Place()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ChangeColor()
    {
        if (isSnapped)
            meshRenderer.material = canBuild;
        else
            meshRenderer.material = cantBuild;

        if(isFoundation)
        {
            meshRenderer.material = canBuild;
            isSnapped = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if(other.tag.Equals(tags[i]))
            {
                buildingSystem.Pause();
                transform.position = other.transform.position;
                isSnapped = true;
                ChangeColor();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if(other.tag.Equals(tags[i]))
            {
                isSnapped = false;
                ChangeColor();
            }
        }
    }
}
