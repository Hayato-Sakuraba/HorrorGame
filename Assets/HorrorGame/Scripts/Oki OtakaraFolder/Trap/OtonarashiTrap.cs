using UnityEngine;

public class OtonarashiTrap : MonoBehaviour, TrapInterface
{
    public void ActiveTrap(GameObject player)
    {
        TrapEvent.TriggerOtonarashiTrap(player);
    }
}
