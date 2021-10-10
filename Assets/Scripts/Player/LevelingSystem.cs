#region Old Code
/*using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelingSystem : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI levelText;
    
    public int level;
    public float xpPerLevel;
    public float xp;

    private int baseXP = 500;

    void Start()
    {
        slider.maxValue = xpPerLevel;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            xp += 100;
        }

        slider.value = xp;

        if(slider.value >= xpPerLevel)
        {
            LevelUp();
        }
    }
    
    public void LevelUp()
    {
        level++;
        levelText.text = level.ToString();

        float firstNumber;
        firstNumber = level * Mathf.Exp(1.5f);

        float secondNumber;
        secondNumber = baseXP / 4;

        xpPerLevel = baseXP * firstNumber - level * secondNumber;

        slider.maxValue = xpPerLevel;
        slider.value = 0;
        xp = 0;
    }
}*/
#endregion

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LevelingSystem : MonoBehaviour
{
    public static int level = 1;
    public static float xp;
    public static float xpNeeded;
    public int baseXP;

    public TextMeshProUGUI popUpText;

    void Update()
    {
        if (xp >= xpNeeded)
        {
            // Level Up
            CalculateXPNeeded();
            StartCoroutine(WaitForText());
        }

        if (Input.GetKeyDown(KeyCode.X))
            AddXP(100);
    }

    public void AddXP(int amount)
    {
        xp += amount;
    }

    private void CalculateXPNeeded()
    {
        xp -= xpNeeded;

        // Formula:
        // baseXP(level ^ 1.5) - level(baseXP * 2)

        level++;

        float firstNumber;
        firstNumber = level * Mathf.Exp(1.5f);

        float secondNumber;
        secondNumber = baseXP * 2;

        xpNeeded = baseXP * firstNumber - level * secondNumber;
    }

    IEnumerator WaitForText()
    {
        popUpText.gameObject.SetActive(true);
        popUpText.text = $"You Level Up\n{level}";

        yield return new WaitForSeconds(2);

        popUpText.gameObject.SetActive(false);
    }
}