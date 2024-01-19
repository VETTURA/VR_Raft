using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WindController : MonoBehaviour
{
    private System.Random random = new();


    [SerializeField]
    private float timer = 0;

    [SerializeField]
    private float coolDown;

    [SerializeField]
    private float windChance;
    
    public bool isWindy;


    public SailController sail;


    void Start()
    {
        sail = FindObjectOfType<SailController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            timer = coolDown;
            var chance = random.NextDouble();
            if (windChance > chance)
            {
                if (isWindy == false)
                {
                    Debug.Log("Ветренно");
                    isWindy = true;
                }
                else
                {
                    Debug.Log("Ветер прекратился");
                    isWindy = false;
                }
            }
                
        }

        sail.CheckWeather(isWindy);
    }

    public void ChangeSail()
    {
        sail = FindObjectOfType<SailController>();
    }
}
