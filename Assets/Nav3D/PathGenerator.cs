using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.Nav3D
{
    public class PathGenerator
    {
        private readonly Vector3[] _neighbours = new Vector3[26]
        {
            new Vector3(-1, 1, -1),
            new Vector3(-1, 1, 0),
            new Vector3(-1, 1, 1),
            new Vector3(0, 1, -1),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, -1),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),

            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),

            new Vector3(-1, -1, -1),
            new Vector3(-1, -1, 0),
            new Vector3(-1, -1, 1),
            new Vector3(0, -1, -1),
            new Vector3(0, -1, 0),
            new Vector3(0, -1, 1),
            new Vector3(1, -1, -1),
            new Vector3(1, -1, 0),
            new Vector3(1, -1, 1),
        };

        private BoxCaster _boxCaster;
        private int _maxPointAmount;

        public PathGenerator(BoxCaster boxCaster, int maxPointAmount)
        {
            _boxCaster = boxCaster ?? throw new ArgumentNullException(nameof(boxCaster));
            _maxPointAmount = maxPointAmount;
        } 

        public bool TryCreatePath(Vector3 from, Vector3 to, out List<Vector3> path)
        {
            path = null;
            from = VectorOps.RoundToGrid(from, _boxCaster.Size);
            to = VectorOps.RoundToGrid(to, _boxCaster.Size);

            if (_boxCaster.Check(from) || _boxCaster.Check(to)) return false;

            if (from == to) return true;

            var pathLinks = new Dictionary<Vector3, Vector3>(_maxPointAmount);
            var checkCache = new List<Vector4>(_maxPointAmount);

            pathLinks.Add(from, from);
            checkCache.Add(from);

            while (checkCache.Count > 0 && pathLinks.Count < _maxPointAmount)
            {
                float minSqrDistance = float.PositiveInfinity;
                int minIndex = 0;
                for (int i = 0; i < checkCache.Count; i++)
                {
                    float sqrDistance = checkCache[i].w;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        minIndex = i;
                    }
                }

                Vector3 position = checkCache[minIndex];
                checkCache.RemoveAt(minIndex);

                bool hasTarget = TryGetTarget(position, to, pathLinks, checkCache);
                if (hasTarget) break;
            }

            if (!pathLinks.ContainsKey(to)) return false;

            path = GetPath(from, to, pathLinks);

            return true;
        }

        private List<Vector3> GetPath(Vector3 from, Vector3 to, Dictionary<Vector3, Vector3> pathLinks)
        {
            List<Vector3> reversePath = new List<Vector3>(_maxPointAmount);
            reversePath.Add(to);

            while (pathLinks.TryGetValue(reversePath[reversePath.Count - 1], out Vector3 position) && reversePath[reversePath.Count - 1] != from)
            {
                reversePath.Add(position);
            }

            reversePath.Reverse();

            return reversePath;
        }

        private bool TryGetTarget(Vector3 position, Vector3 target, Dictionary<Vector3, Vector3> links, List<Vector4> checkCache)
        {
            for (int i = 0; i < _neighbours.Length; i++)
            {
                Vector3 neighbourPosition = position + _neighbours[i] * _boxCaster.Size;

                if (!links.ContainsKey(neighbourPosition) && !_boxCaster.Check(neighbourPosition))
                {
                    links[neighbourPosition] = position;
                    checkCache.Add(new Vector4(neighbourPosition.x, neighbourPosition.y, neighbourPosition.z, (target - neighbourPosition).sqrMagnitude));

                    if (neighbourPosition == target) return true;
                }
            }

            return false;
        }
    }
}