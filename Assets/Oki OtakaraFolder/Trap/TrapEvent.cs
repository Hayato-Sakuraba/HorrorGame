using UnityEngine;
using System;

public class TrapEvent
{
    public static event Action<Transform> OtonarashiTrapActivated;
    public static void TriggerOtonarashiTrap(Transform playerTransform) => OtonarashiTrapActivated?.Invoke(playerTransform);
}
