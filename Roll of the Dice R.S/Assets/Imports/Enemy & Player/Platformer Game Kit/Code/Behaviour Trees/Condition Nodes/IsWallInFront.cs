// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer.Units;
using PlatformerGameKit.Characters;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A <see cref="ConditionNode"/> which checks if a wall is in front of the character.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/specific#conditions">
    /// Behaviour Tree Brains - Conditions</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/IsWallInFront
    /// 
    [Serializable]
    public sealed class IsWallInFront : ConditionNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Meters(Rule = Value.IsNotNegative)]
        [Tooltip("The maximum distance within which to check (in meters)")]
        private float _Range = 1;

        /// <summary>The maximum distance within which to check (in meters).</summary>
        public ref float Range => ref _Range;

        [SerializeField]
        [Degrees(Rule = Value.IsNotNegative)]
        [Tooltip("The maximum angle away from fully vertical that a surface can be and still be considered a wall")]
        private float _WallAngle = 40;

        /// <summary>The maximum angle away from fully vertical that a surface can be and still be considered a wall.</summary>
        public ref float WallAngle => ref _WallAngle;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override bool Condition
        {
            get
            {
                var character = Context<Character>.Current;
                var bounds = character.Body.Collider.bounds;

                var origin = (Vector2)bounds.center;
                origin.y += character.Body.StepHeight * 0.5f;

                var size = (Vector2)bounds.size;
                size.y -= character.Body.StepHeight;

                var filter = character.Body.TerrainFilter;

                var baseAngle = character.MovementDirectionX < 0 ? 0 : 180;
                filter.SetNormalAngle(baseAngle - _WallAngle, baseAngle + _WallAngle);

                var count = Physics2D.BoxCast(
                    origin, size, character.Body.Rotation, character.MovementDirection, filter, PlatformerUtilities.OneRaycastHit, _Range);
                PlatformerUtilities.DrawBoxCast(origin, size, character.MovementDirection, Color.red);
                return count > 0;
            }
        }

        /************************************************************************************************************************/
    }
}