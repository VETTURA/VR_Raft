using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Water : MonoBehaviour
{
    public delegate void OnWeatherChanged(WaterSettings targetSettings, float duration, Color skyboxColor);
    public static event OnWeatherChanged weatherChanged;


    public Shader waterShader;

    [SerializeField]
    private WaterSettings _waterSettings;
    public WaterSettings WaterConfig
    {
        get { return _waterSettings;}
        set { _waterSettings = value; }
    }

    [SerializeField]
    private List<WaterSettings> waterState;
    public float weatherLerpDuration = 3.0f;

    public int planeLength = 10;
    public int quadRes = 10;

    private Camera cam;

    private ComputeBuffer waveBuffer;

    private Material waterMaterial;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] displacedVertices;
    private Vector3[] normals;
    private Vector3[] displacedNormals;

    [Header("FBM Settings")]
    public float vertexSeed = 0;
    public int vertexWaveCount = 8;
    public int fragmentWaveCount = 40;

    private float vertexSeedIter;
    private float vertexFrequency;
    private float vertexFrequencyMult;
    private float vertexAmplitude;
    private float vertexAmplitudeMult;
    private float vertexInitialSpeed;
    private float vertexSpeedRamp;
    private float vertexDrag;
    private float vertexHeight;
    private float vertexMaxPeak;
    private float vertexPeakOffset;

    private float fragmentFrequency;
    private float fragmentFrequencyMult;
    private float fragmentAmplitude;
    private float fragmentAmplitudeMult;
    private float fragmentInitialSpeed;
    private float fragmentSpeedRamp;
    private float fragmentDrag;
    private float fragmentMaxPeak;
    private float fragmentPeakOffset;

    [Header(" ")]
    public float normalStrength = 1;

    private Color ambient;
    private Color diffuseReflectance;
    private Color specularReflectance;
    private float shininess;
    private float specularNormalStrength;
    private Color fresnelColor;
    private bool useTextureForFresnel;
    private Texture environmentTexture;
    private float fresnelBias, fresnelStrength, fresnelShininess;
    private float fresnelNormalStrength;
    private Color tipColor;
    private float tipAttenuation;

    private Color skyboxColor1;
    private Color skyboxColor2;

    private void SetFragmentSettings()
    {
        waterMaterial.SetInt("_FragmentWaveCount", fragmentWaveCount);
        waterMaterial.SetFloat("_FragmentSeed", vertexSeed);
        waterMaterial.SetFloat("_FragmentSeedIter", vertexSeedIter);
        waterMaterial.SetFloat("_FragmentHeight", vertexHeight);
    }

    private void RetrieveInitSettings(WaterSettings waterSettings)
    {
        vertexSeedIter = waterSettings.vertexSeedIter;
        vertexFrequency = waterSettings.vertexFrequency;
        vertexFrequencyMult = waterSettings.vertexFrequencyMult;
        vertexAmplitude = waterSettings.vertexAmplitude;
        vertexAmplitudeMult = waterSettings.vertexAmplitudeMult;
        vertexInitialSpeed = waterSettings.vertexInitialSpeed;
        vertexSpeedRamp = waterSettings.vertexSpeedRamp;
        vertexDrag = waterSettings.vertexDrag;
        vertexHeight = waterSettings.vertexHeight;
        vertexMaxPeak = waterSettings.vertexMaxPeak;
        vertexPeakOffset = waterSettings.vertexPeakOffset;

        fragmentFrequency = waterSettings.fragmentFrequency;
        fragmentFrequencyMult = waterSettings.fragmentFrequencyMult;
        fragmentAmplitude = waterSettings.fragmentAmplitude;
        fragmentAmplitudeMult = waterSettings.fragmentAmplitudeMult;
        fragmentInitialSpeed = waterSettings.fragmentInitialSpeed;
        fragmentSpeedRamp = waterSettings.fragmentSpeedRamp;
        fragmentDrag = waterSettings.fragmentDrag;
        fragmentMaxPeak = waterSettings.fragmentMaxPeak;
        fragmentPeakOffset = waterSettings.fragmentPeakOffset;

        ambient = waterSettings.ambient;
        diffuseReflectance = waterSettings.diffuseReflectance;
        specularReflectance = waterSettings.specularReflectance;
        shininess = waterSettings.shininess;
        specularNormalStrength = waterSettings.specularNormalStrength;
        fresnelColor = waterSettings.fresnelColor;
        useTextureForFresnel = waterSettings.useTextureForFresnel;
        environmentTexture = waterSettings.environmentTexture;
        fresnelBias = waterSettings.fresnelBias;
        fresnelStrength = waterSettings.fresnelStrength;
        fresnelShininess = waterSettings.fresnelShininess;
        fresnelNormalStrength = waterSettings.fresnelNormalStrength;
        tipColor = waterSettings.tipColor;
        tipAttenuation = waterSettings.tipAttenuation;
    }

    private void CreateWaterPlane()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Water";
        mesh.indexFormat = IndexFormat.UInt32;

        float halfLength = planeLength * 0.5f;
        int sideVertCount = planeLength * quadRes;

        vertices = new Vector3[(sideVertCount + 1) * (sideVertCount + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, x = 0; x <= sideVertCount; ++x)
        {
            for (int z = 0; z <= sideVertCount; ++z, ++i)
            {
                vertices[i] = new Vector3(((float)x / sideVertCount * planeLength) - halfLength, 0, ((float)z / sideVertCount * planeLength) - halfLength);
                uv[i] = new Vector2((float)x / sideVertCount, (float)z / sideVertCount);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[sideVertCount * sideVertCount * 6];

        for (int ti = 0, vi = 0, x = 0; x < sideVertCount; ++vi, ++x)
        {
            for (int z = 0; z < sideVertCount; ti += 6, ++vi, ++z)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = vi + sideVertCount + 2;
                triangles[ti + 3] = vi;
                triangles[ti + 4] = vi + sideVertCount + 2;
                triangles[ti + 5] = vi + sideVertCount + 1;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        normals = mesh.normals;

        displacedVertices = new Vector3[vertices.Length];
        Array.Copy(vertices, 0, displacedVertices, 0, vertices.Length);
        displacedNormals = new Vector3[normals.Length];
        Array.Copy(normals, 0, displacedNormals, 0, normals.Length);
    }

    void CreateMaterial()
    {
        if (waterShader == null) return;
        if (waterMaterial != null) return;

        waterMaterial = new Material(waterShader);
        waterMaterial.EnableKeyword("STEEP_SINE_WAVE");

        waterMaterial.EnableKeyword("USE_VERTEX_DISPLACEMENT");
        mesh.vertices = vertices;
        mesh.normals = normals;

        waterMaterial.SetBuffer("_Waves", waveBuffer);

        waterMaterial.EnableKeyword("NORMALS_IN_PIXEL_SHADER");

        waterMaterial.EnableKeyword("CIRCULAR_WAVES");

        waterMaterial.EnableKeyword("STEEP_SINE_WAVE");

        waterMaterial.EnableKeyword("USE_FBM");


        MeshRenderer renderer = GetComponent<MeshRenderer>();

        renderer.material = waterMaterial;
    }

    IEnumerator WaterSettingsBlend(WaterSettings targetSettings, float duration, Color skyboxColor)
    {
        Color currentSkyboxColor = RenderSettings.skybox.GetColor("_SkyTint");
        WaterSettings currentSettings = WaterConfig;
        float time = 0;
        float timeScale = time / duration;

        while (time < duration)
        {
            vertexSeedIter = Mathf.Lerp(currentSettings.vertexSeedIter, targetSettings.vertexSeedIter, time / duration);
            vertexFrequency = Mathf.Lerp(currentSettings.vertexFrequency, targetSettings.vertexFrequency, time / duration);
            vertexFrequencyMult = Mathf.Lerp(currentSettings.vertexFrequencyMult, targetSettings.vertexFrequencyMult, time / duration);
            vertexAmplitude = Mathf.Lerp(currentSettings.vertexAmplitude, targetSettings.vertexAmplitude, time / duration);
            vertexAmplitudeMult = Mathf.Lerp(currentSettings.vertexAmplitudeMult, targetSettings.vertexAmplitudeMult, time / duration);
            vertexInitialSpeed = Mathf.Lerp(currentSettings.vertexInitialSpeed, targetSettings.vertexInitialSpeed, time / duration);
            vertexSpeedRamp = Mathf.Lerp(currentSettings.vertexSpeedRamp, targetSettings.vertexSpeedRamp, time / duration);
            vertexDrag = Mathf.Lerp(currentSettings.vertexDrag, targetSettings.vertexDrag, time / duration);
            vertexHeight = Mathf.Lerp(currentSettings.vertexHeight, targetSettings.vertexHeight, time / duration);
            vertexMaxPeak = Mathf.Lerp(currentSettings.vertexMaxPeak, targetSettings.vertexMaxPeak, time / duration);
            vertexPeakOffset = Mathf.Lerp(currentSettings.vertexPeakOffset, targetSettings.vertexPeakOffset, time / duration);

            fragmentFrequency = Mathf.Lerp(currentSettings.fragmentFrequency, targetSettings.fragmentFrequency, time / duration);
            fragmentFrequencyMult = Mathf.Lerp(currentSettings.fragmentFrequencyMult, targetSettings.fragmentFrequencyMult, time / duration);
            fragmentAmplitude = Mathf.Lerp(currentSettings.fragmentAmplitude, targetSettings.fragmentAmplitude, time / duration);
            fragmentAmplitudeMult = Mathf.Lerp(currentSettings.fragmentAmplitudeMult, targetSettings.fragmentAmplitudeMult, time / duration);
            fragmentInitialSpeed = Mathf.Lerp(currentSettings.fragmentInitialSpeed, targetSettings.fragmentInitialSpeed, time / duration);
            fragmentSpeedRamp = Mathf.Lerp(currentSettings.fragmentSpeedRamp, targetSettings.fragmentSpeedRamp, time / duration);
            fragmentDrag = Mathf.Lerp(currentSettings.fragmentDrag, targetSettings.fragmentDrag, time / duration);
            fragmentMaxPeak = Mathf.Lerp(currentSettings.fragmentMaxPeak, targetSettings.fragmentMaxPeak, time / duration);
            fragmentPeakOffset = Mathf.Lerp(currentSettings.fragmentPeakOffset, targetSettings.fragmentPeakOffset, time / duration);

            ambient = Color.Lerp(currentSettings.ambient, targetSettings.ambient, time / duration);
            diffuseReflectance = Color.Lerp(currentSettings.diffuseReflectance, targetSettings.diffuseReflectance, time / duration);
            specularReflectance = Color.Lerp(currentSettings.specularReflectance, targetSettings.specularReflectance, time / duration);
            shininess = Mathf.Lerp(currentSettings.shininess, targetSettings.shininess, time / duration);
            specularNormalStrength = Mathf.Lerp(currentSettings.specularNormalStrength, targetSettings.specularNormalStrength, time / duration);
            fresnelColor = Color.Lerp(currentSettings.fresnelColor, targetSettings.fresnelColor, time / duration);
            //useTextureForFresnel = waterSettings.useTextureForFresnel;
            environmentTexture = targetSettings.environmentTexture;
            fresnelBias = Mathf.Lerp(currentSettings.fresnelBias, targetSettings.fresnelBias, time / duration);
            fresnelStrength = Mathf.Lerp(currentSettings.fresnelStrength, targetSettings.fresnelStrength, time / duration);
            fresnelShininess = Mathf.Lerp(currentSettings.fresnelShininess, targetSettings.fresnelShininess, time / duration);
            fresnelNormalStrength = Mathf.Lerp(currentSettings.fresnelNormalStrength, targetSettings.fresnelNormalStrength, time / duration);
            tipColor = Color.Lerp(currentSettings.tipColor, targetSettings.tipColor, time / duration);
            tipAttenuation = Mathf.Lerp(currentSettings.tipAttenuation, targetSettings.tipAttenuation, time / duration);
            RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(currentSkyboxColor, skyboxColor, time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        WaterConfig = targetSettings;
        RenderSettings.skybox.SetColor("_SkyTint", skyboxColor);
    }

    private void WaterSettingsBlendHandler(WaterSettings targetSettings, float duration, Color skyboxColor)
    {
        StartCoroutine(WaterSettingsBlend(targetSettings, duration, skyboxColor));
    }

    private void Start()
    {
        weatherChanged += WaterSettingsBlendHandler;
    }

    void OnEnable()
    {
        waterState.Add(Resources.Load<WaterSettings>("OceanStates/Calm"));
        waterState.Add(Resources.Load<WaterSettings>("OceanStates/Stormy"));
        WaterConfig = waterState[0];
        skyboxColor1 = new Color(0.5f, 0.5f, 0.5f);
        skyboxColor2 = new Color(0, 0, 0);
        RenderSettings.skybox.SetColor("_SkyTint", skyboxColor1);

        CreateWaterPlane();
        CreateMaterial();
        RetrieveInitSettings(WaterConfig);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (WaterConfig == waterState[0]) { weatherChanged(waterState[1], weatherLerpDuration, skyboxColor2); }
        //    if (WaterConfig == waterState[1]) { weatherChanged(waterState[0], weatherLerpDuration, skyboxColor1); }
        //}
        waterMaterial.SetVector("_Ambient", ambient);
        waterMaterial.SetVector("_DiffuseReflectance", diffuseReflectance);
        waterMaterial.SetVector("_SpecularReflectance", specularReflectance);
        waterMaterial.SetVector("_TipColor", tipColor);
        waterMaterial.SetVector("_FresnelColor", fresnelColor);
        waterMaterial.SetFloat("_Shininess", shininess * 100);
        waterMaterial.SetFloat("_FresnelBias", fresnelBias);
        waterMaterial.SetFloat("_FresnelStrength", fresnelStrength);
        waterMaterial.SetFloat("_FresnelShininess", fresnelShininess);
        waterMaterial.SetFloat("_TipAttenuation", tipAttenuation);
        waterMaterial.SetFloat("_FresnelNormalStrength", fresnelNormalStrength);
        waterMaterial.SetFloat("_SpecularNormalStrength", specularNormalStrength);
        waterMaterial.SetInt("_UseEnvironmentMap", useTextureForFresnel ? 1 : 0);

        if (useTextureForFresnel)
        {
            waterMaterial.SetTexture("_EnvironmentMap", environmentTexture);
        }

        Matrix4x4 projMatrix = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
        Matrix4x4 viewProjMatrix = projMatrix * cam.worldToCameraMatrix;
        waterMaterial.SetMatrix("_CameraInvViewProjection", viewProjMatrix.inverse);
        waterMaterial.SetInt("_VertexWaveCount", vertexWaveCount);
        waterMaterial.SetFloat("_VertexSeed", vertexSeed);
        waterMaterial.SetFloat("_VertexSeedIter", vertexSeedIter);
        waterMaterial.SetFloat("_VertexFrequency", vertexFrequency);
        waterMaterial.SetFloat("_VertexFrequencyMult", vertexFrequencyMult);
        waterMaterial.SetFloat("_VertexAmplitude", vertexAmplitude);
        waterMaterial.SetFloat("_VertexAmplitudeMult", vertexAmplitudeMult);
        waterMaterial.SetFloat("_VertexInitialSpeed", vertexInitialSpeed);
        waterMaterial.SetFloat("_VertexSpeedRamp", vertexSpeedRamp);
        waterMaterial.SetFloat("_VertexDrag", vertexDrag);
        waterMaterial.SetFloat("_VertexHeight", vertexHeight);
        waterMaterial.SetFloat("_VertexMaxPeak", vertexMaxPeak);
        waterMaterial.SetFloat("_VertexPeakOffset", vertexPeakOffset);
        SetFragmentSettings();
        waterMaterial.SetFloat("_FragmentFrequency", fragmentFrequency);
        waterMaterial.SetFloat("_FragmentFrequencyMult", fragmentFrequencyMult);
        waterMaterial.SetFloat("_FragmentAmplitude", fragmentAmplitude);
        waterMaterial.SetFloat("_FragmentAmplitudeMult", fragmentAmplitudeMult);
        waterMaterial.SetFloat("_FragmentInitialSpeed", fragmentInitialSpeed);
        waterMaterial.SetFloat("_FragmentSpeedRamp", fragmentSpeedRamp);
        waterMaterial.SetFloat("_FragmentDrag", fragmentDrag);
        waterMaterial.SetFloat("_FragmentMaxPeak", fragmentMaxPeak);
        waterMaterial.SetFloat("_FragmentPeakOffset", fragmentPeakOffset);

        waterMaterial.SetFloat("_NormalStrength", normalStrength);
        waterMaterial.SetBuffer("_Waves", waveBuffer);
    }

    void OnDisable()
    {
        if (waterMaterial != null)
        {
            Destroy(waterMaterial);
            waterMaterial = null;
        }

        if (mesh != null)
        {
            Destroy(mesh);
            mesh = null;
            vertices = null;
            normals = null;
            displacedVertices = null;
            displacedNormals = null;
        }

        if (waveBuffer != null)
        {
            waveBuffer.Release();
            waveBuffer = null;
        }
    }
}
