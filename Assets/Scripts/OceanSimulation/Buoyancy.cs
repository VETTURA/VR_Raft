using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;

public class Buoyancy : MonoBehaviour
{
    Water water;
    private Mesh _mesh;

    private void OnEnable()
    {
        water = GetComponent<Water>();
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    private Vector3 vFBM(Vector3 v)
    {
        float frequency = water.waterSettings.vertexFrequency;
        float amplitude = water.waterSettings.vertexAmplitude;
        float speed = water.waterSettings.vertexInitialSpeed;
        float seed = water.vertexSeed;
        Vector3 p = v;
        float amplitudeSum = 0.0f;

        float h = 0.0f;
        Vector2 n = new Vector2(0.0f, 0.0f);

        for (int wave = 0; wave < 3; ++wave) 
        {
            Vector2 d = new Vector2(Mathf.Cos(seed), Mathf.Sin(seed));
            d.Normalize();
            Vector2 _p = new Vector2(p.x, p.z);
            float x = Vector2.Dot(d, _p) * frequency + Time.timeSinceLevelLoad * speed;
            float _wave = amplitude * Mathf.Exp(water.waterSettings.vertexMaxPeak * 
                Mathf.Sin(x) - 
                water.waterSettings.vertexPeakOffset);
            float dx = water.waterSettings.vertexMaxPeak * _wave * Mathf.Cos(x);

            h += _wave;
            _p += d * -dx * amplitude * water.waterSettings.vertexDrag;

            amplitudeSum += amplitude;
            frequency *= water.waterSettings.vertexFrequencyMult;
            amplitude *= water.waterSettings.fragmentAmplitudeMult;
            speed *= water.waterSettings.vertexSpeedRamp;
            seed += water.waterSettings.vertexSeedIter;
        }

        Vector3 output = new Vector3(h, n.x, n.y) / amplitudeSum;
        output.x *= water.waterSettings.vertexHeight;

        return output;
    }
    
    private void DisplaceVertices(Mesh m)
    {
        Vector3[] vertices = m.vertices;
        foreach (Vector3 v in vertices)
        {
            if (v != null)
            {

            }
        }
    }

}
