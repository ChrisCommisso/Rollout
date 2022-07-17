using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public int diceNum;
    public Animator unitController;
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
        print("attacking");
        agentRef.attacking = true;
        agentRef.setDestIfOnNavMesh(target.transform.position);
        unitController.SetTrigger("Attack");
        yield return new WaitForSeconds(attackdelay1);
        target.gameObject.GetComponent<AutoAttack>().doAttackOrder(GetComponent<Attackable>());
        int dmg = Random.Range(1, diceNum + 1);
        target.processDamage(dmg);
        DamageNumbers.Instance.CreateNumber(target.transform.position,1f,dmg);
        yield return new WaitForSeconds(attackdelay2);
        agentRef.attacking = false;
        
    }
    // Start is called before the first frame update
    void Awake()
    {
        

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
        currentAttackType = AttackType.proximity;
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
                    if (agent == null) {
                        continue;
                    }
                    if (agent.allegiance == agentRef.allegiance) {
                        continue;
                    }
                    if ((agent.transform.position - transform.position).sqrMagnitude < attackRange * attackRange&&agentRef?.attacking!=true) {
                        StartCoroutine(DoAttack(agent.gameObject.GetComponent<Attackable>()));
                        break;
                    }
                }    
                break;
            case AttackType.target:
                agentRef.setDestIfOnNavMesh(target.transform.position);
                if ((target.transform.position - agentRef.transform.position).sqrMagnitude < attackRange * attackRange&&agentRef?.attacking != true) 
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
