using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100.0f;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(health);
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
    }
}
