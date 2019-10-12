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

    public Vector3 position;
    public float speed;
    EnemyStates currentState = EnemyStates.Standing;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(5, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
