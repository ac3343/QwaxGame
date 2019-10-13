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

    EnemyStates currentState;
    EnemyStates previousState;

    public Rect collision;
    bool gettingHit;
    bool isDead;

    int currentFrame;

    ///Death fields
    Death deathState;
    public int deathFrames;

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
        currentState = EnemyStates.Walking;
        position = new Vector3(0, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 collisionPosition = new Vector2(position.x - .5f, position.y - .5f);
        collision = new Rect(collisionPosition, new Vector2(1,1));
        currentFrame = 0;
        gettingHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        switch(currentState)
        {
            case EnemyStates.Standing:
                break;
            case EnemyStates.Walking:
                //WalkEnemy();
                break;
            case EnemyStates.Attacking:
                break;
            case EnemyStates.Death:
                break;
            case EnemyStates.Hit:
                break;
            default:
                Debug.Log("Error! Enemy state unknown!");
                break;
        }

        Vector2 collisionPosition = new Vector2(position.x - .5f, position.y - .5f);
        collision.position = collisionPosition;
        transform.position = position;
        DrawRectangle(collision);
    }

    void WalkEnemy()
    {
        position = transform.position;

        position += Vector3.Normalize(player.transform.position - position) * speed;

        collision.position = position;

        transform.position = position;
    }

    void AttackEnemy()
    {

    }

    public void LoseHealth()
    {
        Debug.Log("Lost health runs track");
        enemyHealth--;

        if(enemyHealth <= 0)
        {
            isDead = true;
        }
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
