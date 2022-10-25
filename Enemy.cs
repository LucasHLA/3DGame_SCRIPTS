using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float totalHealth;
    public float currentHealth;
    public float attackDMG;
    public float moveSpeed;
    public float lookRadius = 10f;
    public float colliderRadius;
    public float enemyDMG = 25f;
    public bool playerIsAlive;

    private CapsuleCollider capsule;

    Animator anim;
    public Transform target;
    private NavMeshAgent agent;

    bool isReady;
    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent <NavMeshAgent>();
        capsule = GetComponent<CapsuleCollider>();

        currentHealth = totalHealth;
    }

     void Update()
    {
        if(currentHealth > 0)
        {

        
            float distance = Vector3.Distance(target.position,transform.position);
        

            if (distance <= lookRadius)
            {
                agent.isStopped = false;
                if (!anim.GetBool("Attacking"))
                {
                    agent.SetDestination(target.position);
                    anim.SetInteger("Transition", 2);
                    anim.SetBool("Walking", true);

                }
            


                if (distance <= agent.stoppingDistance)
                {
                    //ação de atacar 
                    StartCoroutine("attack");
                    lookTarget();
                }
                if (distance >= agent.stoppingDistance)
                {
                    anim.SetBool("Attacking", false);
                }
        }
        else
        {
            anim.SetInteger("Transition", 0);
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
            agent.isStopped = true;

        }

       }
   }

    IEnumerator attack()
    {

        if (!isReady && !playerIsAlive && !anim.GetBool("Hiting"))
        {
            isReady = true;
            anim.SetBool("Attacking", true);
            anim.SetBool("Walking", false);
            anim.SetInteger("Transition", 1);
            yield return new WaitForSeconds(1f);
            getPlayer();
           
            isReady = false;

        }
        if(!playerIsAlive)
        {
            anim.SetInteger("Transition", 0);
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
            agent.isStopped = true;
        }

    }

    void getPlayer()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Player>().getHIt(enemyDMG);
                playerIsAlive = c.gameObject.GetComponent<Player>().isAlive;
            }
        }
    }
    void lookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    public void getHIt(float dmg)
    {
       currentHealth -= dmg;

        if (currentHealth > 0)
        {
            StopCoroutine("attack");
            anim.SetInteger("Transition", 3);
            anim.SetBool("Hiting", true);
            StartCoroutine(recoveryFromHit());

        }
        else
        {
           anim.SetInteger("Transition", 4);
           capsule.enabled = false;
          

        }


        }

        IEnumerator recoveryFromHit()
        {
            yield return new WaitForSeconds(1f);
            anim.SetInteger("Transition", 0);
            anim.SetBool("Hiting", false);
            isReady = false;
         }
    }

