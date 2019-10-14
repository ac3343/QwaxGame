using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;

    public GameObject sceneManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            FadeToLevel();
        }

        Manager scene = sceneManager.GetComponent<Manager>();
        
        if (scene.MusicFinished() && (SceneManager.GetActiveScene().buildIndex == 0 ||  SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 5))
        {
            FadeToLevel();
        }
        else if(scene.PlayerPastGoalPost() && (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4))
        {
            FadeToLevel();
        }

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 5:
                if (scene.MusicFinished())
                {
                    FadeToLevel();
                }
                break;
            case 2:
                break;
            case 3:
                if (scene.PlayerPastGoalPost())
                {
                    FadeToLevel();
                }
                break;
            case 4:
                if (scene.PlayerPastGoalPost())
                {
                    FadeToLevel();
                }
                break;
            
            default:
                Debug.Log("Scene Doesn't exist!");
                break;
        }
        
    }

    public void FadeToLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex < 5)
        {
            levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            animator.SetTrigger("FadeOut");
        }
        

    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}