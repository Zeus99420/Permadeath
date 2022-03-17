using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermadeathScreen : MonoBehaviour
{
    public Image image;
    public float fadeInTime;
    public float fadeOutTime;
    Color color;
 
    // Start is called before the first frame update
    void Awake()
    {
        color = image.color;
    }

    public IEnumerator FadeIn()
    {
        image.enabled = true;
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeInTime)
        {
            // set color with i as alpha
            color.a = i;
            image.color = color;
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeOutTime)
        {
            // set color with i as alpha
            color.a = i;
            image.color = color;
            yield return null;
        }

        //image.enabled = false;

    }
}
