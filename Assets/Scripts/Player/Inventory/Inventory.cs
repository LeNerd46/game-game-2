using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameObject container;
    public GameObject homePage;
    public GameObject UI;
    public GameObject questLog;
    public GameObject player;
    public GameObject inventory;
    public Transform questHolder;

    public InventoryObject inventoryObject;
    public GameObject holder;

    public TextMeshProUGUI levelText;
    public Slider xpSlider;

    public static bool MenuOpen { get; private set; }

    void OnEnable()
    {
        levelText.text = LevelingSystem.level.ToString();
        xpSlider.maxValue = LevelingSystem.xpNeeded;
        xpSlider.value = LevelingSystem.xp;
    }

    void Start()
    {
        /*for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().item == null)
                slots[i].GetComponent<Slot>().empty = true;
        }*/

        GameManager.instance.onGameSaved += OnGameSaved;
        GameManager.instance.onGameLoaded += OnGameLoaded;
    }

    void Update()
    {
        if (InputManager.instance.GetKeyDown(Actions.Inventory) && !MenuOpen)
        {
            // Open
            Cursor.lockState = CursorLockMode.None;
            container.SetActive(true);
            homePage.SetActive(true);

            MenuOpen = !MenuOpen;

            player.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(false);
            player.GetComponentInChildren<WeaponSwitching>().enabled = false;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
        }
        else if (InputManager.instance.GetKeyDown(Actions.Inventory) && MenuOpen)
        {
            // Close
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = true;

            MenuOpen = !MenuOpen;

            container.SetActive(false);
            homePage.SetActive(false);
            inventory.SetActive(false);
            questLog.SetActive(false);
            UI.SetActive(true);

            if (WeaponSwitching.weaponHidden)
                player.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(false);
            else
                player.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(true);

            player.GetComponentInChildren<WeaponSwitching>().enabled = true;
        }

        if (MenuOpen)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void OpenQuestLog()
    {
        MenuOpen = true;

        if (QuestLogManager.instance.lastDisplayedQuest != null)
            QuestLogManager.instance.UpdateQuestUI(QuestLogManager.instance.lastDisplayedQuest, QuestLogManager.instance.lastDisplayedQuest.GetObjectiveList());

        for (int i = 0; i < questHolder.childCount; i++)
        {
            if (!questHolder.GetChild(i).name.Equals("Handle"))
            {
                questHolder.GetChild(i).GetComponent<Image>().enabled = true;
                questHolder.GetChild(i).GetComponent<Button>().enabled = true;
                questHolder.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            }
        }

        homePage.SetActive(false);
        UI.SetActive(false);
        inventory.SetActive(false);
        questLog.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenInventory()
    {
        MenuOpen = true;

        for (int i = 0; i < inventoryObject.container.Count; i++)
        {
            holder.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = inventoryObject.container[i].item.itemName;
        }

        homePage.SetActive(false);
        UI.SetActive(false);
        inventory.SetActive(true);
        questLog.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
    }

    void OnApplicationQuit()
    {
        inventoryObject.container.Clear();
    }

    private void OnGameSaved()
    {
        foreach (var item in inventoryObject.container)
        {
            SaveData.Instance.itemDatas.Add(JsonUtility.ToJson(item.item));
        }
    }

    private void OnGameLoaded()
    {
        inventoryObject.container.Clear();

        foreach (var json in SaveData.Instance.itemDatas)
        {
            Debug.Log(json);

            Item scriptableObject = (Item)ScriptableObject.CreateInstance(typeof(Item));
            JsonUtility.FromJsonOverwrite(json, scriptableObject);

            inventoryObject.AddItem(scriptableObject, 1);
        }
    }
}
