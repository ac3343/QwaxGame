using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death
{
    int frames;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveFrames(int deathFrames)
    {
        frames = deathFrames;
    }

    public bool DeathFinished(int currentFrame)
    {
        return currentFrame >= frames;
    }
}
