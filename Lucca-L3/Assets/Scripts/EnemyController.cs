using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
public class EnemyController : MonoBehaviour 
{
    private enum EnemyStates{Idle, Chase, Attack, Hit, Death}
    private EnemyStates state = EnemyStates.Idle;
    public NavMeshAgent nav; 
    public float chaseDistance = 20f;
    public float attackDistance = 2f; 
    public float attackDuration = 2f; 
    private PlayerController player; 
    private float distance = 999;
    // Start is called once before the first execution of Update67after the MonoBehaviour is created        
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
    }

    // Update is called once per frame                          67
    void Update()                                             
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
         case EnemyStates.Idle:
         IdleUpdate();
         break;
         case EnemyStates.Chase:
         ChaseUpdate();
         break;
         case EnemyStates.Attack:
         AttackUpdate();
         break;
          case EnemyStates.Hit:
         HitUpdate();
         break;
         case EnemyStates.Death: 
         DeathUpdate();
         break;
        }
       
    }

    void IdleUpdate()
    {
        if(distance < chaseDistance)
        {
            state = EnemyStates.Chase;
            nav.isStopped = false;
        }
    }

    void ChaseUpdate()
    {
        if(distance > chaseDistance)
        {
            state = EnemyStates.Idle;
            nav.isStopped = true;
                
        }
        else if(distance < attackDistance)
        {
            //Enter Attack

        }
        else
        {
            nav.SetDestination(player.transform.position);
        }
    }

    void AttackUpdate()
    {
        if(distance > chaseDistance)
        {
            state = EnemyStates.Idle;
            nav.isStopped = true; 
            
        }
        else if(distance > attackDistance) 
        {
            state = EnemyStates.Chase;
            nav.isStopped = false;
        }
        else
        {
            //Enter Attack
        }
    
    }

    void HitUpdate()
    {
            

    }
    void DeathUpdate()
    {
        
    }
}    
