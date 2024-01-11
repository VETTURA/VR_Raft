using UnityEngine;

public class Raft : MonoBehaviour
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

    private Player player;

    private Vector3 targetPosition;

    public const string INTERACTABLEITEMTAG = "InteractableItem";

    void Start()
    {
        player = FindObjectOfType<Player>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.SetParent(gameObject.transform, true);
        }

        if (other.tag == INTERACTABLEITEMTAG)
        {
            other.transform.parent.SetParent(gameObject.transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.transform.SetParent(null, true);
        }

        if (other.tag == INTERACTABLEITEMTAG)
        {
            other.transform.parent.SetParent(null, true);
        }
    }
}
