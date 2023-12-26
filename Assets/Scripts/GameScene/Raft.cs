using UnityEngine;

public class Raft : MonoBehaviour
{
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

    public const string INTEACTABLEITEMTAG = "InteractableItem";

    void Start()
    {
        player = FindObjectOfType<Player>();
        targetPosition = new(transform.position.x, transform.position.y, transform.position.z - 1900);
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        MoveRaft(deltaTime);
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

        if (other.tag == INTEACTABLEITEMTAG)
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

        if (other.tag == INTEACTABLEITEMTAG)
        {
            other.transform.parent.SetParent(null, true);
        }
    }
}
