using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private float damage = 0.1f;

    private GameObject player;

    private bool enter = false;

    void Start()
    {
        player = GameObject.Find("XR Origin (XR Rig)");
    }

    void Update()
    {
        if(enter)
        {
            player.GetComponent<Player>().DamagePlayer(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
        player.GetComponent<Player>().StopDamage();
    }
}
