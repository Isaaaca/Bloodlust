using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test: MonoBehaviour
{
    public DialogueManager dm;
    public Dialogue d;
    public ScreenFader SF;
    public CutsceneDirector cd;
    public ScriptedEventSequence sequence;
    // Start is called before the first frame update
    private void Start()
    {
    }

    public static void poop()
    {
        Debug.Log("poop");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9)) SF.FadeToBlack();
        if (Input.GetKeyDown(KeyCode.Alpha0)) SF.FadeIn();
        if (Input.GetKeyDown(KeyCode.Alpha8)) cd.PlaySequence(sequence);
    }
    void OnDrawGizmosSelected()
    {
        float topBorder=1;
        Bounds bounds = new Bounds
        {
            size = Vector3.zero // reset
        };
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }
        bounds.center += Vector3.up * (topBorder/2);
        bounds.size += Vector3.up * topBorder;
        Gizmos.DrawCube(bounds.center, bounds.size);
    }
}
