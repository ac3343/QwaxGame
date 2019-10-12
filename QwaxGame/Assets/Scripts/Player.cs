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
    public float speed;                                     //Edit in inspector
    public KeyCode attackButton;
    PlayerStates currentState = PlayerStates.Standing;

    ///Attack Fields
    Attack[] swings;
    Rect[] attackCollisions;
    public Rect[] attackBoxes;
    int currentSwing;
    int currentAttackFrame;
    

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0, 0, 0);
        swings = new Attack[3];
        attackCollisions = new Rect[3];
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
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    currentState = PlayerStates.Walking;
                }

                if (Input.GetKey(attackButton))
                {
                    currentState = PlayerStates.Attacking;
                }
                break;
            case PlayerStates.Walking:
                if (Input.GetKey(KeyCode.A))
                {
                    position.x -= speed;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    position.x += speed;
                }

                if (Input.GetKey(attackButton))
                {
                    currentState = PlayerStates.Attacking;
                }
                break;
            case PlayerStates.Attacking:
                if (swings[currentSwing].AttackFinished(currentAttackFrame))
                {
                    attackCollisions[currentSwing] = Rect.zero;
                    currentAttackFrame = 0;
                    currentSwing = 0;
                    
                }
                else if(swings[currentSwing].currentAttackState == Attack.AttackStates.CoolDown && Input.GetKeyDown(attackButton) && currentSwing < 2)
                {
                    attackCollisions[currentSwing] = Rect.zero;
                    currentSwing++;
                    currentAttackFrame = 0;
                }
                else if(swings[currentSwing].currentAttackState == Attack.AttackStates.Attack)
                {
                    attackCollisions[currentSwing] = attackBoxes[currentSwing];
                }
                else
                {
                    attackCollisions[currentSwing] = Rect.zero;
                    currentAttackFrame = 0;
                    currentSwing = 0;
                }
                break;
        }
        

        transform.position = position;
    }

    
}
