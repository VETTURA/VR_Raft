using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveItems : MonoBehaviour
{
    [SerializeField]
    private float _itemSpeed = 3.0f;

    public float ItemSpeed
    {
        get => _itemSpeed;
        set
        {
            _itemSpeed = value;
        }
    }

    [SerializeField]
    private bool _isMove = true;

    public bool IsMove
    {
        get => _isMove;
        set 
        { 
            _isMove = value; 
        }
    }

    [SerializeField]
    private float destroyDistance = 100.0f;

    private Raft raft;

    private Vector3 targetPosition;

    void Start()
    {
        raft = FindFirstObjectByType<Raft>();

        targetPosition = new(transform.position.x, raft.transform.position.y, transform.position.z + destroyDistance);
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        MoveItem(deltaTime);
    }

    private void MoveItem(float deltaTime)
    {
        if(IsMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, ItemSpeed * deltaTime);
        }

        if(transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }
}
