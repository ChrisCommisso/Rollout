using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Agent : Units
{
    public float minimumFriendlyDistance;
    public static List<Agent> agents;
    public bool debugmode = true;
    public NavMeshAgent myAgent;
    // Start is called before the first frame update
    public void Awake()
    {
        if (agents == null) 
        {
            agents = new List<Agent>();
        }
        myAgent.radius = (Width + Depth) / 2f;
        myAgent.height = Height;
        agents.Add(this);
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
    void FixedUpdate()
    {
        if(agents!=null)
        foreach (var agent in agents)
        {
            if (agent == this || agent.allegiance!=allegiance) {
                    myAgent.isStopped = false;
                    print(myAgent.destination);
                    continue;
            }
            if ((agent.location - location).sqrMagnitude < minimumFriendlyDistance * minimumFriendlyDistance){
                    myAgent.isStopped = true;
            }
            else if (myAgent.isStopped) {
                    myAgent.isStopped = false;
            }
        }
       
    }
}
