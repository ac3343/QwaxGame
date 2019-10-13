using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    public float fadeOutTime = 1.0f;
    public float fadeInTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeIn()
    {
        StartCoroutine(SpriteFadeIn(GetComponent<SpriteRenderer>()));
    }

    public void FadeOut()
    {
        StartCoroutine(SpriteFadeOut(GetComponent<SpriteRenderer>()));
    }

    IEnumerator SpriteFadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;

        while(tmpColor.a > 0f)
        {
            tmpColor.a -= Time.deltaTime / fadeOutTime;
            _sprite.color = tmpColor;

            if (tmpColor.a <= 0f)
                tmpColor.a = 0.0f;

            yield return null;
        }

        _sprite.color = tmpColor;
    }
    IEnumerator SpriteFadeIn(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;

        while (tmpColor.a < 1f)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            _sprite.color = tmpColor;

            if (tmpColor.a <= 0f)
                tmpColor.a = 0.0f;

            yield return null;
        }

        _sprite.color = tmpColor;
    }
}
