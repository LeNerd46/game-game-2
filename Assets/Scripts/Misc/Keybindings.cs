using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    [System.Serializable]
    public class KeybindingCheck
    {
        public Actions KeybindingAction;
        public KeyCode keyCode;
    }

    public KeybindingCheck[] KeybindingChecks;
}
