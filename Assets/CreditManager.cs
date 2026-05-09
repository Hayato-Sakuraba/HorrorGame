using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public int currentCredit = 0;

    public void AddCredit(int amount)
    {
        currentCredit += amount;
        Debug.Log("現在のクレジット: " + currentCredit);
    }
}