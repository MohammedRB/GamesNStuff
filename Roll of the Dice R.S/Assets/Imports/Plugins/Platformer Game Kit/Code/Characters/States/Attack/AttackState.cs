// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="CharacterState"/> that plays an attack animation then returns to idle.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/attack">Attack</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/AttackState
    /// 
    [HelpURL(APIDocumentation + nameof(AttackState))]
    public abstract class AttackState : CharacterState
    {
        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override bool CanTurn => false;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override bool CanExitState => false;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void OnExitState()
        {
            base.OnExitState();
            Character.Animancer.EndHitSequence();
        }

        /************************************************************************************************************************/
    }
}