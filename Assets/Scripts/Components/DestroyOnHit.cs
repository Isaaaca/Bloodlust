using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool destroy = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (animator != null)
        {
            animator.SetBool("Destroyed",true);
        }
        else
        {
            OnDestroyAnimFinish();
        }
    }

    public void OnDestroyAnimFinish()
    {
        if (destroy)
            Destroy(this.gameObject);
        else
            this.gameObject.SetActive(false);
    }
}
