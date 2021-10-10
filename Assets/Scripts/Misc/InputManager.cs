using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Keybindings Keybindings;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GameManager.instance.onGameSaved += OnSave;
        GameManager.instance.onGameLoaded += OnLoad;
    }

    public KeyCode GetKeyForAction(Actions keybindingAction)
    {
        foreach (var keybindingCheck in Keybindings.KeybindingChecks)
        {
            if (keybindingCheck.KeybindingAction == keybindingAction)
                return keybindingCheck.keyCode;
        }

        return KeyCode.None;
    }

    public bool GetKeyDown(Actions key)
    {
        foreach (var keybindingCheck in Keybindings.KeybindingChecks)
        {
            if (keybindingCheck.KeybindingAction == key)
                return Input.GetKeyDown(keybindingCheck.keyCode);
        }

        return false;
    }

    public bool GetKey(Actions key)
    {
        foreach (var keybindingCheck in Keybindings.KeybindingChecks)
        {
            if (keybindingCheck.KeybindingAction == key)
                return Input.GetKey(keybindingCheck.keyCode);
        }

        return false;
    }

    public void OnSave()
    {
        SaveData.Instance.keybindingsJson = JsonUtility.ToJson(Keybindings);
    }

    public void OnLoad()
    {
        JsonUtility.FromJsonOverwrite(SaveData.Instance.keybindingsJson, Keybindings);
    }
}
