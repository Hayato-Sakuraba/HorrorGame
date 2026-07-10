using UnityEngine;
using UnityEngine.UI;

public class RadiationEffect : MonoBehaviour
{
    public static RadiationEffect Instance;

    public Image radiationImage;

    private float timer;
    private float maxTime;
    private bool active;

    private void Awake()
    {
        Instance = this;

        Color color = radiationImage.color;
        color.a = 0f;
        radiationImage.color = color;
    }

    private void Update()
    {
        if (!active)
            return;

        timer += Time.deltaTime;

        float alpha = timer / maxTime;

        Color color = radiationImage.color;
        color.a = Mathf.Clamp01(alpha);

        radiationImage.color = color;
    }

    public void StartRadiation(float time)
    {
        timer = 0f;
        maxTime = time;
        active = true;
    }

    public void StopRadiation()
    {
        active = false;

        Color color = radiationImage.color;
        color.a = 0f;

        radiationImage.color = color;
    }
}