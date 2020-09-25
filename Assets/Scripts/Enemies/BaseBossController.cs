using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossController : Character
{
    [Header("Character Settings")]
    [SerializeField] private float phase2TriggerPoint = 50;

    protected GameObject player;
    protected float timer = 1f;
    protected bool inCombatMode = true;

    private void Awake()
    {
        GameManager.SetGameplayEnabled += SetCombatEnabled;
    }

    private void SetCombatEnabled(bool isEnabled)
    {
        inCombatMode = isEnabled;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameManager.GetPlayer();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inCombatMode)
        {

            if (health.Get() > 0)
            {
                if (health.Get() > phase2TriggerPoint)
                {
                    Phase1();
                }
                else
                {
                    Phase2();
                }
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            base.Update();
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        sprite.enabled = true;
    }

    protected abstract void Phase1();
    protected abstract void Phase2();

    protected bool InMidAction()
    {
        return controller.IsArcing() || controller.IsDashing() || timer > 0;
    }
}
