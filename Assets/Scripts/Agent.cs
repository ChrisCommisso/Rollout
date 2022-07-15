using System.Collections;
using System.Collections.Generic;
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
    public Vector3 FindDest(Vector3)
    // Update is called once per frame
    void Update()
    {
        
    }
}
