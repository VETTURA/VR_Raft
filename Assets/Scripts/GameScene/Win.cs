using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Raft stage 0" || other.name == "Raft stage 1" || other.name == "Raft stage 2")
        {
            Conductor.ShowScene(Conductor.Scenes.MainMenu);
        }
    }
}
