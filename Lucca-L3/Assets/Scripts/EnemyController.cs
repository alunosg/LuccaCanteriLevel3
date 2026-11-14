using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
public class EnemyController : MonoBehaviour 
{
    private enum EnemyStates{Idle, Chase, Attack, Hit, Death}
    private EnemyStates state = EnemyStates.Idle;
    public NavMeshAgent nav; 
    public Animator anim;
    public float chaseDistance = 20f;
    public float attackDistance = 2f; 
    public float attackDuration = 2f;

    public Transform attackPoint;
    public float attackRadius = 1.5f; 
    public float attackHitDelay = 0.75f;
    public float damage = 1;

    public float hp = 3; 
    public float deathDuration = 5; 

    public GameObject attackHitFx;
    public GameObject HitFx;
    public GameObject deathFx;

    public AudioSource audioSource;
    public AudioClip attackSFX;
    public AudioClip attackHitSFX;
    public AudioClip deathSFX;

     public AudioClip hitSFX;

    private PlayerController player; 
    private float distance = 999;
    private bool locked = false;
    // Start is called once before the first execution of Update67after the MonoBehaviour is created        
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
    }

    // Update is called once per frame                          67
    void Update()                                             
    {
        if(locked) return;

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
            anim.SetBool("IsRunning", true);
        }
    }

    void ChaseUpdate()
    {
        if(distance > chaseDistance)
        {
            state = EnemyStates.Idle;
            nav.isStopped = true;
            anim.SetBool("IsRunning", false);
        }
        else if(distance < attackDistance)
        {
            EnterAttack();
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
            anim.SetBool("IsRunning", false);
        }
        else if(distance > attackDistance) 
        {
            state = EnemyStates.Chase;
            nav.isStopped = false;
            anim.SetBool("IsRunning", true);
        }
        else
        {
            EnterAttack();
        }
    
    }

    void HitUpdate()
    {
            

    }
    void DeathUpdate()
    {
        
    }

    void Unlock() => locked = false;

    void EnterAttack()
    {
        locked = true;
        nav.isStopped = true; 
        anim.SetTrigger("Attack");
        anim.SetBool("IsRunning", false);
        state = EnemyStates.Attack;

        if (attackSFX) audioSource.PlayOneShot(attackSFX); 

        CancelInvoke("Unlock");       
        Invoke("Unlock", attackDuration);

        CancelInvoke("Unlock");
        Invoke("Unlock", attackDuration);

        CancelInvoke("DealDamage");
        Invoke("DealDamage", attackHitDelay);
    }

    void DealDamage()
    {
        
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius);
        for(int i = 0; i<hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                if (attackHitSFX) audioSource.PlayOneShot(attackHitSFX); 
                if (attackHitFx) Instantiate(attackHitSFX, attackPoint. position, attackPoint.rotation);

                player.GetHit(damage);

            }
        }
    }
    public void GetHit(float damage)
    {
        if (hp > 0)
        {
            hp -= damage; 

            if (HitFx) Instantiate(HitFx,transform.position, transform.rotation);
            if (hitSFX) audioSource.PlayOneShot(hitSFX);

            nav.isStopped = true;

            if (hp > 0)
            {
                anim.SetTrigger("Hit"); 
            }
            else
            {
                anim.SetTrigger("Death");
                if (deathSFX) audioSource.PlayOneShot(deathSFX); 
                
                locked =  true; 
                CancelInvoke("AutoDestroy");
                Invoke("AutoDestroy", deathDuration);
            }

            anim.SetBool("IsRunning",false); 
            state = EnemyStates.Idle; 
        }
    }

     private void AutoDestroy() => Destroy(gameObject);
}    

