using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerStates
    {
        Standing,
        Walking, 
        Attacking,
        Damaged, 
        Death
    }
    enum PlayerDirection
    {
        Right, 
        Left,
    }

    //Fields
    public Vector3 position;
    public float speed;                                     //Edit in inspector
    public KeyCode attackButton;
    PlayerStates currentState = PlayerStates.Standing;
    PlayerStates oldState = PlayerStates.Standing;
    PlayerDirection currentDirection = PlayerDirection.Right;
    PlayerDirection oldDirection = PlayerDirection.Right;
    public float distanceTraveled;
    public Camera mainCamera;

    public Animator anim;

    ///Attack Fields
    Attack[] swings;
    Rect[] attackCollisions;         //Communicates with 
    public Vector2[] attackBoxes;
    int currentSwing;
    int currentAttackFrame;

    ///Death fields
    Death deathState;
    public int deathFrames;

    //Properties
    public Rect CurrentAttackCollision
    {
        get { return attackCollisions[currentSwing]; }
    }

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0, 0, 0);
        distanceTraveled = 0f;

        anim = GetComponent<Animator>();

        //Creates swings
        swings = new Attack[3];
        //First Swing
        swings[0] = new Attack();
        swings[0].GiveFrames(5, 20, 10);

        //Second Swing
        swings[1] = new Attack();
        swings[1].GiveFrames(5, 20, 10);

        //Third Swing
        swings[2] = new Attack();
        swings[2].GiveFrames(5 , 20, 10);

        //Sets up attack collisions array
        attackCollisions = new Rect[3];

        //Sets attack data to it's default 
        currentSwing = 0;
        currentAttackFrame = 0;

        //Creates death animation information
        deathState = new Death();
        deathState.GiveFrames(deathFrames);
    }

    // Update is called once per frame
    void Update()
    {
        oldState = currentState;
        oldDirection = currentDirection;
        InputUpdate();

        if(oldState != currentState)
        {
            if (currentState == PlayerStates.Standing)
                anim.Play("PlayerIdle_1", 0, 0);
            if (currentState == PlayerStates.Walking && Input.GetKey(KeyCode.D))
                anim.Play("PlayerWalk_Right", 0, 0);
            else if (currentState == PlayerStates.Walking && Input.GetKey(KeyCode.A))
                anim.Play("PlayerWalk_Right", 0, 0);
            if (currentState == PlayerStates.Attacking)
                anim.Play("PlayerAttack_1", 0, 0);
        }

        if(oldDirection != currentDirection)
        {
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;
        }

    }

    void InputUpdate()
    {
        position = transform.position;

        switch (currentState)
        {
            case PlayerStates.Standing:
                if (Input.GetKey(KeyCode.A))
                {
                    currentState = PlayerStates.Walking;
                    currentDirection = PlayerDirection.Left;
                }
                else if(Input.GetKey(KeyCode.D))
                {
                    currentState = PlayerStates.Walking;
                    currentDirection = PlayerDirection.Right;
                }

                if (Input.GetKey(attackButton))
                {
                    currentState = PlayerStates.Attacking;
                }
                break;
            case PlayerStates.Walking:
                float camRot = mainCamera.transform.GetChild(0).localEulerAngles.z;
                Vector3 camPos = mainCamera.transform.GetChild(0).transform.position;
                Debug.Log(distanceTraveled);
                if (Input.GetKey(KeyCode.A))
                {
                    position.x -= speed;
                    distanceTraveled -= speed;
                    if (distanceTraveled > 5 && distanceTraveled < 10)
                    {
                        Quaternion temp = mainCamera.transform.GetChild(0).localRotation;
                        temp.z += 0.001f;
                        mainCamera.transform.GetChild(0).localRotation = temp;
                    }
                    if (distanceTraveled > 10 && distanceTraveled < 20)
                    {
                        Debug.Log("going down");
                        camPos.y += 0.001f;
                        mainCamera.transform.GetChild(0).transform.position = camPos;
                    }
                    currentDirection = PlayerDirection.Left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    position.x += speed;
                    distanceTraveled += speed;
                    if (distanceTraveled > 5 && distanceTraveled < 10)
                    {
                        
                        Quaternion temp = mainCamera.transform.GetChild(0).localRotation;
                        temp.z -= 0.001f;
                        mainCamera.transform.GetChild(0).localRotation = temp;
                    }
                    if(distanceTraveled > 10 && distanceTraveled < 20)
                    {
                        camPos.y -= 0.001f;
                        mainCamera.transform.GetChild(0).transform.position = camPos;
                    }
                    currentDirection = PlayerDirection.Right;
                }
                else
                {
                    currentState = PlayerStates.Standing;
                }

                if (Input.GetKey(attackButton))
                {
                    currentState = PlayerStates.Attacking;
                }
                break;

            case PlayerStates.Attacking:
                Attacking();
                break;
            case PlayerStates.Damaged:
                break;
            case PlayerStates.Death:
                break;
            default:
                Debug.Log("Player State Unknown");
                break;

        }
        transform.position = position;
    }

    void DrawRectangle(Rect size)
    {
        //Draws top
        Debug.DrawLine(new Vector3(size.xMin, size.yMax, 0), new Vector3(size.xMax, size.yMax, 0), Color.red);
        //Draws bottom
        Debug.DrawLine(new Vector3(size.xMin, size.yMin, 0), new Vector3(size.xMax, size.yMin, 0), Color.red);
        //Draws right
        Debug.DrawLine(new Vector3(size.xMax, size.yMin, 0), new Vector3(size.xMax, size.yMax, 0), Color.red);
        //Draws left
        Debug.DrawLine(new Vector3(size.xMin, size.yMin, 0), new Vector3(size.xMin, size.yMax, 0), Color.red);
    }

    void Attacking()
    {
        //
        if (swings[currentSwing].AttackFinished(currentAttackFrame))
        {
            attackCollisions[currentSwing] = Rect.zero;
            currentAttackFrame = 0;
            currentSwing = 0;
            currentState = PlayerStates.Standing;
        }
        //
        else if (swings[currentSwing].currentAttackState == Attack.AttackStates.CoolDown && Input.GetKeyDown(attackButton) && currentSwing < 2)
        {
            attackCollisions[currentSwing] = Rect.zero;
            currentSwing++;
            currentAttackFrame = 0;
        }
        //
        else if (swings[currentSwing].currentAttackState == Attack.AttackStates.Attack)
        {
            attackCollisions[currentSwing] = new Rect(new Vector2(transform.position.x, transform.position.y), attackBoxes[currentSwing]);
            currentAttackFrame++;

            DrawRectangle(attackCollisions[currentSwing]);
        }
        //
        else if (swings[currentSwing].currentAttackState == Attack.AttackStates.WindUp || swings[currentSwing].currentAttackState == Attack.AttackStates.CoolDown)
        {
            currentAttackFrame++;
            
        }
        //
        else
        {
            attackCollisions[currentSwing] = Rect.zero;
            currentAttackFrame = 0;
            currentSwing = 0;
            currentState = PlayerStates.Standing;

        }
        if(currentAttackFrame == 0)
        {
            if (currentSwing == 1)
            {
                anim.Play("PlayerAttack_2", 0, 0);
            }
            else if (currentSwing == 2)
            {
                anim.Play("PlayerAttack_3", 0, 0);
            }
        }
        
    }


}
