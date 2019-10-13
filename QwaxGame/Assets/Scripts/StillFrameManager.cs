using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillFrameManager : MonoBehaviour
{
    public float timeEachFrame = 4.0f;
    public Sprite[] stillFrames;
    private float timeLeftThisFrame;
    private int frameIndex = 0;
    private bool fadingIn = false;
    private bool fadingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftThisFrame = timeEachFrame;
    }

    // Update is called once per frame
    void Update()
    {

        if(GetComponent<SpriteRenderer>().color.a >= 1 && !fadingOut)
        {
            timeLeftThisFrame -= Time.deltaTime;

            if(timeLeftThisFrame <= 0)
            {
                GetComponent<FadeSprite>().FadeOut();
                timeLeftThisFrame = timeEachFrame;
                fadingIn = false;
                fadingOut = true;
            }
        }

        if(GetComponent<SpriteRenderer>().color.a <= 0 && !fadingIn)
        {
            if (frameIndex < stillFrames.Length - 1)
            {
                frameIndex++;
                GetComponent<SpriteRenderer>().sprite = stillFrames[frameIndex];
                GetComponent<FadeSprite>().FadeIn();
                fadingIn = true;
                fadingOut = false;
            }
        }
    }
}
