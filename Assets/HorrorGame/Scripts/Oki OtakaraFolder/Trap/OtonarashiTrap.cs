using UnityEngine;

public class OtonarashiTrap : MonoBehaviour, TrapInterface
{
    public void ActiveTrap(Transform playerTransform)
    {
        TrapEvent.TriggerOtonarashiTrap(playerTransform);
    }
}
