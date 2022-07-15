// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#if UNITY_EDITOR

using Animancer;
using Animancer.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace PlatformerGameKit
{
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/HitData
    partial class HitData
    {
        /// <summary>[Editor-Only] GUI for <see cref="HitData"/>.</summary>
        /// <remarks>
        /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/melee">Melee Attacks</see>
        /// </remarks>
        /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Drawer
        /// 
        public class Drawer
        {
            /************************************************************************************************************************/
            #region Timeline
            /************************************************************************************************************************/

            public static void OnTimelineBackgroundGUI(HitData[] hits)
            {
                if (hits == null ||
                    Event.current.type != EventType.Repaint)
                    return;

                var previousColor = GUI.color;
                var timelineGUI = TimelineGUI.Current;

                for (int i = 0; i < hits.Length; i++)
                {
                    var hit = hits[i];
                    var start = timelineGUI.SecondsToPixels(hit.StartTime);
                    var end = timelineGUI.SecondsToPixels(hit.EndTime);
                    var area = new Rect(start, 0, end - start, timelineGUI.Area.height - timelineGUI.TickHeight);

                    var color = i % 2 == 0 ?
                        new Color(1, 0.5f, 0.35f, 0.5f) :
                        new Color(0.9f, 0.4f, 0.25f, 0.5f);

                    EditorGUI.DrawRect(area, color);
                }

                GUI.color = previousColor;
            }

            /************************************************************************************************************************/

            public static void OnTimelineForegroundGUI(HitData[] hits) { }

            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
            #region Preview Scene
            /************************************************************************************************************************/

            private static bool PreviewSceneGUIRequiresCurrentEvent
            {
                get
                {
                    switch (Event.current.type)
                    {
                        // Allow:
                        case EventType.Layout:
                        case EventType.Repaint:
                        case EventType.MouseDown:
                        case EventType.MouseUp:
                        case EventType.MouseMove:
                        case EventType.MouseDrag:
                            return true;

                        // Ignore:
                        default:
                            return false;
                    }
                }
            }

            /************************************************************************************************************************/

            public static void OnPreviewSceneGUI(ref HitData[] hits, TransitionPreviewDetails details, AnimancerState state)
            {
                if (hits == null ||
                    !PreviewSceneGUIRequiresCurrentEvent)
                    return;

                var time = state.Time;
                var isAnyActive = false;

                HandleAddRemovePoints(hits, details, time);

                for (int i = 0; i < hits.Length; i++)
                {
                    var hit = hits[i];
                    if ((hit.IsActiveAt(time) ||
                        Mathf.Approximately(hit.StartTime, time)) &&
                        !Mathf.Approximately(hit.EndTime, time))
                    {
                        isAnyActive = true;
                        DoPreviewSceneGUI(hit, details);
                    }
                }

                DoSceneOverlayGUI(ref hits, details, time, isAnyActive);
            }

            /************************************************************************************************************************/

            private static void DoSceneOverlayGUI(ref HitData[] hits, TransitionPreviewDetails details, float time, bool isAnyActive)
            {
                const float Border = 5;

                Handles.BeginGUI();

                using (ObjectPool.Disposable.AcquireContent(out var content, "Add Hit"))
                {
                    var width = GUI.skin.button.CalculateWidth(content);
                    var area = new Rect(Border, Border, width, AnimancerGUI.LineHeight);
                    if (GUI.Button(area, content))
                        AddHit(ref hits, details, time);
                    AnimancerGUI.NextVerticalArea(ref area);

                    if (isAnyActive)
                    {
                        const string Instructions =
                            "Shift + Click = Add/Remove Points" +
                            "\nCtrl + Drag = Snap to Grid";
                        area.width = Screen.width - Border * 2;
                        area.yMax = Screen.height - Border * 2;
                        GUI.Label(area, Instructions, EditorStyles.whiteLargeLabel);
                    }
                }

                Handles.EndGUI();
            }

            /************************************************************************************************************************/

            private static void AddHit(ref HitData[] hits, TransitionPreviewDetails details, float time)
            {
                TransitionPreviewDetails.Property.RecordUndo("Add Hit");

                var index = 0;
                for (; index < hits.Length; index++)
                    if (hits[index].StartTime > time)
                        break;

                var hit = new HitData
                {
                    StartTime = time,
                };

                if (AnimancerUtilities.TryGetFrameRate(TransitionPreviewDetails.Transition, out var frameRate))
                {
                    hit.EndTime = time + 1f / frameRate;
                }
                else
                {
                    hit.EndTime = time + 1;
                }

                if (hits.Length > 0)
                {
                    var copyFrom = hits[Mathf.Clamp(index - 1, 0, hits.Length - 1)];
                    hit.Area = new Vector2[copyFrom.Area.Length];
                    Array.Copy(copyFrom.Area, hit.Area, hit.Area.Length);

                    hit.Damage = copyFrom.Damage;
                }
                else
                {
                    hit.Area = GetDefaultArea(details, index);
                }

                PlatformerUtilities.InsertAt(ref hits, index, hit);
            }

            /************************************************************************************************************************/

            private static Vector2[] GetDefaultArea(TransitionPreviewDetails details, int index)
            {
                var renderer = details.Transform.gameObject.GetComponentInParentOrChildren<SpriteRenderer>();

                if (renderer != null && renderer.sprite != null)
                {
                    var bounds = renderer.sprite.bounds;
                    return new Vector2[]
                    {
                        bounds.min,
                        new Vector2(bounds.max.x, bounds.min.y),
                        bounds.max,
                        new Vector2(bounds.min.x, bounds.max.y),
                    };
                }
                else
                {
                    return new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(1, 0),
                        new Vector2(1, 1),
                        new Vector2(0, 1),
                    };
                }
            }

            /************************************************************************************************************************/

            private static void HandleAddRemovePoints(HitData[] hits, TransitionPreviewDetails details, float time)
            {
                var currentEvent = Event.current;
                if (!currentEvent.shift)
                    return;

                AnimancerGUI.RepaintEverything();

                var transform = details.Transform;

                HitData closestHit = null;
                var closestDistance = float.PositiveInfinity;
                var closestPointIndex = -1;
                var closestLineIntersect = default(Vector3);
                var isClosetoPoint = false;

                for (int iHit = 0; iHit < hits.Length; iHit++)
                {
                    var hit = hits[iHit];
                    if (!hit.IsActiveAt(time))
                        continue;

                    var area = hit.Area;
                    var points = GetTemporaryPoints(area.Length + 1);
                    GetPointsInWorldSpace(area, points, transform, true);

                    for (int iPoint = 0; iPoint < area.Length; iPoint++)
                    {
                        var point = points[iPoint];

                        // Distance to point.
                        var distance = HandleUtility.DistanceToCircle(point, 0);
                        if (distance <= 15 && (closestPointIndex < 0 || closestDistance > distance))
                        {
                            closestHit = hit;
                            closestDistance = distance;
                            closestPointIndex = iPoint;
                            isClosetoPoint = true;
                        }
                    }

                    // Distance to line.
                    if (closestPointIndex < 0)
                    {
                        if (TryEvaluateProximityToPolyLine(points, area.Length + 1, closestDistance,
                            out var index, out var distance, out var closestPoint))
                        {
                            closestHit = hit;
                            closestDistance = distance;
                            closestPointIndex = index;
                            closestLineIntersect = closestPoint;
                        }
                    }
                }

                if (closestHit == null)
                    return;

                var color = Handles.color;
                Handles.color = new Color(0.75f, 1, 0.75f);

                if (isClosetoPoint)
                {
                    ref var area = ref closestHit.Area;

                    if (currentEvent.type == EventType.MouseDown && area.Length > 3)
                    {
                        TransitionPreviewDetails.Property.RecordUndo("Remove Hit Point");
                        PlatformerUtilities.RemoveAt(ref area, closestPointIndex);
                        currentEvent.Use();
                    }
                    else
                    {
                        PlatformerUtilities.DoHandle2D(area[closestPointIndex], 2);
                    }
                }
                else
                {
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        var ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                        if (!CalculateRayTargetXY(ray, out var point, transform.position.z))
                            return;

                        point = transform.InverseTransformPoint(point);

                        if (Event.current.control)
                            point = PlatformerUtilities.RoundToPixel(transform.gameObject, point);

                        TransitionPreviewDetails.Property.RecordUndo("Add Hit Point");
                        PlatformerUtilities.InsertAt(ref closestHit.Area, closestPointIndex + 1, point);

                        currentEvent.Use();
                    }
                    else
                    {
                        PlatformerUtilities.DoHandle2D(closestLineIntersect, 1.5f);
                    }
                }

                Handles.color = color;
            }

            /************************************************************************************************************************/

            private static void DoPreviewSceneGUI(HitData hit, TransitionPreviewDetails details)
            {
                if (!PreviewSceneGUIRequiresCurrentEvent)
                    return;

                var transform = details.Transform;
                var area = hit.Area;
                var count = area.Length;

                var color = Handles.color;
                Handles.color = new Color(0.5f, 1, 0.5f);

                var points = GetTemporaryPoints(count + 1);
                GetPointsInWorldSpace(area, points, transform, true);

                var bounds = new Bounds(points[0], default);

                for (int i = 0; i < count; i++)
                {
                    var point = points[i];
                    bounds.Encapsulate(point);

                    EditorGUI.BeginChangeCheck();

                    point = PlatformerUtilities.DoHandle2D(point);

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (Event.current.control)
                            point = PlatformerUtilities.RoundToPixel(transform.gameObject, point);

                        TransitionPreviewDetails.Property.RecordUndo("Edit Hit Area");
                        area[i] = transform.InverseTransformPoint(point);
                        TransitionPreviewDetails.Property.serializedObject.Update();
                    }
                }

                Handles.color = new Color(0.25f, 1, 0.25f);

                Handles.DrawAAPolyLine(10, count + 1, points);

                Handles.color = color;
            }

            /************************************************************************************************************************/

            private static void GetPointsInWorldSpace(Vector2[] local, Vector3[] world, Transform transform, bool wrapLast)
            {
                for (int i = 0; i < local.Length; i++)
                {
                    world[i] = transform.TransformPoint(local[i]);
                }

                if (wrapLast)
                    world[local.Length] = world[0];
            }

            /************************************************************************************************************************/

            public static bool TryEvaluateProximityToPolyLine(Vector3[] vertices, int vertexCount, float maxDistance,
                out int index, out float distance, out Vector3 closestPoint)
            {
                distance = maxDistance;
                index = -1;

                vertexCount--;
                for (int i = 0; i < vertexCount; i++)
                {
                    var newDistance = HandleUtility.DistanceToLine(vertices[i], vertices[i + 1]);
                    if (distance > newDistance)
                    {
                        distance = newDistance;
                        index = i;
                    }
                }

                if (distance >= maxDistance)
                {
                    closestPoint = default;
                    return false;
                }

                var pointA = vertices[index];
                var pointB = vertices[index + 1];
                var mouseOffset = Event.current.mousePosition - HandleUtility.WorldToGUIPoint(pointA);
                var guiLine = HandleUtility.WorldToGUIPoint(pointB) - HandleUtility.WorldToGUIPoint(pointA);

                var magnitude = guiLine.magnitude;
                var projection = Vector3.Dot(guiLine, mouseOffset);
                if (magnitude > 1E-06f)
                    projection /= magnitude * magnitude;
                projection = Mathf.Clamp01(projection);

                closestPoint = Vector3.Lerp(pointA, pointB, projection);
                return true;
            }

            /************************************************************************************************************************/

            public static bool CalculateRayTargetXY(Ray ray, out Vector3 target, float z = 0)
            {
                target = ray.origin;

                var direction = ray.direction;
                if (direction.z == 0)
                {
                    target.z = z;
                    return false;
                }

                var offset = ray.origin.z - z;
                target -= direction * (offset / direction.z);
                target.z = z;
                return Math.Sign(offset) != Math.Sign(direction.z);
            }

            /************************************************************************************************************************/

            private static float GetHandleSize(Vector3 position) => HandleUtility.GetHandleSize(position) * 0.05f;

            /************************************************************************************************************************/

            private static Vector3[] _Points;

            public static Vector3[] GetTemporaryPoints(int minCount)
            {
                if (_Points == null || _Points.Length < minCount)
                    _Points = new Vector3[Mathf.NextPowerOfTwo(minCount)];

                return _Points;
            }

            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
        }
    }
}

#endif