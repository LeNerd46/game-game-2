using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    public GameObject ui;
    // public GameObject deathScreen;
    public GameObject redVignette;

    public TextMeshProUGUI healsLeftText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI healthText;

    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;

    public static int armorPoints;
    public int maxHealth = 100;
    public int health = 100;
    public int healsLeft = 25;
    public int temp = 100;

    [Tooltip("How high up the player has to be to take fall damage")]
    public int damageThreshold;

    private float startYPos;
    private float endYPos;

    public float seconds;
    public float timeToHeal = 5f;
    public float tempDropRate;

    private float timer;
    private float timerHeal;
    private float tempTimer;
    private float tempDamageTimer;
    private float healTimer;

    private bool tookDamage;
    private bool increasingHeals;
    private bool increasedHealth;
    private bool firstCall;
    private bool takeFallDamage;
    private bool healed;

    public Slider healthSlider;
    public Slider tempSlider;

    void Start()
    {
        GameManager.instance.onGameSaved += OnGameSaved;
        GameManager.instance.onGameLoaded += OnLoad;
    }

    void Update()
    {
        armorText.text = armorPoints.ToString();
        healthText.text = $"{health} / {maxHealth}";

        if (Input.GetKeyDown(KeyCode.K))
            armorPoints = 0;

        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;

        if (health > maxHealth)
            health = maxHealth;

        if (Input.GetKeyDown(KeyCode.M))
            TakeDamage(5);

        if (Input.GetKeyDown(KeyCode.L))
            maxHealth += 15;

        if (health <= 0)
            Die();

        if(temp <= 0)
        {
            if (tempDamageTimer >= 0)
                tempDamageTimer -= Time.deltaTime;
            else
            {
                tempDamageTimer = 1;
                TakeDamage(Mathf.RoundToInt((float)(maxHealth * 0.05)));
            }
        }

        tempSlider.value = temp;

        if (tempTimer >= 0 && temp > 0)
            tempTimer -= Time.deltaTime;
        else if(tempTimer <= 0 && temp > 0)
        {
            tempTimer = tempDropRate;
            temp--;
        }

        if (health == maxHealth && temp == 100)
        {
            tempSlider.gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
        }
        else
        {
            tempSlider.gameObject.SetActive(true);
            healthSlider.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
        }

        if (health <= maxHealth * 0.35)
        {
            redVignette.SetActive(true);
            redVignette.GetComponent<Animator>().Play("RedVignette");
        }
        else if (health > maxHealth * 0.2 && health <= maxHealth * 0.35)
        {
            redVignette.SetActive(true);
            redVignette.GetComponent<Animator>().Play("SortaLowHealth");
        }
        else
            redVignette.SetActive(false);

        healsLeftText.text = healsLeft.ToString();

        healTimer -= Time.deltaTime;

        if (healTimer <= 0f)
        {
            healTimer = 1f;

            if(healed)
            {
                Heal(1);
            }
        }

        if (InputManager.instance.GetKeyDown(Actions.Heal))
        {
            if (healsLeft > 0)
            {
                healed = true;
                healsLeft--;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!increasingHeals)
                StartCoroutine(IncreaseHealsLeft());
        }

        timerHeal += Time.deltaTime;

        if(tookDamage)
        {
            timer = timeToHeal;
            tookDamage = false;
        }
        else
            timer -= Time.deltaTime;

        if(timer <= 0)
        {
            if (timerHeal >= seconds && health <= maxHealth * 0.7)
            {
                timerHeal = 0;
                health++;
            }
        }

        // Fall Damage Code
        if (!player.Grounded)
        {
            if (transform.position.y > startYPos)
                firstCall = true;

            if (firstCall)
            {
                startYPos = gameObject.transform.position.y;
                firstCall = false;
                takeFallDamage = true;
            }
        }

        if (player.Grounded)
        {
            endYPos = transform.position.y;

            if (startYPos - endYPos > damageThreshold)
            {
                if (takeFallDamage)
                {
                    TakeDamage((int)(startYPos - endYPos - damageThreshold));
                    takeFallDamage = false;
                    firstCall = true;
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        int adjustedAmount;

        tookDamage = true;

        adjustedAmount = amount - Mathf.RoundToInt((float)(armorPoints / 1.5));

        health -= adjustedAmount;
    }

    void Die()
    {
        Debug.Log("You died");
        ui.SetActive(false);
        // deathScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        ui.SetActive(true);
        // deathScreen.SetActive(false);

        Time.timeScale = 1;
        health = 100;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    IEnumerator IncreaseHealsLeft()
    {
        increasingHeals = true;

        yield return new WaitForSeconds(2);

        healsLeft += 1;

        increasingHeals = false;
    }

    private void OnGameSaved()
    {
        PlayerData data = new PlayerData
        {
            health = health,
            maxHealth = maxHealth,
            position = transform.position,
            rotation = transform.rotation
        };

        SaveData.Instance.playerData = data;
    }

    public void OnLoad()
    {
        health = SaveData.Instance.playerData.health;
        maxHealth = SaveData.Instance.playerData.maxHealth;
        transform.SetPositionAndRotation(SaveData.Instance.playerData.position, SaveData.Instance.playerData.rotation);
    }
}
