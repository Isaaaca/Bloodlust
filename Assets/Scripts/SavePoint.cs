using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : AreaTrigger
{
    public static event Action<Vector2> OnEnterSavePoint = (savePointPos) => { };

    [SerializeField] Animator animator = null;
    private bool activated = false;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            base.OnTriggerEnter2D(collision);
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                OnEnterSavePoint(this.transform.position);
                activated = true;
                if(animator!=null) animator.SetBool("Activated", activated);
                SaveManager.playerSpawnPoint = this.transform.position;
                SaveManager.curruntLevelSceneCode = SceneManager.GetActiveScene().buildIndex;
                player.health.Set(player.health.GetMax());
            }
        }
        
    }
}
