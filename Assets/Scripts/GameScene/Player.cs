using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100.0f;
    private float hunger = 100.0f;

    private bool damage = false;

    [SerializeField]
    private GameObject damageVision;

    void Start()
    {

    }

    void Update()
    {
        Healing();
        Appetite();
        ChangeVision();
    }

    public void DamagePlayer(float damageValue)
    {
        health -= damageValue;
        damage = true;

        if(health < 0)
        {
            //Сделать экран поражения, пока просто перенос в меню
            Conductor.ShowScene(Conductor.Scenes.MainMenu);
        }
    }

    public void StopDamage()
    {
        damage = false;
    }

    private void Healing()
    {
        if(damage == false && health < 100)
        {
            health += 0.01f;
        }
    }

    private void Appetite()
    {
        if(hunger > 0)
        {
            hunger -= 0.1f;
        }
        else
        {
            DamagePlayer(0.01f);
        }
    }
    
    private void Eating(float foodValue)
    {
        damage = false;
        hunger += foodValue;
    }

    private void ChangeVision()
    {
        float featheringEffect = 0.3f - ((0.3f - 0.08f) / 100 * (100 - hunger));
        float alphaVignetteColor = 0.96f / 100 * (100 - hunger);
        float rVignetteColor = 0;

        if(damage || health < 50)
        {
            rVignetteColor = 0.75f;

            if(featheringEffect > 0.1)
            {
                featheringEffect = 0.1f;
                alphaVignetteColor = 0.7f;
            }
        }

        if(featheringEffect > 0.08)
        {
            damageVision.GetComponent<Renderer>().sharedMaterial.SetFloat("_FeatheringEffect", featheringEffect);
            damageVision.GetComponent<Renderer>().sharedMaterial.SetColor("_VignetteColor", new Color(rVignetteColor, 0, 0, alphaVignetteColor));
        }
    }
}