using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (animator != null)
        {
            animator.Play("Destroy");
        }
        else
        {
            OnDestroyAnimFinish();
        }
    }

    public void OnDestroyAnimFinish()
    {
        Destroy(this.gameObject);
    }
}
