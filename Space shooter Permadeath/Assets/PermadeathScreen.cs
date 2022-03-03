using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermadeathScreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeInTime;
    public float fadeOutTime;
    Color color;
 
    // Start is called before the first frame update
    void Start()
    {
        color = spriteRenderer.color;
    }

    public IEnumerator FadeIn()
    {
        spriteRenderer.enabled = true;
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeInTime)
        {
            Debug.Log(i);
            // set color with i as alpha
            color.a = i;
            spriteRenderer.color = color;
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeOutTime)
        {
            Debug.Log(i);
            // set color with i as alpha
            color.a = i;
            spriteRenderer.color = color;
            yield return null;
        }

        spriteRenderer.enabled = true;

    }
}
