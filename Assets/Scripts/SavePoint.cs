using System;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : AreaTrigger
{
    public static event Action<Vector2> OnEnterSavePoint = (savePointPos) => { };

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        OnEnterSavePoint(this.transform.position);
        SaveManager.playerSpawnPoint = this.transform.position;
    }
}
