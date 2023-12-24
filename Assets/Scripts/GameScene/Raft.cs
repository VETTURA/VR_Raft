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

    private Player player;

    Vector3 targetPosition;

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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, RaftSpeed * deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.SetParent(gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == player.name)
        {
            player.transform.parent.transform.SetParent(null);
        }
    }
}
