using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headEat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Apple")
        {
            Destroy(collision.gameObject);
            this.transform.parent.GetComponent<move>().addnewTail();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
