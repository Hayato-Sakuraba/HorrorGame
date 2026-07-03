using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Image itemImage;
    public TMP_Text descriptionText;

    private bool showing = false;

    private void Start()
    {
        popupPanel.SetActive(false);
    }

    private void Update()
    {
        if (
            showing &&
            Keyboard.current.eKey.wasPressedThisFrame
        )
        {
            ClosePopup();
        }
    }

    public void ShowPopup(Otakara item)
    {
        popupPanel.SetActive(true);

        itemImage.sprite = item.icon;
        descriptionText.text = item.description;

        showing = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ClosePopup()
    {
        popupPanel.SetActive(false);

        showing = false;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool IsShowing()
    {
        return showing;
    }
}