using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    private Raft raft;

    private void Start()
    {
        raft = FindFirstObjectByType<Raft>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == raft.name)
        {
            Conductor.ShowScene(Conductor.Scenes.MainMenu);
        }
    }
}
