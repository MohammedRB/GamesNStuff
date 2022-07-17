// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;

namespace PlatformerGameKit.Characters
{
    /// <summary>Moves a ground-based <see cref="Character"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/physics#ground-character-movement">
    /// Physics - Ground Character Movement</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters/GroundCharacterMovement
    /// 
    [AddComponentMenu(Character.MenuPrefix + "Ground Character Movement")]
    [HelpURL(Character.APIDocumentation + nameof(GroundCharacterMovement))]
    public sealed class GroundCharacterMovement : CharacterMovement
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecond] private float _WalkSpeed = 6;
        [SerializeField, MetersPerSecond] private float _RunSpeed = 9;
        [SerializeField, Seconds] private float _WalkSmoothing = 0;
        [SerializeField, Seconds] private float _RunSmoothing = 0.15f;
        [SerializeField, Seconds] private float _AirSmoothing = 0.3f;
        [SerializeField, Seconds] private float _FrictionlessSmoothing = 0.3f;
        [SerializeField] private float _GripFriction = 0.4f;

        private float _SmoothingSpeed;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _WalkSpeed);
            PlatformerUtilities.NotNegative(ref _RunSpeed);
            PlatformerUtilities.NotNegative(ref _WalkSmoothing);
            PlatformerUtilities.NotNegative(ref _RunSmoothing);
            PlatformerUtilities.NotNegative(ref _AirSmoothing);
            PlatformerUtilities.NotNegative(ref _FrictionlessSmoothing);
            PlatformerUtilities.NotNegative(ref _GripFriction);
        }
#endif

        /************************************************************************************************************************/

        protected override Vector2 UpdateVelocity(Vector2 velocity)
        {
            var brainMovement = Character.MovementDirection.x;
            var currentState = Character.StateMachine.CurrentState;

            var targetSpeed = Character.Run ? _RunSpeed : _WalkSpeed;
            targetSpeed *= brainMovement * currentState.MovementSpeedMultiplier;

            if (!Character.Body.IsGrounded)
            {
                velocity.x = PlatformerUtilities.SmoothDamp(velocity.x, targetSpeed, ref _SmoothingSpeed, _AirSmoothing);
                return velocity;
            }

            var direction = Vector2.right;
            var ground = Character.Body.GroundContact;

            var smoothing = CalculateGroundSmoothing(ground.Collider.friction);

            // Calculate the horizontal speed, excluding the movement of the platform.
            var platformVelocity = ground.Velocity;
            velocity -= platformVelocity;
            var currentSpeed = Vector2.Dot(direction, velocity);

            // Remove the old horizontal speed from the velocity.
            velocity -= direction * currentSpeed;

            // Move the horizontal speed towards the target.
            currentSpeed = PlatformerUtilities.SmoothDamp(currentSpeed, targetSpeed, ref _SmoothingSpeed, smoothing);

            // Add the new horizontal speed and platform velocity back into the actual velocity.
            velocity += direction * currentSpeed + platformVelocity;

            return velocity;
        }

        /************************************************************************************************************************/

        /// <summary>Calculates the speed smoothing time based on the running state and contact friction.</summary>
        private float CalculateGroundSmoothing(float friction)
        {
            var target = Character.Run ? _RunSmoothing : _WalkSmoothing;
            if (_GripFriction == 0)
                return target;

            return Mathf.Lerp(_FrictionlessSmoothing, target, friction / _GripFriction);
        }

        /************************************************************************************************************************/

        public Health _health;
        public HitData _hitData;

        int dmgStr = _hitData.Damage;
        public void Roll(){
            float randNum = Random.Range(1f,100f);
            
            if (randNum == 1 ){
                _health.InstaDeath();
            }
            else if(randNum > 1 && randNum <= 12){
                _WalkSpeed = 3;
                _RunSpeed = 5;
            }
            else if(randNum > 12 && randNum <= 22){
                // jump down
            }
            else if(randNum > 22 && randNum <= 32){
                _health.SetMaximumHealth(50, Health.HealthChangeMode.Offset);
            }
            else if(randNum > 32 && randNum <= 42){
                _hitData.Damage = dmgStr - 3;
            }
            else if(randNum > 42 && randNum <= 55.75){
                _WalkSpeed = 9;
                _RunSpeed = 13;
            }
            else if(randNum > 55.75 && randNum <= 69.5){
                _health.SetMaximumHealth(150, Health.HealthChangeMode.Offset);
            }
            else if(randNum > 69.5 && randNum <= 86.25){
                // jump up
            }
            else if(randNum > 86.25 && randNum <= 100){
                //strength up
            }
        }

        //IEnumerator Effect
    }
}