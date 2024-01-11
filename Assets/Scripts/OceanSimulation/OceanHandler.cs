using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanHandler : MonoBehaviour
{
    public GameObject child;

    [SerializeField]
    private Water waterComponent;

    [SerializeField]
    private GameObject _currentChild;

    public GameObject CurrentChild
    {
        get { return _currentChild; }
        set { _currentChild = value; }
    }

    [SerializeField]
    private List<Color> skyboxColors;

    private delegate void OceanEventHandler(ref Water child, float duration, ref List<Color> skyboxColor);
    private static event OceanEventHandler handler;

    public float lerpDuration = 3.0f;
    public float cycleDuration = 10.0f;

    IEnumerator BlendOceans(Water waterComponent, float duration, List<Color> skyboxColor)
    {
        Color currentSkyboxColor = RenderSettings.skybox.GetColor("_SkyTint");

        float time = 0;

        if (waterComponent._waterSettings == waterComponent.WaterState[0])
        {
            waterComponent.WaterSettingsBlendHandler(waterComponent.WaterState[1], duration);
        }
        if (waterComponent._waterSettings == waterComponent.WaterState[1])
        {
            waterComponent.WaterSettingsBlendHandler(waterComponent.WaterState[0], duration);
        }

        while (time < duration)
        {
            RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(currentSkyboxColor, skyboxColor[1], time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        skyboxColors.Reverse();
        yield break;
    }

    IEnumerator WeatherCycle(float duration)
    {
        while (true)
        {
            float _time = 0;
            while (_time < duration)
            {
                _time += Time.deltaTime;
                yield return null;
            }

            handler(ref waterComponent, lerpDuration, ref skyboxColors);
        }
    }

    public void BlendOceansHandler(ref Water child, float duration, ref List<Color> skyboxColor)
    {
        StartCoroutine(BlendOceans(child, duration, skyboxColor));
    }

    private void Start()
    {
        skyboxColors.Add(new Color(0.6f, 0.6f, 0.7f));
        skyboxColors.Add(new Color(0, 0, 0));
        RenderSettings.skybox.SetColor("_SkyTint", skyboxColors[0]);

        handler += BlendOceansHandler;

        if (child == null)
        {
            child = transform.GetChild(0).gameObject;
            waterComponent = child.GetComponent<Water>();
        }

        StartCoroutine(WeatherCycle(cycleDuration));
    }
}
