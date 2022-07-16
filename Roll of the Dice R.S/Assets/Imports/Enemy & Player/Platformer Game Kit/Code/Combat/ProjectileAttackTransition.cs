// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#if ! UNITY_EDITOR
#pragma warning disable CS0618 // Type or member is obsolete (for Layers in Animancer Lite).
#endif

using Animancer;
using Animancer.Units;
using PlatformerGameKit.Characters;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using Animancer.Editor;
using UnityEditor;
#endif

namespace PlatformerGameKit
{
    /// <summary>A <see cref="ClipTransition"/> which launches a projectile.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/ranged">Ranged Attacks</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/ProjectileAttackTransition
    /// 
    [Serializable]
    [EventNames(EventName)]
    public partial class ProjectileAttackTransition : ClipTransition, ISerializationCallbackReceiver
#if UNITY_EDITOR
        , ITransitionGUI
#endif
    {
        /************************************************************************************************************************/

        public const string EventName = "Fire";

        [SerializeField]
        [Tooltip("The prefab that will be instantiated to create the projectile")]
        private Projectile _ProjectilePrefab;
        public ref Projectile ProjectilePrefab => ref _ProjectilePrefab;

        [SerializeField]
        [Tooltip("The local position where the projectile will be created")]
        private Vector2 _LaunchPoint;
        public ref Vector2 LaunchPoint => ref _LaunchPoint;

        [SerializeField, MetersPerSecond]
        [Tooltip("The initial speed the projectile will be given")]
        private float _LaunchSpeed;
        public ref float LaunchSpeed => ref _LaunchSpeed;

        [SerializeField]
        [Tooltip("The amount of damage the projectile will deal")]
        private int _Damage;
        public ref int Damage => ref _Damage;

        /************************************************************************************************************************/

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
#if ! UNITY_EDITOR
            // No events in Animancer Lite at runtime.
#pragma warning disable CS0162 // Unreachable code detected.
            if (!AnimancerUtilities.IsAnimancerPro)
                return;
#endif

            if (_ProjectilePrefab != null)
            {
#if UNITY_EDITOR
                // In the Edit Mode the sequence might have already been initialised or might not have the event.
                if (AnimancerEditorUtilities.PlayModeState != PlayModeStateChange.EnteredPlayMode)
                {
                    var index = Events.IndexOf(EventName);
                    if (index >= 0)
                    {
                        var callback = Events[index].callback;
                        if (callback != Fire)
                            Events.SetCallback(index, Fire);
                    }

                    return;
                }
#endif

                Events.SetCallback(EventName, Fire);
            }

#pragma warning restore CS0162 // Unreachable code detected.
        }

        /************************************************************************************************************************/

        private void Fire()
        {
            var attacker = CharacterAnimancerComponent.GetCurrent();
            var facing = attacker.Facing;
            var facingLeft = attacker.FacingLeft;

            var position = CalculateLaunchPosition(attacker.Character.Body.Position, facingLeft);

            var angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
            if (facingLeft)
                angle -= 180;

            var rotation = Quaternion.Euler(0, 0, angle);

            var projectile = Object.Instantiate(_ProjectilePrefab, position, rotation);
            projectile.Fire(facing * _LaunchSpeed, attacker.Character.Health.Team, _Damage, null);
            projectile.Renderer.flipX = facingLeft;
        }

        /************************************************************************************************************************/

        private Vector2 CalculateLaunchPosition(Vector2 position, bool flipX)
        {
            var launchPosition = _LaunchPoint;
            if (flipX)
                launchPosition.x = -launchPosition.x;

            return position + launchPosition;
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        private Transform _ProjectilePreviewInstance;

        void ITransitionGUI.OnPreviewSceneGUI(TransitionPreviewDetails details)
        {
            var position = (Vector2)details.Transform.position;

            var renderer = details.Transform.gameObject.GetComponentInParentOrChildren<SpriteRenderer>();
            var flipX = renderer != null && renderer.flipX;

            var launchPosition = CalculateLaunchPosition(position, flipX);

            if (_ProjectilePrefab != null)
            {
                if (_ProjectilePreviewInstance == null)
                    _ProjectilePreviewInstance = Object.Instantiate(_ProjectilePrefab, details.Transform.root).transform;

                _ProjectilePreviewInstance.position = launchPosition;
            }

            EditorGUI.BeginChangeCheck();

            Handles.color = new Color(0.5f, 1, 0.5f);
            launchPosition = PlatformerUtilities.DoHandle2D(launchPosition);

            if (EditorGUI.EndChangeCheck())
            {
                TransitionPreviewDetails.Property.RecordUndo("Edit Launch Point");

                launchPosition -= position;
                if (flipX)
                    launchPosition.x = -position.x;

                if (Event.current.control)
                    launchPosition = PlatformerUtilities.RoundToPixel(details.Transform.gameObject, launchPosition);

                _LaunchPoint = launchPosition;
            }
        }

        /************************************************************************************************************************/

        void ITransitionGUI.OnTimelineBackgroundGUI() { }

        void ITransitionGUI.OnTimelineForegroundGUI() { }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}