using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV22_MorenoSosa
{
    public enum ENEMY { BASIC = 0, BOSS = 1 };

    public class Enemy : MonoBehaviour
    {
        public ENEMY EnemyType;
    }
}