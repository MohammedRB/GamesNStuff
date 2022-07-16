// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer.Units;
using PlatformerGameKit.Characters;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// A <see cref="LeafNode"/> which sets the <see cref="Character.MovementDirectionY"/> to cause them to fly up and
    /// down in a sine wave.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/specific#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/SetMovementSine
    /// 
    [Serializable]
    public sealed class SetMovementSine : LeafNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Meters(Rule = Value.IsFinite)]
        [Tooltip("The height of the sine wave")]
        private float _Amplitude = 1;

        /// <summary>The height of the sine wave.</summary>
        public ref float Amplitude => ref _Amplitude;

        [SerializeField]
        [Meters(Rule = Value.IsFinite)]
        [Tooltip("The spacing between peaks of the sine wave")]
        private float _Frequency = 1;

        /// <summary>The spacing between peaks of the sine wave.</summary>
        public ref float Frequency => ref _Frequency;

        private float _BaseAltitude = float.NaN;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            var character = Context<Character>.Current;

#if UNITY_2020_1_OR_NEWER
            if (float.IsNaN(_BaseAltitude))
                _BaseAltitude = character.Body.Position.y;
#else
            // [SerializeReference] fields in Unity 2019 don't run their field initializers during deserialization.
            if (_BaseAltitude == 0)
            {
                _BaseAltitude = character.Body.Position.y;
                if (_BaseAltitude == 0)
                    _BaseAltitude = float.Epsilon;
            }
#endif

            var wave = Mathf.Sin(Time.timeSinceLevelLoad * _Frequency * Mathf.PI * 2) * _Amplitude;
            var targetAltitude = _BaseAltitude + wave;
            var currentAltitude = character.Body.Position.y;

            character.MovementDirectionY = Mathf.Clamp(targetAltitude - currentAltitude, -1, 1);

            return Result.Pass;
        }

        /************************************************************************************************************************/
    }
}