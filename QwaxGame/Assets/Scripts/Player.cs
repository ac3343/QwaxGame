using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum PlayerStates
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

    Vector2 collisionBoxSize;
    int damagedFrames;
    int currentDamagedFrame;
    int invincibleFrames;
    bool isInvincible;

    public bool enabledControls = true;
    public bool isPoweredUp = false;

    //Properties
    public Rect CurrentAttackCollision
    {
        get { return attackCollisions[currentSwing]; }
    }

    public Rect CollisionBox
    {
        get { return new Rect(new Vector2(position.x - collisionBoxSize.x / 2, position.y - collisionBoxSize.y / 2), collisionBoxSize); }
    }
    public PlayerStates CurrentState
    {
        get { return currentState; }
    }
    public bool IsInvincible
    {
        get { return isInvincible; }
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
        swings[0].GiveFrames(10, 30, 20);

        //Second Swing
        swings[1] = new Attack();
        swings[1].GiveFrames(10, 30, 20);

        //Third Swing
        swings[2] = new Attack();
        swings[2].GiveFrames(10 , 30, 20);

        //Sets up attack collisions array
        attackCollisions = new Rect[3];

        //Sets attack data to it's default 
        currentSwing = 0;
        currentAttackFrame = 0;

        //Creates death animation information
        deathState = new Death();
        deathState.GiveFrames(deathFrames);

        collisionBoxSize = new Vector2(1, 1);

        currentDamagedFrame = 0;
        damagedFrames = 60;
        invincibleFrames = 120;
        isInvincible = false;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        oldState = currentState;
        oldDirection = currentDirection;

        if (isPoweredUp)
        {
            isInvincible = true;
        }

        if (enabledControls)
        {
            InputUpdate();

            if (oldState != currentState && !isPoweredUp)
            {
                if (currentState == PlayerStates.Standing)
                    anim.Play("PlayerIdle_1", 0, 0);
                if (currentState == PlayerStates.Walking && Input.GetKey(KeyCode.D))
                    anim.Play("PlayerWalk_Right", 0, 0);
                else if (currentState == PlayerStates.Walking && Input.GetKey(KeyCode.A))
                    anim.Play("PlayerWalk_Right", 0, 0);
                if (currentState == PlayerStates.Attacking)
                    anim.Play("PlayerAttack_1", 0, 0);
                if (currentState == PlayerStates.Damaged)
                    anim.Play("PlayerIdle_1", 0, 0);
            }
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
                if(SceneManager.GetActiveScene().name.Equals("FirstSlope"))
                {
                    BackgroundRotation();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    position.x -= speed;
                    
                    currentDirection = PlayerDirection.Left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    position.x += speed;
                    
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
                if(currentDamagedFrame > damagedFrames)
                {
                    currentState = PlayerStates.Standing;
                }
                else
                {
                    currentDamagedFrame++;
                }
                
                break;
            case PlayerStates.Death:
                break;
            default:
                //Debug.Log("Player State Unknown");
                break;

        }

        if(currentDamagedFrame > invincibleFrames + damagedFrames)
        {
            isInvincible = false;
            currentDamagedFrame = 0;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            sprite.color = Color.white;
        }
        else
        {
            currentDamagedFrame++;
        }

        DrawRectangle(CollisionBox);
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

    void BackgroundRotation()
    {
        float camRot = mainCamera.transform.GetChild(0).localEulerAngles.z;
        Vector3 camPos = mainCamera.transform.GetChild(0).transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            distanceTraveled -= speed;
            if (distanceTraveled > 10 && distanceTraveled < 30)
            {
                Quaternion temp = mainCamera.transform.GetChild(0).localRotation;
                temp.z += 0.001f;
                mainCamera.transform.GetChild(0).localRotation = temp;
            }
            if (distanceTraveled > 30 && distanceTraveled < 50)
            {
                Debug.Log("going down");
                camPos.y += 0.003f;
                mainCamera.transform.GetChild(0).transform.position = camPos;
            }
        }
        else if(Input.GetKey(KeyCode.D))
        {
            distanceTraveled += speed;
            if (distanceTraveled > 10 && distanceTraveled < 30)
            {

                Quaternion temp = mainCamera.transform.GetChild(0).localRotation;
                temp.z -= 0.001f;
                mainCamera.transform.GetChild(0).localRotation = temp;
            }
            if (distanceTraveled > 30 && distanceTraveled < 50)
            {
                camPos.y -= 0.003f;
                mainCamera.transform.GetChild(0).transform.position = camPos;
            }
        }
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
            if(currentDirection == PlayerDirection.Right)
                attackCollisions[currentSwing] = new Rect(new Vector2(transform.position.x, transform.position.y - attackBoxes[currentSwing].y / 2), attackBoxes[currentSwing]);
            else if(currentDirection == PlayerDirection.Left)
                attackCollisions[currentSwing] = new Rect(new Vector2(transform.position.x - attackBoxes[currentSwing].x, transform.position.y - attackBoxes[currentSwing].y / 2), attackBoxes[currentSwing]);
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
        if(currentAttackFrame == 0 && !isPoweredUp)
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

    public void Dies()
    {
        currentState = PlayerStates.Death;
        anim.SetTrigger("Killed");
    }

    public void GetsDamaged()
    {
        currentState = PlayerStates.Damaged;

        isInvincible = true;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        sprite.color = Color.red;

        //Debug.Log("This code is running!");
    }

    public void OnDeathFinished()
    {
        gameObject.SetActive(false);
    }
}
