// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using PlatformerGameKit.Characters.Brains;
using UnityEngine;

namespace PlatformerGameKit.Characters
{
    /// <summary>Base class for moving a <see cref="Character"/>.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/physics">Physics</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters/CharacterMovement
    /// 
    [HelpURL(Character.APIDocumentation + nameof(CharacterMovement))]
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public abstract class CharacterMovement : MonoBehaviour
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Run after brains to use their input but before <see cref="CharacterBody2D"/> to set the
        /// <see cref="CharacterBody2D.Velocity"/> before it gets used.
        /// </summary>
        public const int DefaultExecutionOrder = (CharacterBrain.DefaultExecutionOrder + CharacterBody2D.DefaultExecutionOrder) / 2;

        /// <summary>Any speed smaller than this is treated as zero.</summary>
        public const float MinimumSpeed = 0.01f;

        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;
        public ref Character Character => ref _Character;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            var previousVelocity = _Character.Body.Velocity;
            var velocity = UpdateVelocity(previousVelocity);

            // Ensure that a very small velocity is actually zero so the Rigidbody can go to sleep to improve performance.
            if (velocity.sqrMagnitude < MinimumSpeed * MinimumSpeed)
                velocity = default;

            if (previousVelocity.x != velocity.x ||// Don't use Vector2== because it's actually an approximation.
                previousVelocity.y != velocity.y)
                _Character.Body.Velocity = velocity;
            // Else let the Rigidbody go to sleep to improve performance.
        }

        /************************************************************************************************************************/

        protected abstract Vector2 UpdateVelocity(Vector2 velocity);

        /************************************************************************************************************************/
    }
}