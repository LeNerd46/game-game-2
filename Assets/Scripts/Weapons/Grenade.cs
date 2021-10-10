using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Grenade : MonoBehaviour
{
    [Tooltip("How long it takes for the grenade to explode")]
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public int damage = 50;
    public float changeAmount = 0.1f;

    public GameObject explosionEffect;
    public ChromaticAberration chrome;

    private float timer;
    private float distortTimer;
    private float fullTimer;
    private float bufferTime;

    private bool hasExploded;
    private bool playerExploded;
    private bool hasReachedOne;

    void Start()
    {
        timer = delay;
        chrome = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<PostProcessVolume>().GetComponent<ChromaticAberration>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        distortTimer += Time.deltaTime;
        fullTimer += Time.deltaTime;

        if (timer <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

        if (playerExploded && distortTimer >= 0.5f)
        {
            chrome.intensity.value += changeAmount;

            distortTimer = 0f;
        }

        if (playerExploded && chrome.intensity.value == 1f)
        {
            if (bufferTime < 10)
                bufferTime += Time.deltaTime;
        }

        if(playerExploded && fullTimer >= 0.5f)
        {
            chrome.intensity.value -= changeAmount;

            fullTimer = 0f;
            hasReachedOne = true;
        }

        if (chrome.intensity.value <= 0 && hasReachedOne)
        {
            playerExploded = false;

            Destroy(gameObject);
            hasReachedOne = false;
        }
    }

    private void Explode()
    {
        // Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
            Target target = collider.GetComponent<Target>();

            if (rb != null)
                rb.AddExplosionForce(force, transform.position, radius);

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                playerExploded = true;
            }

            if (target != null)
                target.TakeDamage(damage);
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        // Destroy(gameObject);
    }
}
