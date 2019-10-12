using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerStates
    {
        Standing,
        Walking, 
        Attacking
    }

    //Fields
    public float speed;
    PlayerStates currentState = PlayerStates.Standing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InputUpdate()
    {
        Vector3 position = transform.position;

        switch (currentState)
        {

        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x -= speed;
        }

        position = transform.position;
    }
}
