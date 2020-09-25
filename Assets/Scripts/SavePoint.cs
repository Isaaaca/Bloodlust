using System;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : AreaTrigger
{
    public static event Action<Vector2> OnEnterSavePoint = (savePointPos) => { };

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            OnEnterSavePoint(this.transform.position);
            SaveManager.playerSpawnPoint = this.transform.position;
            player.health.Set(player.health.GetMax());
        }
    }
}
