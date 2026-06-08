using UnityEngine;

public class PlayerItemPick : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInput playerInput;
    private OtakaraInfo currentOtakaraInfo;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Otakara"))
        {
            if(other.TryGetComponent(out OtakaraInfo otakaraInfo))
                currentOtakaraInfo = otakaraInfo;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Otakara"))
        {
            currentOtakaraInfo = null;
        }
    }

    void Update()
    {
        if (playerInput.Interact && currentOtakaraInfo != null)
        {
            currentOtakaraInfo.Pickup(inventory);
            currentOtakaraInfo = null;
        }
    }
}
