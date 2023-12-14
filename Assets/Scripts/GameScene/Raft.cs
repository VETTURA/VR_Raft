using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raft : MonoBehaviour
{

    Vector3 raftPosition;
    Vector3 targetPosition;

    public float raftSpeed = 0.3f;

    void Start()
    {
        raftPosition = transform.position;
        targetPosition = new Vector3(raftPosition.x, raftPosition.y, raftPosition.z - 100);
    }

    void FixedUpdate()
    {
        transform.Translate(targetPosition.normalized * raftSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("XR Interaction Setup").transform.SetParent(gameObject.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("XR Interaction Setup").transform.SetParent(null);
    }
}
