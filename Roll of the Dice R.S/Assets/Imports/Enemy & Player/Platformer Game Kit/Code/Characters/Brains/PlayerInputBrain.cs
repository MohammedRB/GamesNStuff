// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using PlatformerGameKit.Characters.States;
using UnityEngine;

namespace PlatformerGameKit.Characters.Brains
{
    /// <summary>A brain for characters controlled by local player <see cref="Input"/> (keyboard and mouse).</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/brains/player">Player Input Brain</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.Brains/PlayerInputBrain
    /// 
    [AddComponentMenu(MenuPrefix + "Player Input Brain")]
    [HelpURL(APIDocumentation + nameof(PlayerInputBrain))]
    public sealed class PlayerInputBrain : CharacterBrain
    {
        /************************************************************************************************************************/

        [Header("Input Names")]

        [SerializeField]
        [Tooltip("Space by default")]
        private string _JumpButton = "Jump";

        [SerializeField]
        [Tooltip("Left Click by default")]
        private string _PrimaryAttackButton = "Fire1";

        [SerializeField]
        [Tooltip("Right Click by default")]
        private string _SecondaryAttackButton = "Fire2";

        [SerializeField]
        [Tooltip("Left Shift by default")]
        private string _RunButton = "Fire3";

        [SerializeField]
        [Tooltip("A/D and Left/Right Arrows by default")]
        private string _XAxisName = "Horizontal";

        [SerializeField]
        [Tooltip("W/S and Up/Down Arrows by default")]
        private string _YAxisName = "Vertical";

        [Header("Actions")]
        [SerializeField] private CharacterState _Jump;
        [SerializeField] private CharacterState _PrimaryAttack;
        [SerializeField] private CharacterState _SecondaryAttack;

        private CharacterState _CurrentJumpState;

        /************************************************************************************************************************/

        private void Update()
        {
            if (_Jump != null)
            {
                // On press jump, enter the jump state and store the entered state (in case it's a MultiState).
                if (Input.GetButtonDown(_JumpButton) &&
                    Character.StateMachine.TrySetState(_Jump))
                    _CurrentJumpState = Character.StateMachine.CurrentState;

                // If currently in that state and jump is released, return to idle.
                if (_CurrentJumpState == Character.StateMachine.CurrentState &&
                    Input.GetButtonUp(_JumpButton))
                    Character.StateMachine.TrySetDefaultState();
            }

            if (_PrimaryAttack != null && Input.GetButtonDown(_PrimaryAttackButton))
                Character.StateMachine.TryResetState(_PrimaryAttack);

            if (_SecondaryAttack != null && Input.GetButtonDown(_SecondaryAttackButton))
                Character.StateMachine.TryResetState(_SecondaryAttack);

            Character.Run = Input.GetButton(_RunButton);

            Character.MovementDirection = new Vector2(
                Input.GetAxisRaw(_XAxisName),
                Input.GetAxisRaw(_YAxisName));
        }

        /************************************************************************************************************************/
    }
}