using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SailController : MonoBehaviour
{
    public Animator animator;

    private bool isFurled = false;
    private bool isInflated = false;

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
                animator.SetBool("IsWindy", false);
                animator.SetBool("IsFurled", true);
                isFurled = true;
            }
            else
            {
                animator.SetBool("IsFurled", false);
                isFurled = false;
            }
        }
    }

    public void checkWeather(bool isWindy)
    {
        if (isWindy == true)
        {
            animator.SetBool("IsWindy", true);
            isInflated = true;
        }
        else
        {
            animator.SetBool("IsWindy", false);
            isInflated = false;
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
