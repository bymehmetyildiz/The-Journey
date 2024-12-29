using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Statue : MonoBehaviour
{

    [SerializeField] private GameObject tmPro;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tmPro.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tmPro.SetActive(false);
        }
    }
}
