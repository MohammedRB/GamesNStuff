// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;
using UnityEngine;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// A <see cref="ModifierNode"/> which executes its <see cref="ModifierNode.Child"/> but ignores the
    /// <see cref="BehaviourTrees.Result"/> it gives to return a fixed <see cref="Result"/> instead.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#modifiers">
    /// Behaviour Tree Brains - Modifiers</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/Ignore
    /// 
    [Serializable]
    public sealed class Ignore : ModifierNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The result which this node returns after executing its child")]
        private Result _Result = Result.Pass;

        /// <summary>The <see cref="BehaviourTrees.Result"/> which this node returns after executing its child.</summary>
        public ref Result Result => ref _Result;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            Child?.Execute();
            return _Result;
        }

        /************************************************************************************************************************/
    }
}