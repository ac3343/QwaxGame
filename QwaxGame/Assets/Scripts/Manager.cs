using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject player;
    EnemyManager enemyManager = new EnemyManager();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCollisionBox(Rect boxRect)
    {

    }
    
    void CheckCollisions()
    {

    }

    bool areColliding(Rect box1, Rect box2)
    {
        return box1.min.x < box2.max.x &&
            box1.min.y < box2.max.y &&
            box2.min.x < box1.max.x &&
            box2.min.y < box1.max.y;
    }

}
