using UnityEngine;

public class PlayerTrapActivated : MonoBehaviour
{
    private TrapInterface trapInterface;

    private void OnTriggerEnter(Collider Trap)
    {

        if (!Trap.CompareTag("Trap"))
        {
            return;
        }


        trapInterface =
            Trap.GetComponent<TrapInterface>();

        if (trapInterface != null)
        {
            trapInterface.ActiveTrap(gameObject);
        }
    }

    private void OnTriggerExit(Collider Trap)
    {
        if (!Trap.CompareTag("Trap"))
        {
            return;
        }

        if (trapInterface != null)
        {
            trapInterface.UnActiveTrap();
            trapInterface = null;
        }
    }
}
