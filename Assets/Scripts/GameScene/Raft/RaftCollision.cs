using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaftCollision: MonoBehaviour
{
    private Player player;

    public const string INTERACTABLEITEMTAG = "InteractableItem";
    public const string INTERACTABLEITEMTAGCOLLIDER = "InteractableItemCollider";
    public const string ROCKTAG = "Rock";

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.SetParent(gameObject.transform, true);
        }

        if (other.tag == INTERACTABLEITEMTAGCOLLIDER)
        {
            if(other.transform.parent.GetComponent<ItemController>().InNat == false)
            {
                other.transform.parent.SetParent(gameObject.transform, true);
            }
        }

        if (other.tag == ROCKTAG)
        {
            Conductor.ShowScene(Conductor.Scenes.MainMenu);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.SetParent(null, true);
        }

        if (other.tag == INTERACTABLEITEMTAGCOLLIDER)
        {
            if(other.gameObject.transform.parent.GetComponent<ItemController>().InNat == false)
            {
                other.transform.parent.SetParent(null, true);
            }
        }
    }
}
