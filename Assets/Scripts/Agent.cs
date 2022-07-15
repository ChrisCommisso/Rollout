using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Agent : Units
{
    public static List<Agent> agents;
    public bool debugmode = true;
    NavMeshAgent myAgent;
    // Start is called before the first frame update
    void Start()
    {
        if (agents != null) 
        {
            agents = new List<Agent>();
        }
        myAgent.radius = (Width + Depth) / 2f;
        myAgent.height = Height;
    }

    private void OnDrawGizmos()
    {
        if(debugmode)
        Gizmos.DrawSphere(myAgent.transform.position,(Width + Depth) / 2f);
    }
    public bool setDestIfOnNavMesh(Vector3 dest){
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 1f, NavMesh.AllAreas))
        {
            myAgent.SetDestination(hit.position);
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
