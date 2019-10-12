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
    public KeyCode attackButton;
    PlayerStates currentState = PlayerStates.Standing;
    Attack[] swings;
    int currentSwing;
    int currentAttackFrame;
    

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0, 0, 0);
        swings = new Attack[3];
        currentSwing = 0;
        currentAttackFrame = 0;
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
            case PlayerStates.Standing:
                break;
            case PlayerStates.Walking:
                break;
            case PlayerStates.Attacking:
                if (swings[currentSwing].AttackFinished(currentAttackFrame))
                {
                    currentAttackFrame = 0;
                    currentSwing = 0;
                }
                else if(swings[currentSwing].currentAttackState == Attack.AttackStates.CoolDown && Input.GetKeyDown(attackButton) && currentSwing < 2)
                {
                    currentSwing++;
                    currentAttackFrame = 0;
                }
                else if(swings[currentSwing].currentAttackState == Attack.AttackStates.Attack)
                {

                }
                break;
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
