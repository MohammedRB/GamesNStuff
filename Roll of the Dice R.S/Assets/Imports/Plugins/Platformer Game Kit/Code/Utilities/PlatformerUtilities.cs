// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer;
using System;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>Various utility methods used throughout the <see cref="PlatformerGameKit"/>.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/PlatformerUtilities
    /// 
    public static partial class PlatformerUtilities
    {
        /************************************************************************************************************************/

        /// <summary>An array of one hit for physics queries.</summary>
        public static readonly RaycastHit2D[] OneRaycastHit = new RaycastHit2D[1];

        /// <summary>An array of one contact for physics queries.</summary>
        public static readonly ContactPoint2D[] OneContact = new ContactPoint2D[1];

        /************************************************************************************************************************/

        /// <summary>Returns a new vector that is perpendicular to the `original`.</summary>
        public static Vector2 PerpendicularA(this Vector2 original) => new Vector2(-original.y, original.x);

        /// <summary>Returns a new vector that is perpendicular to the `original`.</summary>
        public static Vector2 PerpendicularB(this Vector2 original) => new Vector2(original.y, -original.x);

        /************************************************************************************************************************/

        /// <summary>Returns a new vector that is perpendicular to the `original` on the XY plane.</summary>
        public static Vector3 PerpendicularAXY(this Vector3 original) => new Vector3(-original.y, original.x, original.z);

        /// <summary>Returns a new vector that is perpendicular to the `original` on the XY plane.</summary>
        public static Vector3 PerpendicularBXY(this Vector3 original) => new Vector3(original.y, -original.x, original.z);

        /************************************************************************************************************************/

        /// <summary>Makes sure the `value` is not negative.</summary>
        public static void NotNegative(ref float value) => value = Math.Max(0, value);

        /// <summary>Makes sure the `value` is not negative.</summary>
        public static void NotNegative(ref int value) => value = Math.Max(0, value);

        /// <summary>Makes sure the `value` is between the `min` and `max` (inclusive).</summary>
        public static void Clamp(ref float value, float min, float max) => value = Mathf.Clamp(value, min, max);

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the `target` if `smoothTime` is 0. Otherwise calls
        /// <see cref="Mathf.SmoothDamp(float, float, ref float, float)"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Mathf.SmoothDamp(float, float, ref float, float)"/> has a (very small) minimum threshold for
        /// `smoothTime` which can prevent the result from exactly reaching the `target` and is often undesirable.
        /// </remarks>
        public static float SmoothDamp(float current, float target, ref float velocity, float smoothTime)
            => smoothTime > 0 ? Mathf.SmoothDamp(current, target, ref velocity, smoothTime) : target;

        /// <summary>
        /// Returns the `target` if `smoothTime` is 0. Otherwise calls
        /// <see cref="Vector2.SmoothDamp(Vector2, Vector2, ref Vector2, float)"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Vector2.SmoothDamp(Vector2, Vector2, ref Vector2, float)"/> has a (very small) minimum threshold
        /// for `smoothTime` which can prevent the result from exactly reaching the `target` and is often undesirable.
        /// </remarks>
        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 velocity, float smoothTime)
            => smoothTime > 0 ? Vector2.SmoothDamp(current, target, ref velocity, smoothTime) : target;

        /************************************************************************************************************************/

        /// <summary>Resizes the `array` to be 1 larger and inserts the `item` at the specified `index`.</summary>
        public static void InsertAt<T>(ref T[] array, int index, T item)
        {
            var newArray = new T[array.Length + 1];
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index, newArray, index + 1, array.Length - index);
            newArray[index] = item;
            array = newArray;
        }

        /************************************************************************************************************************/

        /// <summary>Removes the item at the specified `index` and resizes the `array` to be 1 smaller.</summary>
        public static void RemoveAt<T>(ref T[] array, int index)
        {
            var newArray = new T[array.Length - 1];
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
            array = newArray;
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Returns a copy of the `point` rounded to the nearest pixel of the <see cref="SpriteRenderer"/> attached to
        /// the `gameObject` (or its parent or children).
        /// </summary>
        public static Vector2 RoundToPixel(GameObject gameObject, Vector2 point)
        {
            var renderer = gameObject.GetComponentInParentOrChildren<SpriteRenderer>();

            if (renderer != null && renderer.sprite != null)
            {
                var sprite = renderer.sprite;

                var inversePixelsPerUnit = 1f / sprite.pixelsPerUnit;
                var pivot = sprite.pivot * inversePixelsPerUnit;

                var transform = renderer.transform;
                point -= (Vector2)transform.position;

                point.x = pivot.x + AnimancerUtilities.Round(point.x - pivot.x, inversePixelsPerUnit);
                point.y = pivot.y + AnimancerUtilities.Round(point.y - pivot.y, inversePixelsPerUnit);

                point += (Vector2)transform.position;
                point = Vector2.Scale(point, transform.lossyScale);
            }

            return point;
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Draws a 2D handle at the specified `position` which can be dragged around.</summary>
        public static Vector2 DoHandle2D(Vector2 position, float sizeMultiplier = 1)
        {
            var size = UnityEditor.HandleUtility.GetHandleSize(position) * 0.05f * sizeMultiplier;
            return UnityEditor.Handles.Slider2D(
                position, Vector3.forward, Vector3.right, Vector3.up, size, UnityEditor.Handles.DotHandleCap, 0);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #region Debug
        /************************************************************************************************************************/

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawBox(Vector3 center, Vector3 size, Color color, float duration = float.Epsilon)
        {
#if UNITY_EDITOR
            if (duration > 0)
            {
                size *= 0.5f;

                var flipSize = size;
                flipSize.x = -flipSize.x;

                Debug.DrawLine(center - size, center - flipSize, color, duration);
                Debug.DrawLine(center + size, center - flipSize, color, duration);
                Debug.DrawLine(center + size, center + flipSize, color, duration);
                Debug.DrawLine(center - size, center + flipSize, color, duration);
            }
#endif
        }

        /************************************************************************************************************************/

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawBoxCast(Vector3 center, Vector3 size, Vector3 direction, Color color, float duration = float.Epsilon)
        {
#if UNITY_EDITOR
            if (duration > 0)
            {
                DrawBox(center, size, color, duration);

                size *= 0.5f;

                var flipSize = size;
                flipSize.x = -flipSize.x;

                Debug.DrawRay(center - size, direction, color, duration);
                Debug.DrawRay(center + size, direction, color, duration);
                Debug.DrawRay(center - flipSize, direction, color, duration);
                Debug.DrawRay(center + flipSize, direction, color, duration);
            }
#endif
        }

        /************************************************************************************************************************/

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = float.Epsilon)
        {
            if (duration > 0)
                Debug.DrawLine(start, end, color, duration);
        }

        /************************************************************************************************************************/

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawRay(Vector3 point, Vector3 normal, Color color, float duration = float.Epsilon)
        {
#if UNITY_EDITOR
            if (duration > 0)
            {
                Debug.DrawLine(point, point + normal, color, duration);

                normal *= 0.05f;
                var tangent = normal.PerpendicularAXY();

                Debug.DrawLine(point + normal + tangent, point - normal - tangent, color, duration);
                Debug.DrawLine(point + normal - tangent, point - normal + tangent, color, duration);
            }
#endif
        }

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawRay(RaycastHit hit, Color color, float duration = float.Epsilon)
            => DrawRay(hit.point, hit.normal, color, duration);

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawRay(RaycastHit2D hit, Color color, float duration = float.Epsilon)
            => DrawRay(hit.point, hit.normal, color, duration);

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawRay(ContactPoint contact, Color color, float duration = float.Epsilon)
            => DrawRay(contact.point, contact.normal, color, duration);

        [System.Diagnostics.Conditional(Animancer.Strings.UnityEditor)]
        public static void DrawRay(ContactPoint2D contact, Color color, float duration = float.Epsilon)
            => DrawRay(contact.point, contact.normal, color, duration);

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}