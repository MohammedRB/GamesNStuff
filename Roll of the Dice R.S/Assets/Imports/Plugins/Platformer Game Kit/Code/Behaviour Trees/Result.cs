// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A trinary value: <see cref="Pending"/>, <see cref="Pass"/>, or <see cref="Fail"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/introduction#core-concept">
    /// Behaviour Tree Brains - Core Concept</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/Result
    /// 
    public enum Result
    {
        /// <summary>Neutral. Result is undetermined because execution has not completed.</summary>
        Pending = 0,

        /// <summary>Positive. The operation was completed successfully.</summary>
        Pass = 1,

        /// <summary>Negative. The operation was unsuccessful.</summary>
        Fail = -1,
    }

    /************************************************************************************************************************/

    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/BehaviourTreeUtilities
    /// 
    public static partial class BehaviourTreeUtilities
    {
        /************************************************************************************************************************/

        /// <summary>Returns a <see cref="Result"/> representing the `value`.</summary>
        public static Result ToResult(this bool value) => value ? Result.Pass : Result.Fail;

        /************************************************************************************************************************/

        /// <summary>Returns the opposite <see cref="Result"/> to the `result`.</summary>
        /// <example><list type="bullet">
        /// <item><see cref="Result.Pending"/> -> <see cref="Result.Pending"/></item>
        /// <item><see cref="Result.Pass"/> -> <see cref="Result.Fail"/></item>
        /// <item><see cref="Result.Fail"/> -> <see cref="Result.Pass"/></item>
        /// </list></example>
        public static Result Invert(this Result result) => (Result)(-(int)result);

        /************************************************************************************************************************/
    }
}