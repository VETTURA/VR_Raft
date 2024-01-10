using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shark : MonoBehaviour
{

    private System.Random random = new();

    private enum SharkState
    {
        Hunting,
        Attack,
        RunAway,
        Starting
    }

    private SharkState sharkState = SharkState.Starting;

    [SerializeField]
    private float angularSpeed;
    
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Quaternion startRotation;

    [SerializeField]
    private Vector3 runAwayPoint;

    [SerializeField]
    private float attackChance = 0.3f;

    [SerializeField]
    private float huntingChance = 0.3f;

    [SerializeField]
    private float uppdateTime = 3.0f;

    private List<GameObject> attackPoints;

    private Raft raft;

    private GameObject jaws;

    private GameObject nearestPoint;

    void Start()
    {
        startPosition = transform.localPosition;

        startRotation = transform.localRotation;
        
        runAwayPoint = new(0, -1.5f, 0);

        raft = FindObjectOfType<Raft>();

        jaws = GameObject.Find("Jaws");

        attackPoints = GameObject.FindGameObjectsWithTag("AttackPoint").ToList();
        StartCoroutine(GenerateChance());
    }

   
    void Update()
    {
        var deltaTime = Time.deltaTime;

        switch (sharkState)
        {
            case SharkState.Starting:
                StartingPosition();
                break;
            case SharkState.Hunting:
                Hunting(deltaTime);
                break;
            case SharkState.Attack:
                Attack(deltaTime);
                break;
            case SharkState.RunAway: 
                RunAway(deltaTime);
                break;
        }
    }
    
    public void CalculateNearestPoint()
    {
        float minDistance = 100.0f;

        foreach (var attackPoint in attackPoints)
        {
            float distance = Vector3.Distance(jaws.transform.position, attackPoint.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = attackPoint;
            }
        }
    }

    //Сброс в начальную позицию для охоты
    public void StartingPosition()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    //Если акула находится в "StartingPosition", то с некоторой вероятностью начать охоту
    public void Hunting(float deltaTime)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, -0.4f, transform.localPosition.z), speed * deltaTime);
        transform.RotateAround(raft.transform.localPosition, Vector3.up, angularSpeed * deltaTime);
    }


    //Если акула находится в состоянии "Hunting", то с некоторой вероятностью начать атаку
    public void Attack(float deltaTime)
    {
        if (nearestPoint == null)
        {
            CalculateNearestPoint();
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nearestPoint.transform.localPosition, speed * deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(raft.transform.position - transform.position), rotationSpeed * deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(raft.transform.position.x, raft.transform.position.y + 1f, raft.transform.position.z) - transform.position), rotationSpeed * deltaTime);


        //if (transform.localPosition == nearestPoint.transform.localPosition)
        //{
        //    transform.rotation = Quaternion.Euler(-12f, 0, 0);
        //}

    }

    //Ессли игрок нанес акуле урон во время состояния "Attack", то она переходит в состояние "RunAway"
    public void RunAway(float deltaTime)
    {
        if (nearestPoint != null)
        {
            nearestPoint = null;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(runAwayPoint - transform.localPosition), rotationSpeed * deltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, runAwayPoint, speed * deltaTime);

        if (transform.localPosition == runAwayPoint)
        {
            sharkState = SharkState.Starting;
        }
    }

    IEnumerator GenerateChance()
    {
        while (true)
        {
            var chance = random.NextDouble();
            Debug.Log("Chance: " + chance);
            
            if (attackChance > chance && sharkState == SharkState.Hunting)
            {
                Debug.Log("Shark state Attack by chance " + chance);
                sharkState = SharkState.Attack;
            }

            if (huntingChance > chance && sharkState == SharkState.Starting)
            {
                Debug.Log("Shark state Hunting by chance " + chance);
                sharkState = SharkState.Hunting;
            }

            yield return new WaitForSeconds(uppdateTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Rock")
        {
            sharkState = SharkState.RunAway;
        }
    }
}



/*
        if (starting && hunting != true && attack != true && runaway != true)
        {
            if (nearestPoint != null)
            {
                nearestPoint = null;
            }
            transform.localPosition = startPosition;
            transform.localRotation = startRotation;
        }

        if (hunting && starting == false)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, -0.17f, transform.localPosition.z), speed * Time.deltaTime);
            transform.RotateAround(raft.transform.localPosition, Vector3.up, angularSpeed * Time.deltaTime);
        }

        if (attack && hunting != true)
        {
            if (nearestPoint == null)
            {
                CalculateNearestPoint();
            }
            Debug.Log(raft.transform.localPosition);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nearestPoint.transform.localPosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(raft.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        }

        if (runaway && hunting != true)
        {
            if (nearestPoint != null)
            {
                nearestPoint = null;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(runAwayPoint - transform.localPosition), rotationSpeed * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, runAwayPoint, speed * Time.deltaTime);
        }

        */
