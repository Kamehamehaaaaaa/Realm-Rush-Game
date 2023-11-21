using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayHover : MonoBehaviour
{
    [SerializeField] GameObject HowToPlayImage;

    public void OnMouseOver() 
    {
        HowToPlayImage.SetActive(true);
    }

    public void OnMouseExit() 
    {
        HowToPlayImage.SetActive(false);
    }
}
