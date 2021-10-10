using UnityEngine;
using TMPro;

public class InteractManager : MonoBehaviour
{
    public Camera cam;
    public GameObject normalCrosshair;
    public GameObject selectCrosshair;
    public TextMeshProUGUI interactText;

    public Keybindings keybindings;

    public static bool isLookingAtInteractable;

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 3))
        {
            Interactable interactable = hit.transform.GetComponent<Interactable>();

            if (interactable != null)
            {
                normalCrosshair.SetActive(false);
                selectCrosshair.SetActive(true);

                isLookingAtInteractable = true;

                interactText.gameObject.SetActive(true);

                if (interactable.interactType.ToString().Equals("Item"))
                {
                    if (interactable.item == null)
                        interactText.text = $"Uh Oh! This looks null, why don't you report that to the devs :)";
                    else if (!string.IsNullOrEmpty(interactable.item.itemName) && !interactable.item.interactEvent.Equals(InteractEvent.PickUp))
                        interactText.text = $"[{keybindings.KeybindingChecks[1].keyCode}] {interactable.item.interactEvent}\n{interactable.item.itemName}";
                    else if (!string.IsNullOrEmpty(interactable.item.itemName) && interactable.item.interactEvent.Equals(InteractEvent.PickUp))
                        interactText.text = $"[{keybindings.KeybindingChecks[1].keyCode}] Pick Up\n{interactable.item.itemName}";
                    else
                        interactText.text = $"Uh Oh! This looks null, why don't you report that to the devs :)";

                }
                else if (interactable.interactType.ToString().Equals("NPC"))
                {
                    if (interactable.npc == null)
                        interactText.text = $"Uh Oh! This looks null, why don't you report that to the devs :)";
                    else if (!string.IsNullOrEmpty(interactable.npc.npcName))
                        interactText.text = $"[{keybindings.KeybindingChecks[1].keyCode}] {interactable.npc.interactEvent}\n{interactable.npc.npcName}";
                    else
                        interactText.text = $"Uh Oh! This looks null, why don't you report that to the devs :)";
                }
                else if (interactable.item == null || interactable.npc == null)
                {
                    interactText.text = $"Uh Oh! This looks null, why don't you report that to the devs :)";
                }

                if (InputManager.instance.GetKeyDown(Actions.Interact))
                {
                    if (interactable.interactType.ToString().Equals("Item") && interactable.item != null)
                    {
                        if (interactable.item.interactEvent.ToString() == "PickUp")
                            interactable.PickUp();
                        else if (interactable.item.interactEvent.ToString() == "Open")
                            interactable.Open();
                        else if (interactable.item.interactEvent.ToString() == "Wear")
                            interactable.Wear();
                    }
                    else if (interactable.interactType.ToString().Equals("NPC") && interactable.npc != null)
                    {
                        if (interactable.npc.interactEvent.ToString().Equals("Talk"))
                        {
                            interactable.dialogueBase.Enqueue();

                            if (DiscordPresence.PresenceManager.instance != null)
                                DiscordPresence.PresenceManager.UpdatePresence(detail: "Talking to Bob", state: "Playing the game game", largeKey: "bob", largeText: "Isn't Bob pretty cool");
                        }
                    }
                }
            }
            else
            {
                normalCrosshair.SetActive(true);
                selectCrosshair.SetActive(false);

                interactText.gameObject.SetActive(false);
            }
        }
        else
        {
            normalCrosshair.SetActive(true);
            selectCrosshair.SetActive(false);

            interactText.gameObject.SetActive(false);

            isLookingAtInteractable = false;
        }
    }
}