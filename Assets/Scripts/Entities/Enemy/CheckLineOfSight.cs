using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CheckLineOfSight : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public float fov = 90f;
    public LayerMask lineOfSightLayer;

    public delegate void GainSight(Transform target);
    public GainSight OnGainSight;
    public delegate void LoseSight(Transform target);
    public LoseSight OnLoseSight;

    private Coroutine checkSight;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(!CheckForSight(other.transform))
            checkSight = StartCoroutine(CheckSight(other.transform));
    }

    void OnTriggerExit(Collider other)
    {
        OnLoseSight?.Invoke(other.transform);

        if(checkSight != null)
            StopCoroutine(checkSight);
    }

    private bool CheckForSight(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);

        if(dotProduct >= Mathf.Cos(fov))
        {
            if(Physics.Raycast(transform.position, direction, out RaycastHit hit, sphereCollider.radius, lineOfSightLayer))
            {
                OnGainSight?.Invoke(target);
                return true;
            }
        }

        return false;
    }

    private IEnumerator CheckSight(Transform target)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while(!CheckForSight(target))
        {
            yield return wait;
        }
    }
}
