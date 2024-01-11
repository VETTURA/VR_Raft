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
        targetPosition = new(transform.position.x, transform.position.y, transform.position.z - 1900);
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

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
        if (RaftHealth < 60)
        {
            stage0.SetActive(false);
            stage1.SetActive(true);
        }

        if (RaftHealth < 30)
        {
            stage1.SetActive(false);
            stage2.SetActive(true);
        }
    }

    private void MoveRaft(float deltaTime)
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, RaftSpeed * deltaTime);
        }
    }
}
