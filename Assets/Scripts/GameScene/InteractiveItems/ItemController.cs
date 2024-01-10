using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private float immersionDistance = 0.5f;

    private MoveItems moveItems;
    private XRGrabInteractable interactable;
    private Rigidbody rb;

    private Water water;

    void Start()
    {
        moveItems = GetComponent<MoveItems>();
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        water = FindFirstObjectByType<Water>(); 
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        CheckSelectedItem();
        CheckWaterTouch();
    }

    private void CheckSelectedItem()
    {
        if (interactable.isSelected)
        {
            moveItems.IsMove = !interactable.isSelected;
        }

        if(!interactable.isSelected && !moveItems.IsMove)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void CheckWaterTouch()
    {
        if (!moveItems.IsMove)
        {
            var yPossitionWater = water.transform.position.y;
            var yPossitionItem = transform.position.y;

            if((yPossitionWater - yPossitionItem) > immersionDistance)
            {
                moveItems.UpMove = true;
                moveItems.IsMove = true;
                moveItems.ResetRotation();                
                moveItems.CalculateTargetPosition();

                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
}
