using UnityEngine;
using System;

public class TrapEvent
{
    public static event Action<GameObject> OtonarashiTrapActivated;
    public static void TriggerOtonarashiTrap(GameObject player) => OtonarashiTrapActivated?.Invoke(player);
}
