using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HamburgerButton : MonoBehaviour
{
    public CustomAnimator buttonAnimator;
    public CustomAnimator contentAnimator;
    bool isOpen;
    public void OpenOrClose()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            buttonAnimator.ChangeState("BurgerToX");
            contentAnimator.ChangeState("SlideIn");
        }
        else
        {
            buttonAnimator.ChangeState("XToBurger");
            contentAnimator.ChangeState("SlideOut");
        }
    }
}
