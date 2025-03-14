﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CoroutineHandler : MonoBehaviour
    {
        public static CoroutineHandler Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }
}