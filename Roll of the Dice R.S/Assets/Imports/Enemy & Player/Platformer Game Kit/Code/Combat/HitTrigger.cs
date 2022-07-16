// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using PlatformerGameKit.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A component which uses a <see cref="PolygonCollider2D"/> trigger to <see cref="Hit"/> things.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/melee">Melee Attacks</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/HitTrigger
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Hit Trigger")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(HitTrigger))]
    public sealed class HitTrigger : MonoBehaviour
    {
        /************************************************************************************************************************/
        #region Prefab
        /************************************************************************************************************************/

        private static HitTrigger _Prefab;
        private static bool _HasLoadedPrefab;

        /// <summary>The prefab from which all <see cref="HitTrigger"/> instances are cloned.</summary>
        /// <remarks>
        /// The prefab must be located in the foot of a <see cref="Resources"/> folder and its name must be
        /// <c>HitTrigger.prefab</c> for this property to load it.
        /// <para></para>
        /// This would be much better with
        /// <see href="https://kybernetik.com.au/weaver/docs/asset-injection">Weaver's Asset Injection</see> system
        /// because the code could look like this:
        /// <para></para>
        /// <code>[AssetReference] private static HitTrigger Prefab;</code>
        /// <para></para>
        /// No need for a property with multiple backing fields and no need to have a prefab at a specific hard coded
        /// file path. It's just a single field which can be assigned in the Weaver window and will then be given its
        /// reference automatically at runtime.
        /// </remarks>
        private static HitTrigger Prefab
        {
            get
            {
                if (!_HasLoadedPrefab)// Faster than null-checking the instance every time.
                {
                    _HasLoadedPrefab = true;
                    _Prefab = Resources.Load<HitTrigger>(nameof(HitTrigger));
                    AnimancerUtilities.Assert(_Prefab != null,
                        $"There is no '{nameof(HitTrigger)}.prefab' asset (with that exact name) in the root of a Resources folder");
                }

                return _Prefab;
            }
        }

        /************************************************************************************************************************/

        private static int _HitLayers;

        /// <summary>The physics layers targeted by attacks.</summary>
        /// <remarks>
        /// This value is automatically determined based on the layer of the <see cref="HitTrigger"/> prefab.
        /// <para></para>
        /// In the example scenes which use the default layers, this mask will simply include all layers. But when the
        /// <see cref="Physics2D"/> collision matrix is properly configured the <see cref="HitTrigger"/> should be on a
        /// layer that only interacts with a few other layers so that hit queries don't waste performance considering
        /// objects that can't receive hits (such as platforms and other static parts of the environment).
        /// </remarks>
        public static int HitLayers
        {
            get
            {
                if (_HitLayers == 0)
                    _HitLayers = Physics2D.GetLayerCollisionMask(Prefab.gameObject.layer);
                return _HitLayers;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Object Pool
        // This would be much simpler with Weaver's Object Pool system: https://kybernetik.com.au/weaver/docs/misc/object-pooling
        // Animancer's ObjectPool doesn't support MonoBehaviour components.
        /************************************************************************************************************************/

        private static readonly List<HitTrigger>
            SpareInstances = new List<HitTrigger>();

        /************************************************************************************************************************/

        public static HitTrigger Activate(Character character, HitData data, bool flipX, HashSet<Hit.ITarget> ignore)
        {
            AnimancerUtilities.Assert(character != null,
                $"{nameof(Characters.Character)} is null.");
            AnimancerUtilities.Assert(data != null,
                $"{nameof(HitData)} is null.");
            AnimancerUtilities.Assert(data.Area != null,
                $"{nameof(HitData)}.{nameof(HitData.Area)} is null.");

            var instance = GetInstance();

            instance.Parent = character.Animancer.transform;
            instance.Character = character;
            instance.Data = data;
            instance.Ignore = ignore;

            var area = data.Area;
            if (!flipX)
            {
                instance.Collider.points = area;
            }
            else
            {
                var points = ObjectPool.AcquireList<Vector2>();

                var count = area.Length;
                for (int i = 0; i < count; i++)
                {
                    var point = area[i];
                    point.x = -point.x;
                    points.Add(point);
                }

                instance.Collider.SetPath(0, points);
                ObjectPool.Release(points);
            }

#if UNITY_EDITOR
            instance.name = $"{character.name}: {data.Damage}";
#endif

            instance.Transform.SetPositionAndRotation(instance.Parent.position, instance.Parent.rotation);
            return instance;
        }

        /************************************************************************************************************************/

        private static HitTrigger GetInstance()
        {
            var count = SpareInstances.Count;
            if (count == 0)
            {
                return CreateInstance();
            }
            else
            {
                count--;
                var instance = SpareInstances[count];
                SpareInstances.RemoveAt(count);
                instance.gameObject.SetActive(true);
                return instance;
            }
        }

        /************************************************************************************************************************/

        private static Transform _Group;

        private static HitTrigger CreateInstance()
        {
            if (_Group == null)
            {
                var gameObject = new GameObject("Hit Triggers");
                DontDestroyOnLoad(gameObject);
                _Group = gameObject.transform;
            }

            var instance = Instantiate(Prefab, _Group);
            instance.Transform = instance.transform;
            return instance;
        }

        /************************************************************************************************************************/

        public static void PreAllocate(int capacity)
        {
            while (SpareInstances.Count < capacity)
                SpareInstances.Add(CreateInstance());
        }

        /************************************************************************************************************************/

        public void Deactivate()
        {
            if (this == null || !gameObject.activeSelf)
                return;

            gameObject.SetActive(false);

            Parent = null;
            Character = null;
            Data = null;
            Ignore = null;

            SpareInstances.Add(this);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/

        [SerializeField]
        private PolygonCollider2D _Collider;
        public PolygonCollider2D Collider => _Collider;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Collider);
        }
#endif

        /************************************************************************************************************************/

        // Set by CreateInstance.
        public Transform Transform { get; private set; }

        /************************************************************************************************************************/

        // Set by Activate.
        public Transform Parent { get; private set; }
        public Character Character { get; private set; }
        public HitData Data { get; private set; }
        public HashSet<Hit.ITarget> Ignore { get; private set; }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            // If the parent has been destroyed, they can no longer hit anything.
            if (Parent == null)
            {
                Deactivate();
                return;
            }

            Transform.SetPositionAndRotation(Parent.position, Parent.rotation);
        }

        /************************************************************************************************************************/

        private void OnTriggerEnter2D(Collider2D collider)
            => OnTriggerStay2D(collider);

        private void OnTriggerStay2D(Collider2D collider)
        {
            // The physics engine can still send trigger messages on the frame the trigger was deactivated.
            if (Data == null)
                return;

            AnimancerUtilities.Assert(Character != null,
                $"{nameof(Characters.Character)} has been destroyed but didn't release its {nameof(HitTrigger)}.");
            AnimancerUtilities.Assert(Character.gameObject.activeInHierarchy,
                $"{nameof(Characters.Character)} is inactive but didn't release its {nameof(HitTrigger)}.");

            var hit = new Hit(Character.transform, Character.Health.Team, Data.Damage, Ignore);
            hit.TryHitComponent(collider);
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Displays some non-serialized details at the bottom of the Inspector.</summary>
        /// <example>
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/pro">Inspector Gadgets Pro</see> would allow this to
        /// be implemented much easier by simply placing
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/docs/script-inspector/inspectable-attributes">
        /// Inspectable Attributes</see> on the properties we want to display like so:
        /// <para></para><code>
        /// [Inspectable]
        /// public Transform Parent { get; private set; }
        /// </code>
        /// </example>
        [UnityEditor.CustomEditor(typeof(HitTrigger), true)]
        public class Editor : UnityEditor.Editor
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                using (new UnityEditor.EditorGUI.DisabledScope(true))
                {
                    var target = (HitTrigger)this.target;
                    UnityEditor.EditorGUILayout.ObjectField("Parent", target.Parent, typeof(Transform), true);
                    UnityEditor.EditorGUILayout.ObjectField("Character", target.Character, typeof(Character), true);
                    UnityEditor.EditorGUILayout.LabelField("Hit Data", target.Data?.ToString());

                    UnityEditor.EditorGUILayout.LabelField("Ignore");
                    UnityEditor.EditorGUI.indentLevel++;
                    foreach (var item in target.Ignore)
                    {
                        if (item is Object obj)
                        {
                            UnityEditor.EditorGUILayout.ObjectField(obj, typeof(Object), true);
                        }
                        else
                        {
                            UnityEditor.EditorGUILayout.LabelField(item.ToString());
                        }
                    }
                    UnityEditor.EditorGUI.indentLevel--;
                }
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}