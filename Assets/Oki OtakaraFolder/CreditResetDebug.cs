using UnityEngine;
using UnityEngine.InputSystem;

public class CreditResetDebug : MonoBehaviour
{
    public CreditManager creditManager;

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            creditManager.ResetCredit();

        }
    }
}