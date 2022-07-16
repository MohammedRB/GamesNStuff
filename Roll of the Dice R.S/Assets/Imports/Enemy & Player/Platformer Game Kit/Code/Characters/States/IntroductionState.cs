// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#if ! UNITY_EDITOR
#pragma warning disable CS0618 // Type or member is obsolete (for PlayableAssetState in Animancer Lite).
#endif
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>
    /// A <see cref="CharacterState"/> that plays a <see cref="UnityEngine.Playables.PlayableAsset"/> (such as a
    /// <see cref="UnityEngine.Timeline.TimelineAsset"/>)
    /// then returns to idle.
    /// </summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/introduction">Introduction</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/IntroductionState
    /// 
    [AddComponentMenu(MenuPrefix + "Introduction State")]
    [HelpURL(APIDocumentation + nameof(IntroductionState))]
    public sealed class IntroductionState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private PlayableAssetTransition _Animation;

        /************************************************************************************************************************/

        private void Awake()
        {
            if (!_Animation.IsValid)
                return;

            _Animation.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
            Character.StateMachine.TrySetState(this);
        }

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            Character.Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/

        public override bool CanExitState => false;

        /************************************************************************************************************************/

        public override void OnExitState()
        {
            Character.Animancer.States.Destroy(_Animation);
        }

        /************************************************************************************************************************/
    }
}