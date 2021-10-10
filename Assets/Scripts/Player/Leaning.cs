using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaning : MonoBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;
    public Camera cam;

    public float rotationSpeed = 10f;
    public float maxAngle = 20f;

    public float number;

    private float curAngle;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // Figure out which direction to lean in
            
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit oHit, 3))
            {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(number, 0), out RaycastHit hit, 3))
                {
                    if (hit.collider == null)
                        Debug.Log("Didn't hit anything!");
                    else
                        curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("Didn't hit anything!");

                    curAngle = Mathf.MoveTowardsAngle(curAngle, -maxAngle, rotationSpeed * Time.deltaTime);
                }

                if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(-number, 0), out RaycastHit _hit, 3))
                {
                    if (hit.collider == null)
                        Debug.Log("Didn't hit anything!");
                    else
                        curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("Didn't hit anything!");

                    curAngle = Mathf.MoveTowardsAngle(curAngle, maxAngle, rotationSpeed * Time.deltaTime);
                }
            }

            cam.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
        }
    }
}
