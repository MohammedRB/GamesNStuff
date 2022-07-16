// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

namespace Animancer
{
    /// <summary>Sets the <see cref="AnimancerPlayable.DefaultFadeDuration"/> to 0 on startup (editor and runtime).</summary>
    internal static class DefaultFadeDuration
    {
        /************************************************************************************************************************/

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() => AnimancerPlayable.DefaultFadeDuration = 0;

        /************************************************************************************************************************/
    }
}