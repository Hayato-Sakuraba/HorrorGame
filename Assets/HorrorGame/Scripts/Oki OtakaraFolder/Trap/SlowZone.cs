using UnityEngine;

public class SlowZone : MonoBehaviour,TrapInterface
{
    public float slowMultiplier = 0.5f;

    public AudioSource audioSource;
    public AudioClip slowSE;
    private void OnTriggerEnter(Collider player)
    {
        if (!player.CompareTag("Player"))
        {
            return;
        }

        ActiveTrap(player.gameObject);
    }

    public void ActiveTrap(GameObject player)
    {
        DebugMove move =
            player.GetComponent<DebugMove>();

        if (move != null)
        {
            move.SetSpeed(
                move.moveSpeed * slowMultiplier
            );

            Debug.Log("ˆÚ“®‘¬“x’á‰º");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        DebugMove move =
            other.GetComponent<DebugMove>();

        if (move != null)
        {
            move.ResetSpeed();

            Debug.Log("‘¬“x–ß‚Á‚½");
        }
    }
}