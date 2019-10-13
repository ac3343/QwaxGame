using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;

    private string levelToLoad;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FadeToLevel("Scene2");
        }
    }

    public void FadeToLevel(string levelName)
    {
        levelToLoad = levelName;
        animator.SetTrigger("FadeOut");

    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}