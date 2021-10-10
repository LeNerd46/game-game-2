using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public Camera cam;
    public LayerMask mask;

    public float stickTolorance = 1.5f;

    public bool isBuilding;
    private bool paused;

    private GameObject previewObject;
    private PreviewObject preview;

    void Update()
    {
        // Build
        if(Input.GetMouseButtonDown(0) && isBuilding)
        {
            if (preview.isSnapped)
                Build();
            else
                Debug.Log("Not snapped");
        }

        // Cancel
        if (Input.GetKeyDown(KeyCode.C))
            CancelBuilding();

        // Rotate
        if(Input.GetKeyDown(KeyCode.T))
        {
            previewObject.transform.Rotate(0f, 90f, 0f);
        }

        if(isBuilding)
        {
            if (paused)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (Mathf.Abs(mouseX) >= stickTolorance || Mathf.Abs(mouseY) >= stickTolorance)
                    paused = false;
            }
            else
                BuildRay();
        }
    }

    /// <summary>
    /// Build a new object
    /// </summary>
    /// <param name="gameObject">Object to build</param>
    public void BuildNew(GameObject gameObject)
    {
        previewObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        preview = previewObject.GetComponent<PreviewObject>();
        isBuilding = true;
    }

    public void CancelBuilding()
    {
        Destroy(gameObject);
        previewObject = null;
        preview = null;
        isBuilding = false;
    }

    public void Build()
    {
        preview.Place();
        previewObject = null;
        preview = null;
        isBuilding = false;
    }

    public void Pause()
    {
        paused = !paused;
    }

    public void BuildRay()
    {
        if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, mask))
        {
            float y = hit.point.y + (previewObject.transform.localScale.y / 2f);
            Vector3 pos = new Vector3(hit.point.x, y, hit.point.z);
            previewObject.transform.position = pos;
        }
    }
}
