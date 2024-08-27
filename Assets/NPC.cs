using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPCState { Idle, Wander, Swimming, GoToPlayer }
[RequireComponent(typeof(NavMeshAgent))]
public class NPC : Savebel
{
    [HideInInspector] public Vector3 HomePoint;

    public float health;

    public NPCState state;

    NavMeshAgent agent;
    float IdleTimer;

    //[SerializeField] float maxViewDistance = 25f;
    [SerializeField] LayerMask viewLayer;
    [SerializeField] float WanderDistance = 10;
    // Start is called before the first frame update
    public NPC(Savebel savebel) : base(savebel)
    {
        
    }

    void Start()
    {
        HomePoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NavDistance(transform.position, agent.pathEndPosition) < 0.25 || (agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            //print("reached point {"+ NavDistance(transform.position, agent.destination) + "}");
            switch (state)
            {
                case NPCState.Idle:
                    //wait
                    if (IdleTimer > 0)
                        IdleTimer -= Time.deltaTime;
                    else
                    {
                        state = NPCState.Wander;
                        IdleTimer = Random.Range(0.2f, 10f);
                    }
                    break;
                case NPCState.Wander:
                    //New Destynation
                    if (Random.value > 0.25)
                        agent.SetDestination(PosToGround(HomePoint + randomVector3(new Vector3(-WanderDistance, 0, -WanderDistance), new Vector3(WanderDistance, 0, WanderDistance))));
                    else
                        state = NPCState.Idle;
                    break;
                case NPCState.Swimming:
                    //Next Destynation
                    break;
            }
        }
    }
    public float NavDistance(Vector3 posA, Vector3 posB)
    {
        return Vector3.Distance(new Vector3(posA.x, 0, posA.z), new Vector3(posB.x, 0, posB.z));
    }
    public Vector3 randomVector3(int x, int y, int z)
    {
        return randomVector3(Vector3.zero, new Vector3(x, y, z));
    }
    public Vector3 randomVector3(Vector3 vectorMin, Vector3 vectorMax)
    {
        return new Vector3(Random.Range(vectorMin.x, vectorMax.x), Random.Range(vectorMin.y, vectorMax.y), Random.Range(vectorMin.z, vectorMax.z));
    }
    public Vector3 PosToGround(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out hit, 50))
        {
            return hit.point;
        }
        return pos;
    }
    public override void Load(Savebel savebel)
    {
        NPC save = (NPC)savebel;
        health = save.health;
        HomePoint = save.HomePoint;
        state = save.state;

        transform.position = save.transform.position;
    }
}
