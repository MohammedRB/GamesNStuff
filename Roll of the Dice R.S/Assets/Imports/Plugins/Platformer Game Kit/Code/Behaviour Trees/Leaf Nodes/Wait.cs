// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer.Units;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// A <see cref="LeafNode"/> which waits for a specific amount of time before returning
    /// <see cref="Result.Pass"/>.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/Wait
    /// 
    [Serializable]
    public sealed class Wait : LeafNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        [Tooltip("The number of seconds to wait")]
        private float _Duration;

        /// <summary>The number of seconds to wait.</summary>
        public ref float Duration => ref _Duration;

        private float _ElapsedTime;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            _ElapsedTime += Time.deltaTime;
            if (_ElapsedTime < _Duration)
                return Result.Pending;

            _ElapsedTime = 0;
            return Result.Pass;
        }

        /************************************************************************************************************************/
    }
}