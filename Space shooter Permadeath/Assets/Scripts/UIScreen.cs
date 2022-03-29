using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    
    public float fadeInTime;
    public float fadeOutTime;

    public List<Image> imageList;
    public List<Color> imageColorList;
    public List<Text> textList;
    public List<Color> textColorList;

    // Start is called before the first frame update
    void Awake()
    {
        imageList.AddRange(GetComponentsInChildren<Image>(true));
        textList.AddRange(GetComponentsInChildren<Text>(true));
        foreach (Image image in imageList) imageColorList.Add(image.color);
        foreach (Text text in textList) textColorList.Add(text.color);
    }

    public IEnumerator FadeIn()
    {
        gameObject.SetActive(true);
        // loop over 1 second
        for (float time = 0; time <= 1; time += Time.deltaTime / fadeInTime)
        {
            for (int t = 0; t < imageList.Count; t++)
            {
                Color color = imageColorList[t];
                color.a = imageColorList[t].a * time;
                imageList[t].color = color;
            }

            for (int t = 0; t < textList.Count; t++)
            {
                Color color = textColorList[t];
                color.a = textColorList[t].a * time;
                textList[t].color = color;
            }
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        for (float time = 1; time >= 0; time -= Time.deltaTime / fadeOutTime)
        {
            for (int t = 0; t < imageList.Count; t++)
            {
                Color color = imageColorList[t];
                color.a = imageColorList[t].a * time;
                imageList[t].color = color;
            }

            for (int t = 0; t < textList.Count; t++)
            {
                Color color = textColorList[t];
                color.a = textColorList[t].a * time;
                textList[t].color = color;
            }
            yield return null;
        }

        gameObject.SetActive(false);

    }
}

