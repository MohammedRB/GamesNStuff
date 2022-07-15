// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>An object with a <see cref="PlatformerGameKit.Team"/>.</summary>
    /// <example><code>
    /// class Character : MonoBehaviour, ITeam
    /// {
    ///     [SerializeField]
    ///     private Team _Team;
    ///     public Team Team => _Team;
    /// }
    /// </code></example>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/teams">Teams</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/ITeam
    /// 
    public interface ITeam
    {
        /// <summary>The <see cref="Platformer.Team"/> this object is on.</summary>
        Team Team { get; }
    }

    /************************************************************************************************************************/

    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/PlatformerUtilities
    public static partial class PlatformerUtilities
    {
        /************************************************************************************************************************/

        public static bool IsEnemy(this Team team, GameObject other)
            => team.IsEnemy(other.GetTeam());

        /************************************************************************************************************************/

        public static bool IsEnemy(GameObject gameObject, GameObject other)
            => gameObject.GetTeam().IsEnemy(other);

        /************************************************************************************************************************/

        public static Team GetTeam(this GameObject gameObject)
        {
            if (gameObject == null)
                return default;

            var team = gameObject.GetComponentInParent<ITeam>();
            if (team == null)
                return default;

            return team.Team;
        }

        /************************************************************************************************************************/
    }
}