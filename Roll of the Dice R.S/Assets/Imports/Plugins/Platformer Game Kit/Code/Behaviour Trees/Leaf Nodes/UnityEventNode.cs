// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A <see cref="LeafNode"/> which invokes a <see cref="UnityEvent"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/UnityEventNode
    /// 
    [Serializable]
    public sealed class UnityEventNode : LeafNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The delegate which will be invoked by " + nameof(Execute))]
        private UnityEvent _Action;

        /// <summary>The delegate which will be invoked by <see cref="Execute"/>.</summary>
        public ref UnityEvent Action => ref _Action;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            try
            {
                _Action?.Invoke();
                return Result.Pass;
            }
            catch
            {
                return Result.Fail;
            }
        }

        /************************************************************************************************************************/
    }
}