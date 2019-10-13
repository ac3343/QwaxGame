using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    enum EnemyStates
    {
        Standing,
        Walking,
        Attacking,
        Hit,
        Death
    }

    public GameObject player;
    public Vector3 position;
    public float speed;
    public int enemyHealth;                 //In inspector
    public float attackRadius;

    EnemyStates currentState;
    EnemyStates previousState;

    public Rect collision;
    bool gettingHit;
    bool isDead;

    int currentAttackFrame;
    Vector2 attackCollisionSize = new Vector2(2, 2);
    public Rect attackCollisionBox;

    ///Death fields
    Death deathState;
    public int deathFrames;

    //Animation
    public Animator enemyAnimation;

    Attack enemyAttack;

    //Properties
    public bool GettingHit
    {
        get { return gettingHit; }
        set { gettingHit = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyStates.Standing;
        position = new Vector3(0, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 collisionPosition = new Vector2(position.x - .5f, position.y - .5f);
        collision = new Rect(collisionPosition, new Vector2(1,1));

        currentAttackFrame = 0;
        gettingHit = false;

        enemyAttack = new Attack();
        enemyAttack.GiveFrames(30, 30, 30);
        attackRadius = 2.5f;

        attackCollisionBox = Rect.zero;
    }

    // Update is called once per frame
    void Update()
    {
        previousState = currentState;
        position = transform.position;
        switch(currentState)
        {
            case EnemyStates.Standing:
                if (IsPlayerClose())
                    currentState = EnemyStates.Attacking;
                break;
            case EnemyStates.Attacking:
                if (!enemyAttack.AttackFinished(currentAttackFrame))
                {
                    if(enemyAttack.currentAttackState == Attack.AttackStates.Attack)
                    {
                        attackCollisionBox = new Rect(new Vector2(position.x - attackCollisionSize.x , position.y - attackCollisionSize.y / 2), attackCollisionSize);
                    }
                    else
                    {
                        attackCollisionBox = Rect.zero;
                    }

                    currentAttackFrame++;
                }
                else
                {
                    currentAttackFrame = 0;
                    currentState = EnemyStates.Standing;
                    attackCollisionBox = Rect.zero;
                }
                break;
            case EnemyStates.Death:
                break;
            case EnemyStates.Hit:
                break;
            default:
                Debug.Log("Error! Enemy state unknown!");
                attackCollisionBox = Rect.zero;
                break;
        }

        if (previousState != currentState)
        {
            if (currentState == EnemyStates.Standing)
                enemyAnimation.Play("Idle", 0, 0);
            if (currentState == EnemyStates.Attacking)
                enemyAnimation.Play("Attacking", 0, 0);
        }

        Vector2 collisionPosition = new Vector2(position.x - .5f, position.y - .5f);
        //attackCollisionBox = new Rect(new Vector2(position.x, position.y - 1), attackCollisionSize);

        collision.position = collisionPosition;
        transform.position = position;

        DrawRectangle(collision);
        DrawRectangle(attackCollisionBox);

        Debug.Log("Current state" + currentState);
    }

    void WalkEnemy()
    {
        position = transform.position;

        position += Vector3.Normalize(player.transform.position - position) * speed;

        collision.position = position;

        transform.position = position;
    }
    

    public void LoseHealth()
    {
        //Debug.Log("Lost health runs track");
        enemyHealth--;

        if(enemyHealth <= 0)
        {
            isDead = true;
        }
    }

    bool IsPlayerClose()
    {
        float distanceFromPlayer = player.transform.position.x - position.x;
        distanceFromPlayer = Mathf.Abs(distanceFromPlayer);
        return distanceFromPlayer <= attackRadius;
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
}
