using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HamburgerButton : MonoBehaviour
{
    public CustomAnimator buttonAnimator;
    public CustomAnimator contentAnimator;
    [Space]
    public string ButtonOpenAnim = "BurgerToX";
    public string ButtonCloseAnim = "XToBurger";
    [Space]
    public string ContentOpenAnim = "SlideIn";
    public string ContentCloseAnim = "SlideOut";

    bool isOpen;
    public void OpenOrClose()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            buttonAnimator.ChangeState(ButtonOpenAnim);
            contentAnimator.ChangeState(ContentOpenAnim);
        }
        else
        {
            buttonAnimator.ChangeState(ButtonCloseAnim);
            contentAnimator.ChangeState(ContentCloseAnim);
        }
    }

    public void SetOpenState(bool open)
    {
        isOpen = open;
        if (isOpen)
        {
            buttonAnimator.ChangeState(ButtonOpenAnim);
            contentAnimator.ChangeState(ContentOpenAnim);
        }
        else
        {
            buttonAnimator.ChangeState(ButtonCloseAnim);
            contentAnimator.ChangeState(ContentCloseAnim);
        }
    }
}
