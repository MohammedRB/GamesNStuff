// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#if UNITY_EDITOR

using Animancer;
using Animancer.Editor;
using UnityEditor;
using UnityEngine;

namespace PlatformerGameKit
{
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/AttackTransition
    partial class AttackTransition : ITransitionGUI
    {
        /************************************************************************************************************************/

        void ITransitionGUI.OnPreviewSceneGUI(TransitionPreviewDetails details)
            => HitData.Drawer.OnPreviewSceneGUI(ref _Hits, details, details.Animancer.States.GetOrCreate(this));

        /************************************************************************************************************************/

        void ITransitionGUI.OnTimelineBackgroundGUI()
            => HitData.Drawer.OnTimelineBackgroundGUI(_Hits);

        /************************************************************************************************************************/

        void ITransitionGUI.OnTimelineForegroundGUI()
            => HitData.Drawer.OnTimelineForegroundGUI(_Hits);

        /************************************************************************************************************************/

        /// <inheritdoc/>
        [CustomPropertyDrawer(typeof(AttackTransition), true)]
        public new class Drawer : ClipTransition.Drawer
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                var height = base.GetPropertyHeight(property, label);
                if (property.isExpanded)
                    height += AnimancerGUI.LineHeight + AnimancerGUI.StandardSpacing;
                return height;
            }

            /************************************************************************************************************************/

            /// <inheritdoc/>
            protected override void DoChildPropertyGUI(ref Rect area, SerializedProperty rootProperty,
                SerializedProperty property, GUIContent label)
            {
                if (property.propertyPath.EndsWith("." + nameof(_Hits)))
                {
                    DoDamageField(area);
                    AnimancerGUI.NextVerticalArea(ref area);
                }

                base.DoChildPropertyGUI(ref area, rootProperty, property, label);
            }

            /************************************************************************************************************************/

            private void DoDamageField(Rect area)
            {
                var transition = Context.Transition as AttackTransition;
                if (transition == null)
                    return;

                using (ObjectPool.Disposable.AcquireContent(out var content, "Damage"))
                {
                    var hits = transition.Hits;
                    if (hits.IsNullOrEmpty())
                    {
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUI.TextField(area, content, "No Hits");
                        return;
                    }

                    var showMixedValue = EditorGUI.showMixedValue;

                    var damage = hits[0].Damage;
                    for (int i = 0; i < hits.Length; i++)
                        if (hits[i].Damage != damage)
                            EditorGUI.showMixedValue = true;

                    EditorGUI.BeginChangeCheck();

                    damage = EditorGUI.IntField(area, content, damage);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Context.Property.RecordUndo();

                        if (damage < 0)
                            damage = 0;

                        for (int i = 0; i < hits.Length; i++)
                            hits[i].Damage = damage;
                    }

                    EditorGUI.showMixedValue = showMixedValue;
                }

            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
    }
}

#endif