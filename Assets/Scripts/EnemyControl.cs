using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    AutoAttack attackControl;
    private void Awake()
    {
        attackControl = GetComponent<AutoAttack>();
    }
    private void FixedUpdate()
    {
        attackControl?.doAttackOrder(attackControl.transform.position);
    }

}
