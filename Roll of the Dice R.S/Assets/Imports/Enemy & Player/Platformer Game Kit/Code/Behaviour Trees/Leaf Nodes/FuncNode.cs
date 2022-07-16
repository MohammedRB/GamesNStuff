// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A <see cref="LeafNode"/> which invokes a <see cref="Func{T}"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/FuncNode
    /// 
    public sealed class FuncNode : LeafNode
    {
        /************************************************************************************************************************/

        /// <summary>The delegate which will be invoked by <see cref="Execute"/>.</summary>
        public Func<Result> Func { get; set; }

        /************************************************************************************************************************/

        /// <summary>Creates a new <see cref="FuncNode"/>.</summary>
        public FuncNode() { }

        /// <summary>Creates a new <see cref="FuncNode"/>.</summary>
        public FuncNode(Func<Result> func)
        {
            Func = func;
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            return Func.Invoke();
        }

        /************************************************************************************************************************/
    }
}