using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PoolSystem
{
    [CreateAssetMenu(fileName = "NewPoolPack", menuName = "ScriptableObjects/ObjectPool", order = 1)]
    public class PoolPack : ScriptableObject
    {
        public List<Pool> pools;
    }
}


