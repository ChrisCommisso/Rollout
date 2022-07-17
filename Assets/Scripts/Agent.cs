using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Agent : Units
{
    public bool attacking;
    public float minimumFriendlyDistance;
    public static List<Agent> agents;
    public bool debugmode = true;
    public NavMeshAgent myAgent;
    public AutoAttack attackComponent;
    public bool isSelected;
    public GameObject SelectionLight;

    // Start is called before the first frame update
    public void Awake()
    {
        attackComponent = GetComponent<AutoAttack>();
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
        if (NavMesh.SamplePosition(dest, out hit, 2f, NavMesh.AllAreas))
        {
            myAgent.SetDestination(hit.position);
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
      
        attackComponent?.unitController?.SetFloat("Movespeed",myAgent.velocity.magnitude);
        
        if(agents!=null)
        foreach (var agent in agents)
        {
            if (agent == this || agent.allegiance!=allegiance) {
                    continue;
            }
            if ((agent.location - location).sqrMagnitude < minimumFriendlyDistance * minimumFriendlyDistance||attacking){
                    myAgent.isStopped = true;
            }
            else if (myAgent.isStopped) {
                    myAgent.isStopped = false;
            }
        }

        if (isSelected)
        {
            if (SelectionLight != null)
                SelectionLight.SetActive(true);
            else
            {
                SelectionLight = Instantiate(Resources.Load("SelectionLight") as GameObject);
                SelectionLight.transform.parent = transform;
                SelectionLight.transform.position = transform.position + Vector3.up * 2F;
            }
        }
        else
            SelectionLight.SetActive(false);

    }
}
