using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Game
{
    [RequireComponent(typeof(SphereCollider))]
    public class Checkpoint : MonoBehaviour
    {
        public int Number = 0;

        void Start()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }
    }
}
