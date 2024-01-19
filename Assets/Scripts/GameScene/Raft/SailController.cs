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
                animator.SetBool("IsWindy", false);
                animator.SetBool("IsFurled", true);
            }
            else
            {
                animator.SetBool("IsFurled", false);
            }
        }
    }

    public void checkWeather(bool isWindy)
    {
        if (isWindy == true)
        {
            animator.SetBool("IsWindy", true);
        }
        else
        {
            animator.SetBool("IsWindy", false);
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
