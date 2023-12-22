using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Raft")
        {
            Conductor.ShowScene(Conductor.Scenes.MainMenu);
        }
    }
}
