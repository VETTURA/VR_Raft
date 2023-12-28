using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanHandler : MonoBehaviour
{
    public List<GameObject> children = new List<GameObject>();

    [SerializeField]
    private List<Water> waterComponent = new List<Water>();

    [SerializeField]
    private GameObject _currentChild;

    public GameObject CurrentChild
    {
        get { return _currentChild; }
        set { _currentChild = value; }
    }

    [SerializeField]
    private List<Color> skyboxColors;

    private delegate void OceanEventHandler(ref List<GameObject> children, float duration, ref List<Color> skyboxColor);
    private static event OceanEventHandler handler;

    public float lerpDuration = 3.0f;
    public float cycleDuration = 10.0f;

    IEnumerator BlendOceans(List<GameObject> children, float duration, List<Color> skyboxColor)
    {
        Color currentSkyboxColor = RenderSettings.skybox.GetColor("_SkyTint");

        Vector3 activePosition = children[0].transform.localPosition;
        Vector3 hiddenPosition = children[1].transform.localPosition;
        float time = 0;

        while (time < duration)
        {
            children[1].transform.localPosition = Vector3.Lerp(hiddenPosition, activePosition, time / duration);
            children[0].transform.localPosition = Vector3.Lerp(activePosition, hiddenPosition, time / duration);
            
            RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(currentSkyboxColor, skyboxColor[1], time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        children.Reverse();
        waterComponent.Reverse();
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

            handler(ref children, lerpDuration, ref skyboxColors);

            waterComponent[0].peepeepoopoo(duration);
            //waterComponent[1]._waterSettings = waterComponent[1].WaterState[]
            waterComponent[1].peepeepoopoo(duration);
            //WaterStateCondition(1, duration);
        }
    }

    public void BlendOceansHandler(ref List<GameObject> children, float duration, ref List<Color> skyboxColor)
    {
        StartCoroutine(BlendOceans(children, duration, skyboxColor));
    }

    private void Start()
    {
        skyboxColors.Add(new Color(0.6f, 0.6f, 0.7f));
        skyboxColors.Add(new Color(0, 0, 0));
        RenderSettings.skybox.SetColor("_SkyTint", skyboxColors[0]);

        handler += BlendOceansHandler;

        foreach (Transform child in transform)
        {
            if (child != null)
            {
                children.Add(child.gameObject);
                waterComponent.Add(child.gameObject.GetComponent<Water>());
            }
        }
        CurrentChild = children[0];

        StartCoroutine(WeatherCycle(cycleDuration));
    }
}
