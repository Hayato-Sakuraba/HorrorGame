using UnityEngine;
using TMPro;

public class CreditManager : MonoBehaviour
{
    public int currentCredit = 0;

    public TMP_Text creditText;
    private Inventory inventory;

    void Start()
    {
        currentCredit = PlayerPrefs.GetInt("Credit", 0);
        inventory = gameObject.GetComponent<Inventory>();

        UpdateUI();
    }

    public void AddCredit(int amount)
    {
        currentCredit += amount;

        Save();

        UpdateUI();
    }

    // デバッグ用のクレジットリセット
    
    public void ResetCredit()
    {
        currentCredit = 0;
        inventory.currentSize = 0;

        PlayerPrefs.DeleteKey("Credit");

        UpdateUI();

        Debug.Log("クレジットリセット");
    }
    //

    void UpdateUI()
    {
        if (creditText != null)
        {
            creditText.text = "クレジット : " + currentCredit;
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("Credit", currentCredit);


        PlayerPrefs.Save();
    }
}