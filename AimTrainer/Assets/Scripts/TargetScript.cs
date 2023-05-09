using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void kill()
    {
        dead = true;
        Debug.Log("Killing");
    }

    public bool isDead()
    {
        return dead;
    }
}
