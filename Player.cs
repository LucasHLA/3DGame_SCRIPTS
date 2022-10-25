using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    public float rotation;
    public float gravity;
    public float colliderRadius;
    public float dmg = 25f;
    public float totalHealth;
    public float currentHealth;

    Vector3 moveDirection;
    CharacterController controller;
    Animator anim;

    bool isReady;
    public bool isAlive;
    List<Transform> enemiesList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentHealth = totalHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        getMouseInput();
    }


    void Move()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (!anim.GetBool("Attacking"))
                {
                    anim.SetBool("Walking", true);
                    anim.SetInteger("Transition", 1);
                    moveDirection = Vector3.forward * speed;
                    moveDirection = transform.TransformDirection(moveDirection);
                }
                else
                {
                    anim.SetBool("Walking", false);
                    moveDirection = Vector3.zero;
                    StartCoroutine("attack");

                }

            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("Walking", false);
                anim.SetInteger("Transition", 0);
                moveDirection = Vector3.zero;
            }
        }
        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void getMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.GetBool("Walking"))
                {
                    anim.SetBool("Walking", false);
                    anim.SetInteger("Transition", 0);
                }
                else if (!anim.GetBool("Walking"))
                {
                    //attack aqui 
                    StartCoroutine("attack");
                }
            }
        }
    }

    IEnumerator attack()
    {

        if (!isReady && !anim.GetBool("Hiting"))
        {
            isReady = true;
            anim.SetBool("Attacking", true);
            anim.SetInteger("Transition", 2);

            yield return new WaitForSeconds(0.5f);

                    getEnemiesRange();

            foreach (Transform enemies in enemiesList)
            {
                //executar ação de dano no inimigo
                Enemy enemy = enemies.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.getHIt(dmg);
                }
                
            }

            yield return new WaitForSeconds(0.8f);
            anim.SetInteger("Transition", 0);
            anim.SetBool("Attacking", false);
            isReady = false;

        }
   
    }

    void getEnemiesRange()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemiesList.Add(c.transform);
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }

    public void getHIt(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth > 0)
        {
            //toma hit aqui 
            anim.SetInteger("Transition", 3);
            anim.SetBool("Hiting", true);
            StartCoroutine(recoveryFromHit());
        }
        else
        {
           //morre aqui 
            anim.SetInteger("Transition", 4);
            isAlive = false;
        }


    }

    IEnumerator recoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Transition", 0);
        anim.SetBool("Hiting", false);
        isReady = false;
        anim.SetBool("Attacking", false);
    }
}