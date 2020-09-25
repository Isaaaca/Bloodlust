using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffProjectile : Projectile
{
    [SerializeField]private Animator animator = null;
    
    public float duration = 2f;
    private float timer = 0f;

    protected override void Start() { }

    public void Fire(Vector2 startPos, Vector2 dir)
    {
        rb2d.position = startPos;
        velocity = Vector2.Scale(dir,initialVelocity);
        timer = duration;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
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
        }
    }

    private void OnDestroyAnimFinish()
    {
        this.gameObject.SetActive(false);
    }
}
