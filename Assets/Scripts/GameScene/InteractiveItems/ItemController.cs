using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemController : MonoBehaviour
{
    private MoveItems moveItems;
    private XRGrabInteractable interactable;
    private Rigidbody rb;

    void Start()
    {
        moveItems = GetComponent<MoveItems>();
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        CheckSelectedItem();
    }

    private void CheckSelectedItem()
    {
        if (interactable.isSelected)
        {
            moveItems.enabled = false;
        }

        if(!interactable.isSelected && !moveItems.enabled)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }


    }
}
