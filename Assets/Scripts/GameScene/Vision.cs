using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision
{
    private TypeVision typeVision;
    private float apertureSize { get; set; }
    private float featherungEffect { get; set; }
    private Color vignetteColor { get; set; }
    private float vignetteColorBlend { get; set; }

    List<Vision> visions = new List<Vision>()
    {
        new Vision() {},
        new Vision() {},
        new Vision() {},
        new Vision() {}
    };
}

public enum TypeVision
{
    Clear,
    Hungry,
    Damage,
    HungryAndDamage
}