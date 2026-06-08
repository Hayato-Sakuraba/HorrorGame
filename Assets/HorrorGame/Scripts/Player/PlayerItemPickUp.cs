// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerItemPickUp : MonoBehaviour
// {
//     [SerializeField] private Inventory inventory;
//     [SerializeField] private LayerMask itemLayer;
//     [SerializeField] private LayerMask BuyerLayer;
//     [SerializeField] private PlayerInput playerInput;
//     private bool itemInRange = false;
//     private bool buyerInRange = false;
//     private GameObject Item;
//     private bool Interact => playerInput.actions["Interact"].IsPressed();

//     void Update()
//     {
//         if (Interact && itemInRange)
//         {
//             Otakara _otakara = Item.GetComponent<OtakaraInfo>().otakara;
//             if (_otakara != null)
//             {
//                 inventory.AddItem(_otakara);
//                 Item.SetActive(false);
//             }
//         }
        
//         if(Interact && buyerInRange)
//         {
//             inventory.ConvertToCredit();
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (((1 << collision.gameObject.layer) & itemLayer) != 0)
//         {
//             itemInRange = true;
//             Item = collision.gameObject;
//         }

//         if (((1 << collision.gameObject.layer) & BuyerLayer) != 0)
//         {
//             buyerInRange = true;
//         }
//     }

//     private void OnTriggerExit2D(Collider2D collision)
//     {
//         if (((1 << collision.gameObject.layer) & itemLayer) != 0)
//         {
//             itemInRange = false;
//             Item = null;
//         }

//         if (((1 << collision.gameObject.layer) & BuyerLayer) != 0)
//         {
//             buyerInRange = false;
//         }
//     }
    
// }
