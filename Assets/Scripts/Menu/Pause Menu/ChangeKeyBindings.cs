using System.Collections;
using UnityEngine;
using TMPro;

public class ChangeKeyBindings : MonoBehaviour
{
    public Keybindings Keybindings;

    [Header("All the text objects")]
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI crouchText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI sprintText;
    public TextMeshProUGUI reloadText;

    private bool keyPressed;
    private KeyCode tempKeyCode;
    private int index;

    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
            tempKeyCode = e.keyCode;
    }

    public void ChangeJump()
    {
        index = 0;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeInteract()
    {
        index = 1;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangePause()
    {
        index = 2;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeInventory()
    {
        index = 3;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeCrouch()
    {
        index = 4;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeHeal()
    {
        index = 5;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeSprint()
    {
        index = 6;
        StartCoroutine(WaitForKeyPress());
    }

    public void ChangeReload()
    {
        index = 7;
        StartCoroutine(WaitForKeyPress());
    }

    IEnumerator WaitForKeyPress()
    {
        while (tempKeyCode == KeyCode.None)
        {
            yield return null;
        }

        Keybindings.KeybindingChecks[index].keyCode = tempKeyCode;

        switch (index)
        {
            case (0):
                jumpText.text = $"[{tempKeyCode}] Change Jump Key";
                break;
            
            case (1):
                interactText.text = $"[{tempKeyCode}] Change Interact Key";
                break;

            case (2):
                pauseText.text = $"[{tempKeyCode}] Change Pause Key";
                break;

            case (3):
                inventoryText.text = $"[{tempKeyCode}] Change Inventory Key";
                break;

            case (4):
                crouchText.text = $"[{tempKeyCode}] Change Crouch Key";
                break;

            case (5):
                healText.text = $"[{tempKeyCode}] Change Heal Key";
                break;

            case (6):
                sprintText.text = $"[{tempKeyCode}] Change Sprint Key";
                break;

            case (7):
                reloadText.text = $"[{tempKeyCode}] Change Reload Key";
                break;
        }

        tempKeyCode = KeyCode.None;
    }
}
