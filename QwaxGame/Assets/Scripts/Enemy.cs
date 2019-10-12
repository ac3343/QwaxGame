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
    }

    public GameObject player;
    public Vector3 position;
    public float speed;
    EnemyStates currentState;
    public Rect collision;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyStates.Walking;
        position = new Vector3(5, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        collision = new Rect(position, new Vector2(2,2));
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyStates.Standing:
                break;
            case EnemyStates.Walking:
                WalkEnemy();
                break;
            case EnemyStates.Attacking:
                break;
            default:
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
}
