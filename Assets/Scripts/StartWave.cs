using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    public class StartWave : MonoBehaviour
    {
        public static event Action Start;

        private Animator _animator;

        private void Awake()
        {
            TryGetComponent(out _animator);
        }

        public void InitWave()
        {
            _animator.Play("ButtonPressed");
            Start?.Invoke();
        }
    }

}
