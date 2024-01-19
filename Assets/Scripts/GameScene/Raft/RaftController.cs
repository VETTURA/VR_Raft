using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RaftController : MonoBehaviour
{
    [SerializeField]
    public GameObject targetPoint;

    [SerializeField]
    public GameObject raftMainObject;

    [SerializeField]
    public GameObject stage0;

    [SerializeField]
    public GameObject stage1;

    [SerializeField]
    public GameObject stage2;

    private GameObject currentStage;

    private Player player;

    [SerializeField]
    private float _raftHealth = 100.0f;

    public float RaftHealth
    {
        get => _raftHealth;
        set { 
            _raftHealth = value; 
        }
    }

    [SerializeField]
    private float _raftSpeed = 0.1f;

    public float RaftSpeed
    {
        get => _raftSpeed;
        set
        {
            _raftSpeed = value;
        }
    }

    [SerializeField]
    private bool _isMoving = true;

    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            _isMoving = value;
        }
    }

    enum TurnSide
    {
        Left,
        Right,
    }

    void Start()
    {
        player = FindAnyObjectByType<Player>();

        currentStage = stage0;

        Turn(TurnSide.Left, 60);
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        MoveRaft(deltaTime);
        HealthCheck();
    }

    public void DamageRaft(float damageValue)
    {
        RaftHealth -= damageValue;
        Debug.Log($"Raft HP: {RaftHealth}%");
    }

    private void HealthCheck()
    {
        if (RaftHealth < 60 && currentStage.name == stage0.name)
        {
            ChangeState(stage0, stage1);
        }

        if (RaftHealth < 30 && currentStage.name == stage1.name)
        {
            ChangeState(stage1, stage2);
        }
    }

    public void ChangeState(GameObject oldStage, GameObject newStage)
    {
        currentStage = newStage;

        newStage.SetActive(true);

        player.transform.parent.SetParent(newStage.transform);

        List<GameObject> interactObjects = new();

        foreach(Transform child in oldStage.transform)
        {
            if(child.tag == RaftCollision.INTERACTABLEITEMTAG)
            {
                interactObjects.Add(child.gameObject);
            }
        }

        foreach (var elem in interactObjects)
        {
            elem.transform.SetParent(newStage.transform);
        }

        oldStage.SetActive(false);
    }

    private void MoveRaft(float deltaTime)
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.transform.position, RaftSpeed * deltaTime);
        }
    }

    private void Turn(TurnSide side, float degrees)
    {
        var turnDegress = side == TurnSide.Left ? -degrees : degrees;
        transform.RotateAround(raftMainObject.transform.localPosition, Vector3.up, turnDegress);
    }
}
