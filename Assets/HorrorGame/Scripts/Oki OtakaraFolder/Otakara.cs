using UnityEngine;

[CreateAssetMenu(fileName = "Otakara", menuName = "Otakara/kariOtakara")]
public class Otakara : ScriptableObject
{
    public int price;
    public int currentPrice;
    public int guram;

    
    public Sprite icon;
    [TextArea]
    public string description;
    

    private void OnEnable()
    {
        currentPrice = price;
    }
}