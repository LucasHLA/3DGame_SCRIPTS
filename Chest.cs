using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    public float colliderRadius;
    public bool isOpen;

    public List<Itens> itensList = new List<Itens>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        getPlayer();
    }

    void getPlayer()
    {
        if (!isOpen)
        {
            foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    if (Input.GetKeyDown("e"))
                    {
                        openChest();
                    }

                }
            }
        }
        
    }

    void openChest()
    {
        foreach(Itens i in itensList)
        {
            i.getAction();
        }

        anim.SetTrigger("Open");
        isOpen = true;
    }
}
