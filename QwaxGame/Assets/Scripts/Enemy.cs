using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public enum EnemyStates
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

    public EnemyStates currentState;
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

    private bool bossCutsceneHappened = false;
    private bool bossCutsceneIsHappening = false;

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
        position = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 collisionPosition = new Vector2(position.x - (transform.localScale.x / 2), position.y - (transform.localScale.y / 2));
        collision = new Rect(collisionPosition, new Vector2(transform.localScale.x, transform.localScale.y));

        currentAttackFrame = 0;
        gettingHit = false;

        enemyAttack = new Attack();
        enemyAttack.GiveFrames(30, 30, 30);
        attackRadius = 2.5f;

        attackCollisionBox = Rect.zero;

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            enemyHealth = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        previousState = currentState;
        position = transform.position;

        if (bossCutsceneIsHappening) PlayBossCutscene();

        switch(currentState)
        {
            case EnemyStates.Standing:
                if (IsPlayerClose())
                    currentState = EnemyStates.Attacking;
                break;
            case EnemyStates.Attacking:
                if(SceneManager.GetActiveScene().name.Equals("BossFight"))
                {
                    if(!bossCutsceneHappened)
                    {
                        player.GetComponent<Player>().enabledControls = false;
                        player.GetComponent<Player>().anim.Play("PlayerFall_1", 0, 0);
                        bossCutsceneHappened = true;
                        bossCutsceneIsHappening = true;
                    }
                }

                if (!enemyAttack.AttackFinished(currentAttackFrame))
                {
                    if(enemyAttack.currentAttackState == Attack.AttackStates.Attack)
                    {
                        attackCollisionBox = new Rect(new Vector2(collision.xMin - attackCollisionSize.x / 2, collision.yMin - attackCollisionSize.y / 2), attackCollisionSize);
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

        transform.position = position;

        DrawRectangle(collision);
        DrawRectangle(attackCollisionBox);
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

    void PlayBossCutscene()
    {
        Player playerInfo = player.GetComponent<Player>();
        if(Mathf.Abs(playerInfo.position.x - position.x) < 10)
        {
            playerInfo.position.x -= 3 * Time.deltaTime;
            playerInfo.transform.position = playerInfo.position;
        }
        else
        {
            playerInfo.anim.Play("PlayerPower_1", 0, 0);
            playerInfo.isPoweredUp = true;
            bossCutsceneIsHappening = false;
            playerInfo.enabledControls = true;
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
