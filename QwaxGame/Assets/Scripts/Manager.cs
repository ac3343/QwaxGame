using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //Fields
    public GameObject playerPrefab;
    GameObject player;

    public GameObject enemyGroupPrefab;
    List<GameObject> enemyGroups = new List<GameObject>();

    public int playerHealth;                //In inspector

    public List<GameObject> enemyLocations; //In inspector

    public List<GameObject> audioSources;
    

    // Start is called before the first frame update
    void Start()
    {
        //Sets the player variable to the created player instance
        player = Instantiate(playerPrefab, new Vector3(-50, -16, 0), Quaternion.identity);

        if(audioSources.Count > 0)
        {
            audioSources[0].GetComponent<AudioSource>().Play();
        }

        //Adds created enemy group instance to the enemy groups list
        foreach(GameObject g in enemyLocations)
        {
            enemyGroups.Add(Instantiate(enemyGroupPrefab, g.transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for collision
        CheckCollisions();
    }
    
    void CheckCollisions()
    {
        //Gets refrence to player object's script
        Player playerInfo = player.GetComponent<Player>();

        //Checks to see if the current attack collision is not zero
        if (playerInfo.CurrentAttackCollision != Rect.zero)
        {
            //Runs through the list of enemygroups
            foreach(GameObject g in enemyGroups)
            {
                //Gets a refrence to the current enemy group's script
                EnemyManager enemyGroup = g.GetComponent<EnemyManager>();

                //Runs through list of enemies in current enemy group
                for(int i = enemyGroup.enemies.Count - 1; i >= 0; i--)
                {
                    //Gets a refrence to the current enemy's script
                    Enemy currentEnemy = enemyGroup.enemies[i].GetComponent<Enemy>();

                    
                    EnemyCollision(enemyGroup.enemies[i], g, playerInfo.CurrentAttackCollision, i);
                    
                    /*
                    //Checks to see if the enemy and the attack box are colliding
                    if (areColliding(playerInfo.CurrentAttackCollision, currentEnemy.collision))
                    {

                        //Gets a refrence to the enemy to be destroyed
                        GameObject destroyedEnemy = enemyGroup.enemies[i];

                        //Removes enemy from the current list of enemies
                        enemyGroup.enemies.RemoveAt(i);

                        //Destroys current enemy
                        GameObject.Destroy(destroyedEnemy);
                        
                    }
                    */
                }
            }
        }
        else
        {
            //Runs through the list of enemygroups
            foreach (GameObject g in enemyGroups)
            {
                //Gets a refrence to the current enemy group's script
                EnemyManager enemyGroup = g.GetComponent<EnemyManager>();

                //Runs through list of enemies in current enemy group
                for (int i = enemyGroup.enemies.Count - 1; i >= 0; i--)
                {
                    //Gets a refrence to the current enemy's script
                    Enemy currentEnemy = enemyGroup.enemies[i].GetComponent<Enemy>();


                    currentEnemy.GettingHit = false;
                }
            }
        }

        foreach(GameObject g in enemyGroups)
        {
            //Checks to see if the enemyGroup is within a certain range of the player
            if(player.transform.position.x - g.transform.position.x <6 && player.transform.position.x - g.transform.position.x > -6)
            {
                //Gets a refrence to the current enemy group's script
                EnemyManager enemyGroup = g.GetComponent<EnemyManager>();

                //Runs through list of enemies in current enemy group
                for (int i = enemyGroup.enemies.Count - 1; i >= 0; i--)
                {
                    //Gets a refrence to the current enemy's script
                    Enemy currentEnemy = enemyGroup.enemies[i].GetComponent<Enemy>();

                    //Checks to see if 
                    if (currentEnemy.currentState == Enemy.EnemyStates.Attacking && 
                        areColliding(currentEnemy.attackCollisionBox, playerInfo.CollisionBox) && 
                        (playerInfo.CurrentState != Player.PlayerStates.Damaged || playerInfo.CurrentState != Player.PlayerStates.Attacking) && 
                        playerHealth > 0 && !playerInfo.IsInvincible)
                    {

                        
                        playerHealth--;

                        if(playerHealth < 1)
                        {
                            playerInfo.Dies();
                        }
                        else
                        {
                            playerInfo.GetsDamaged();
                        }
                    }
                }
            }
        }
    }

    bool areColliding(Rect box1, Rect box2)
    {
        return box1.min.x < box2.max.x &&
            box1.min.y < box2.max.y &&
            box2.min.x < box1.max.x &&
            box2.min.y < box1.max.y;
    }

    void EnemyCollision(GameObject enemy, GameObject enemyGroup, Rect playerCollision, int currentEnemyIndex)
    {
        //Gets a refrence to the current enemy group's script
        EnemyManager currentEnemyGroup = enemyGroup.GetComponent<EnemyManager>();

        Enemy currentEnemy = enemy.GetComponent<Enemy>();


        //Checks to see if the enemy and the attack box are colliding
        if (areColliding(playerCollision, currentEnemy.collision))
        {
            //If the enemy isn't dead and aren't getting hit
            if (!currentEnemy.GettingHit && !currentEnemy.IsDead)
            {
                //Reduces enemy health
                currentEnemy.LoseHealth();

                //If the enemy survives the hit
                if (!currentEnemy.IsDead)
                {
                    currentEnemy.GettingHit = true;
                }
                else
                {
                    //Gets a refrence to the enemy to be destroyed
                    GameObject destroyedEnemy = currentEnemyGroup.enemies[currentEnemyIndex];

                    //Removes enemy from the current list of enemies
                    currentEnemyGroup.enemies.RemoveAt(currentEnemyIndex);

                    //Destroys current enemy
                    GameObject.Destroy(destroyedEnemy);
                }
            }

        }
        else
        {
            currentEnemy.GettingHit = false;
        }

    }

    
}
