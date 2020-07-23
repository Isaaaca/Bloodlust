using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    Collider2D col2D = null;
    // Start is called before the first frame update
    void Start()
    {
        col2D = GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            col2D.isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
