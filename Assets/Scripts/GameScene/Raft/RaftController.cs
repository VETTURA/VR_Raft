using System.Collections;
using UnityEngine;

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

    //TODO переписать как только будет готова физическая вода
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

    void Start()
    {
        player = FindAnyObjectByType<Player>();

        currentStage = stage0;

        targetPosition = new(transform.position.x, transform.position.y, transform.position.z - 1996);
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

        oldStage.SetActive(false);
    }

    private void MoveRaft(float deltaTime)
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, RaftSpeed * deltaTime);
        }
    }
}
