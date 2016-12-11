using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Paths
{
    public class CatmullRomSplineNode : MonoBehaviour
    {

        public CatmullRomSplineNode NextNode
        {
            get
            {
                if (transform.parent == null) return null;

                var parent = transform.parent.GetComponent<CatmullRomSpline>();

                if (parent == null) return null;

                var nodes = parent.Nodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    if (node.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    {
                        var next = nodes[i == nodes.Count - 1 ? 0 : i + 1];
                        return next.GetComponent<CatmullRomSplineNode>();
                    }
                }
                return null;
            }
        }

    }
}
