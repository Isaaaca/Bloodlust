using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float duration =1;

    private float targetOpacity = 0;
    private Image image;
    private bool inTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (image.color.a > targetOpacity)
        {
            image.color -= new Color(0, 0, 0, 1) * Time.deltaTime / duration;
            if (image.color.a < 0)
            {
                image.color = new Color(0, 0, 0, 0);
                inTransition = false;
            }
        }
        else if(image.color.a < targetOpacity)
        {
            image.color += new Color(0, 0, 0, 1) * Time.deltaTime / duration;
            if (image.color.a > 1)
            {
                image.color = new Color(0, 0, 0, 1);
                inTransition = false;
            }

        }
        else if (image.color.a == targetOpacity)
        {
            inTransition = false;
        }
    }

    public void FadeToBlack()
    {
        inTransition = true;
        targetOpacity = 1;
    }

    public void FadeIn()
    {
        inTransition = true;
        targetOpacity = 0;
    }

    public bool isTransitioning()
    {
        return inTransition;
    }

    public float getOpacity()
    {
        return image.color.a;
    }
}
