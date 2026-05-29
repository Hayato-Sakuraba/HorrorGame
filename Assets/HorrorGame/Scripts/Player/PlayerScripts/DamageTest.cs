using UnityEngine;
using UnityEngine.InputSystem;

public class DamageTest : MonoBehaviour
{
    private PlayerLife playerLife;

    private void Awake()
    {
        playerLife = GetComponent<PlayerLife>();
    }

    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            playerLife.Damage(1);
        }
    }
}