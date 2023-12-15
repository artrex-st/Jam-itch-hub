using System;
using UnityEngine;

namespace JIH.GamePlay
{
    public class PigeonFlyManager : PigeonBaseManager
    {
        private void Update()
        {
            MoveAsync();
        }
    }
}
