using UnityEngine;

public class Poison : MonoBehaviour, GimmickInterface
{
    public void isGimmickTriggered()
    {
        Debug.Log("毒のギミックが発動しました");
    }
}
