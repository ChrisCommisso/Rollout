using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float attackdelay1;//delay before damage
    public float attackdelay2;//delay after damage
    public float attackRange;//attack range
    public Attackable target;
    public enum AttackType { 
    proximity,
    target,
    none
    }
    public AttackType currentAttackType;//change the attack type to set the 
    Agent agentRef;
    IEnumerator DoAttack(Attackable target) {
        agentRef.attacking = true;
        yield return new WaitForSecondsRealtime(attackdelay1);
        target.processDamage(Random.Range(1,7));
        yield return new WaitForSecondsRealtime(attackdelay2);
        agentRef.attacking = false;
    }
    // Start is called before the first frame update
    void Awake()
    {
        currentAttackType = AttackType.none;
        agentRef = GetComponent<Agent>();
    }
    public void doAttackOrder(Attackable attackable) {//target an enemy
        target = attackable;
        currentAttackType = AttackType.target;
    }
    public bool doAttackOrder(Vector3 location) {//do a proximity attack order
        currentAttackType = AttackType.proximity;
        if (agentRef.setDestIfOnNavMesh(location)) { 
        return true;
        }
        currentAttackType = AttackType.none;
        return false;
    }
    public bool noAttackMove(Vector3 location) {//regular movement
        currentAttackType = AttackType.none;
        if (agentRef.setDestIfOnNavMesh(location))
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null && currentAttackType == AttackType.target)
        {
            currentAttackType = AttackType.proximity;
        }
        else if(target!=null) 
            if(target.gameObject.GetComponent<Attackable>().Health <= 0) {
            currentAttackType = AttackType.proximity;
        }
        switch (currentAttackType)
        {
            case AttackType.proximity:
                foreach (Agent agent in Agent.agents) {
                    if (agent.allegiance == agentRef.allegiance) {
                        continue;
                    }
                    if ((agent.transform.position - transform.position).sqrMagnitude < attackRange * attackRange) {
                        StartCoroutine(DoAttack(agent.gameObject.GetComponent<Attackable>()));
                        break;
                    }
                }    
                break;
            case AttackType.target:
                agentRef.setDestIfOnNavMesh(target.transform.position);
                if ((target.transform.position - agentRef.transform.position).sqrMagnitude < attackRange * attackRange) 
                {
                    StartCoroutine(DoAttack(target));
                }
                break;
            case AttackType.none:
                break;
            default:
                break;
        }
    }
}
