// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A <see cref="ScriptableObject"/> representing the relationships between factions.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/teams">Teams</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Team
    /// 
    [CreateAssetMenu(
        menuName = Strings.MenuPrefix + nameof(Team),
        fileName = nameof(Team))]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(Team))]
    public sealed class Team : ScriptableObject
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("Other teams are enemies by default unless they are in this list")]
        private Team[] _Allies;
        public ref Team[] Allies => ref _Allies;

#if UNITY_EDITOR
        [SerializeField, TextArea]
        private string _EditorDescription;
#endif

        /************************************************************************************************************************/
    }

    /************************************************************************************************************************/

    public static partial class PlatformerUtilities
    {
        /************************************************************************************************************************/

        public static bool IsAlly(this Team team, Team other)
            => team != null && (team == other || Array.IndexOf(team.Allies, other) >= 0);

        public static bool IsEnemy(this Team team, Team other)
            => !team.IsAlly(other);

        /************************************************************************************************************************/
    }
}