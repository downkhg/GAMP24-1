using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dynamic dynamic = collision.GetComponent<Dynamic>();
        if (dynamic)
        {
            dynamic.score += score;
        }
    }
}
