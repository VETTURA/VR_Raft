using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class RaftController : MonoBehaviour
{
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

    private Vector3 targetPosition;

    private WindController wind;
    private SailController sail;

    private float windFurledSpeed;
    private float furledSpeed;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        wind = FindAnyObjectByType<WindController>();
        sail = FindAnyObjectByType<SailController>();

        windFurledSpeed = RaftSpeed * 2.0f;
        furledSpeed = RaftSpeed * 1.5f;

        currentStage = stage0;

        targetPosition = new(transform.position.x, transform.position.y, transform.position.z - 1996);
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        CheckWind();
        MoveRaft(deltaTime);
        HealthCheck();
    }

    private void CheckWind()
    {
        if (!sail.isFurled && wind.isWindy)
        {
            RaftSpeed = windFurledSpeed;
        }
        else if (!sail.isFurled)
        {
            RaftSpeed = furledSpeed;
        }
        else
        {
            RaftSpeed = 1.0f;
        }
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

        sail = FindAnyObjectByType<SailController>();
        FindFirstObjectByType<WindController>().ChangeSail();
    }

    private void MoveRaft(float deltaTime)
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, RaftSpeed * deltaTime);
        }
    }
}
