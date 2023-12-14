using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private float damage = 0.1f;

    private GameObject player;

    private bool enter = false;

    // Start is called before the first frame update
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
    }
}
