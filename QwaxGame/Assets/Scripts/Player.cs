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
    public Vector3 position;
    public float speed;
    PlayerStates currentState = PlayerStates.Standing;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
    }

    void InputUpdate()
    {
        position = transform.position;

        switch (currentState)
        {

        }
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log(speed);
            position.x -= speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x += speed;
        }

        transform.position = position;
    }
}
