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
    public bool CloseMenuOnDisable;
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
        if (isOpen == open) return;
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
    private void OnEnable()
    {
        
        if(CloseMenuOnDisable) isOpen = false;

        // A simple fix for the animators resetting when getting disabled.
        if (isOpen)
        {
            buttonAnimator.SetState(ButtonOpenAnim, 0, 1);
            contentAnimator.SetState(ContentOpenAnim, 0, 1);
        }
        else
        {
            buttonAnimator.SetState(ButtonCloseAnim, 0, 1);
            contentAnimator.SetState(ContentCloseAnim, 0, 1);
        }
    }
}
