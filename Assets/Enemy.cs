using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Idle, Wander, Searching, Chase, Swimming}
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Savebel
{
    [HideInInspector] public Vector3 HomePoint;
    float searchingTimer = 0;
    [HideInInspector] public Vector3 searchingPoint;
    public EnemyState state;
    NavMeshAgent agent;
    [SerializeField] float maxViewDistance = 25f;
    [SerializeField] LayerMask viewLayer;
    [SerializeField] float WanderAndSearchDistance = 10;

    public Enemy(Savebel savebel) : base(savebel)
    {

    }



    // Start is called before the first frame update
    void Start()
    {
        HomePoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(PlayerController.controller.transform.position, transform.position) < maxViewDistance)
        {
            Debug.DrawLine(transform.position + Vector3.up * 1.5f, Camera.main.transform.position, Color.white);
            if (!Physics.Linecast(transform.position + Vector3.up * 1.5f, Camera.main.transform.position, viewLayer))
            {
                state = EnemyState.Chase;
                agent.SetDestination(PlayerController.controller.transform.position);
            }
        }
        if(state == EnemyState.Searching && searchingTimer > 0)
        {
            searchingTimer -= Time.deltaTime;
        }

        //print(NavDistance(transform.position, agent.destination));
        
        //if (NavDistance(transform.position, agent.destination) < 0.25)
        if (NavDistance(transform.position, agent.pathEndPosition) < 0.25 || (agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            //print("reached point {"+ NavDistance(transform.position, agent.destination) + "}");
            switch (state)
            {
                case EnemyState.Idle:
                    //wait
                    break;
                case EnemyState.Wander:
                    //New Destynation
                    agent.SetDestination(PosToGround(HomePoint + randomVector3(new Vector3(-WanderAndSearchDistance, 0, -WanderAndSearchDistance), new Vector3(WanderAndSearchDistance, 0, WanderAndSearchDistance))));
                    break;
                case EnemyState.Chase:
                    //Go TO Searching
                    searchingPoint = transform.position;
                    state = EnemyState.Searching;
                    searchingTimer = 0;
                    break;
                case EnemyState.Searching:
                    //Next Destynation
                    if (searchingTimer > 0 && searchingTimer <= 0.5)
                    {
                        state = EnemyState.Wander;
                        searchingTimer = 100;
                    }
                    if (searchingTimer <= 0)
                        searchingTimer = Random.Range(15f, 60f);

                    print("next serching point");
                    agent.SetDestination(PosToGround(searchingPoint + randomVector3(new Vector3(-WanderAndSearchDistance, 0, -WanderAndSearchDistance), new Vector3(WanderAndSearchDistance, 0, WanderAndSearchDistance))));
                    break;
                case EnemyState.Swimming:
                    //Next Destynation
                    break;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(searchingPoint, 0.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(HomePoint, 0.25f);
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
        if(Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out hit, 50))
        {
            return hit.point;
        }
        return pos;
    }
    public override void Load(Savebel savebel)
    {
        Enemy save = (Enemy)savebel;
        state = save.state;
        HomePoint = save.HomePoint;
        searchingPoint = save.searchingPoint;

        transform.position = save.transform.position;
    }
}
