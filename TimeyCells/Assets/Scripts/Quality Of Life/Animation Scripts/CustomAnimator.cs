using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    public Animator Animator;
    string currState;
    public void ChangeState(string newstate)
    {
        if (currState == newstate) return;
        Animator.Play(newstate);
        currState = newstate;
    }
}
