using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _openAnim;
    [SerializeField] private AnimationClip _closeAnim;

    private bool isOpen;
    public void ToggleAnimation()
    {
        if (!isOpen)
        {
            isOpen = true;
            _animator.Play(_openAnim.name);
        }
        else
        {
            isOpen = false;
            _animator.Play(_closeAnim.name);
        }
    }
}
