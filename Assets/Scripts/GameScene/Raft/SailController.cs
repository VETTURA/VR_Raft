using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SailController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (animator.GetBool("IsFurled") == false)
            {
                if (animator.GetBool("IsWindy") == true)
                {
                    animator.SetBool("IsWindy", false);
                }
                animator.SetBool("IsFurled", true);
            }
            else
            {
                animator.SetBool("IsFurled", false);
            }
        }
    }

    /*public void SailAction()
    {
        if (animator.GetBool("IsFurled") == false)
        {
            animator.SetBool("IsFurled", true);
        }
        else
        {
            animator.SetBool("IsFurled", false);
        }
    }*/
}
