using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueUI;
    public GameObject dialogueOptionUI;
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;
    public GameObject weaponPlayer;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public GameObject[] optionTexts;

    private DialogueOptions dialogueOptions;
    private DialogueBase dialogue;

    private Queue<DialogueBase.Info> info = new Queue<DialogueBase.Info>();

    public bool isOption;
    public bool inDialogue;
    private bool clickedOnce;

    public int amountOfOptions;
    public int currentResponse;
    private int clickCount;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void EnqueueDialogue(DialogueBase dialogue)
    {
        weaponPlayer.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(false);
        weaponPlayer.GetComponentInChildren<WeaponSwitching>().enabled = false;

        /*if (inDialogue)
            return;
        else*/
        inDialogue = true;

        info.Clear();

        dialogueUI.SetActive(true);
        player.enabled = false;

        this.dialogue = dialogue;

        if (dialogue is DialogueOptions)
        {
            isOption = true;
            dialogueOptions = dialogue as DialogueOptions;
            amountOfOptions = dialogueOptions.options.Length;

            for (int i = 0; i < amountOfOptions; i++)
            {
                optionTexts[i].SetActive(false);
            }

            for (int i = 0; i < amountOfOptions; i++)
            {
                optionTexts[i].SetActive(true);
                optionTexts[i].GetComponent<TextMeshProUGUI>().text = dialogueOptions.options[i].optionName;
            }
        }
        else
            isOption = false;

        foreach (var information in dialogue.dialogue)
        {
            info.Enqueue(information);
            dialogueOptionUI.SetActive(false);
        }

        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (info.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueBase.Info information = info.Dequeue();

        if (!isOption)
            dialogueOptionUI.SetActive(false);

        dialogueName.text = information.characterName;
        dialogueText.text = information.text;
    }

    public void EndDialogue()
    {
        if (isOption)
            dialogueOptionUI.SetActive(true);
        else
        {
            player.enabled = true;
            weaponPlayer.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(false);
            weaponPlayer.GetComponentInChildren<WeaponSwitching>().enabled = false;
            dialogueUI.SetActive(false);
            dialogueOptionUI.SetActive(false);

            inDialogue = false;

            if (DiscordPresence.PresenceManager.instance != null)
                DiscordPresence.PresenceManager.UpdatePresence(detail: "Walking around", state: "Playing the game game", largeKey: "ingame", largeText: "This is a pretty good game game");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            dialogueUI.SetActive(false);
            player.enabled = true;
            weaponPlayer.GetComponentInChildren<WeaponSwitching>().gameObject.transform.GetChild(WeaponSwitching.selectedWeapon).gameObject.SetActive(true);
            weaponPlayer.GetComponentInChildren<WeaponSwitching>().enabled = true;
            inDialogue = false;
        }

        if (inDialogue)
        {
            if (Input.GetMouseButtonDown(0))
                DequeueDialogue();

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                currentResponse++;

                if (currentResponse >= amountOfOptions - 1)
                    currentResponse = amountOfOptions - 1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                currentResponse--;

                if (currentResponse < 0)
                    currentResponse = 0;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                currentResponse--;

                if (currentResponse < 0)
                    currentResponse = 0;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                currentResponse++;

                if (currentResponse >= amountOfOptions - 1)
                    currentResponse = amountOfOptions - 1;
            }

            for (int i = 0; i < amountOfOptions; i++)
            {
                if (i == currentResponse)
                    optionTexts[i].GetComponent<TextMeshProUGUI>().color = Color.grey;
                else
                    optionTexts[i].GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }

        if (Input.GetMouseButtonDown(0) && clickedOnce && inDialogue)
        {
            clickedOnce = false;

            for (int i = 0; i < dialogueOptions.options.Length; i++)
            {
                if (dialogueOptions.options[i].nextDialogue != null && currentResponse.Equals(i))
                {
                    if (dialogueOptions.options[i].nextDialogue.dialogue.Length.Equals(1))
                        clickedOnce = true;

                    if (dialogueOptions.options[i].giveQuest != null)
                        dialogueOptions.options[i].giveQuest.InitalizeQuest();

                    dialogueOptions.options[i].nextDialogue.Enqueue();
                }
                else if (dialogueOptions.options[i].nextDialogue == null && currentResponse.Equals(i))
                {
                    if (dialogueOptions.options[i].giveQuest != null)
                        dialogueOptions.options[i].giveQuest.InitalizeQuest();

                    isOption = false;
                    EndDialogue();
                }
                else if (dialogue.dialogue.Length.Equals(1))
                {
                    if (Input.GetMouseButtonDown(0))
                        clickCount++;

                    if (clickCount.Equals(3))
                    {
                        if (dialogueOptions.options[i].giveQuest != null)
                            dialogueOptions.options[i].giveQuest.InitalizeQuest();

                        isOption = false;
                        EndDialogue();

                        clickCount = 0;
                        currentResponse = 0;
                        inDialogue = false;
                    }
                }
            }
        }
        else if (dialogueOptionUI.activeSelf && !clickedOnce && Input.GetMouseButtonDown(0))
            clickedOnce = true;
    }
}
