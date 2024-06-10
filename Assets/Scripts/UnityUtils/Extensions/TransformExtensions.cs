using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityUtils.Extensions
{
    public static class TransformExtensions
    {
        public static float ListDistance(this List<Transform> transforms)
        {
            var distances = new List<float>();
            
            foreach (var pointA in transforms)
                distances.AddRange(transforms.Select(pointB => 
                    Vector3.Distance(pointA.position, pointB.position)));

            return distances.Max();
        }
        
        public static Vector3 ListCenter(this List<Transform> transforms)
        {
            float totalX = 0;
            float totalY = 0;
            float totalZ = 0;

            foreach (var position in transforms.Select(transform => transform.position))
            {
                totalX += position.x;
                totalY += position.y;
                totalZ += position.z;
            }

            var total = new Vector3(totalX,totalY,totalZ);
            var center = total / transforms.Count;
            return center;
        }
        
        public static void Teleport(this Transform target, Transform to)
        {
            target.transform.position = to.position;
            target.transform.rotation = to.rotation;
        }
        
        public static void Teleport(this Transform target, Vector3 position, Quaternion rotation)
        {
            target.transform.position = position;
            target.transform.rotation = rotation;
        }
        
        public static bool RaycastToTransform(this Transform thisTransform, Transform target, out RaycastHit hitInfo,
            float maxDistance = 500, bool drawGizmos = true)
        {
            if (!Physics.Raycast(thisTransform.position, thisTransform.ToDirection(target), out hitInfo, maxDistance))
                return false;
            if (drawGizmos)
                Debug.DrawLine(thisTransform.position, hitInfo.point, Color.red);
            return true;
        }

        public static bool RaycastToTransform(this Transform thisTransform, Transform target, out RaycastHit hitInfo,
            int layerMask, float maxDistance = 500, bool drawGizmos = true)
        {
            if (!Physics.Raycast(thisTransform.position, thisTransform.ToDirection(target), out hitInfo, maxDistance,
                    layerMask))
                return false;
            if (drawGizmos)
                Debug.DrawLine(thisTransform.position, hitInfo.point, Color.red);
            return true;
        }

        public static Vector3 ToDirection(this Transform thisTransform, Transform target)
            => (target.position - thisTransform.position).normalized;

        public static List<Transform> GetChildren(this Transform target)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                Transform child = target.transform.GetChild(i);
                children.Add(child);
            }

            return children;
        }

        public static void AddToPositionXAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.x += add;
            transform.position = pos;
        }

        public static void AddToPositionYAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.y += add;
            transform.position = pos;
        }

        public static void AddToPositionZAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.z += add;
            transform.position = pos;
        }

        public static Vector3 ChangeLocalRotationAxis(this Transform transform, Axis axis, float value)
        {
            var rotation = transform.localRotation.eulerAngles;
            switch (axis)
            {
                case Axis.X: rotation.x = value; break;
                case Axis.Y: rotation.y = value; break;
                case Axis.Z: rotation.z = value; break;
            }

            transform.localEulerAngles = rotation;
            return rotation;
        }

        public static void ChangeRotationAxis(this Transform transform, Axis axis, float value)
        {
            var rotation = transform.eulerAngles;
            switch (axis)
            {
                case Axis.X: rotation.x = value; break;
                case Axis.Y: rotation.y = value; break;
                case Axis.Z: rotation.z = value; break;
            }
            transform.eulerAngles = rotation;
        }

        public static void DestroyChildren(this Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child != null)
                    Object.Destroy(child.gameObject);
            }
        }
        
        public static void ResetTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        
        public static void ResetTransformWithoutScale(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        public static void LookAtWithoutY(this Transform transform, Transform target)
        {
            var direction = target.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }
}