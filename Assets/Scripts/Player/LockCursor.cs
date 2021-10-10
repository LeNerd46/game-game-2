using UnityEngine;

public class LockCursor : MonoBehaviour
{
    public CapsuleCollider player;
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController settings;
    public Animator animator;

    public static bool crouching;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crouching = false;
    }

    void Update()
    {
        if (settings.movementSettings.Running)
            animator.SetBool("Sprinting", true);
        else
            animator.SetBool("Sprinting", false);

        if(!InputManager.instance.GetKeyDown(Actions.Sprint) && InputManager.instance.GetKeyDown(Actions.Crouch))
        {
            if (crouching)
            {
                player.height = 1.25f;
                settings.movementSettings.ForwardSpeed = 2f;
                crouching = false;
            }
            else if (!crouching)
            {
                player.height = 2f;
                settings.movementSettings.ForwardSpeed = 4f;
                crouching = true;
            }
        }
    }
}