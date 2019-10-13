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
        position = new Vector3(5, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        collision = new Rect(position, new Vector2(2,2));
        currentFrame = 0;
        gettingHit = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        enemyHealth--;

        if(enemyHealth <= 0)
        {
            isDead = true;
        }
    }
}
