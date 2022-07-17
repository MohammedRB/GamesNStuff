// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
        public int tempDmg;
        public bool checkDmg = true;

        public BoxCharacterBody2D gravity;
        public float tempGrav;
        public bool checkGrav = true;

        public GameObject[] diceFaces;
        public AudioSource diceRoll;

        public void Roll(){
            diceRoll.Play();

            float randNum = Random.Range(1f,117.75f);
            
            if (checkGrav == true){
                tempGrav = gravity._GravityScale;
                checkGrav = false;
            }

            if (checkDmg == true){
                tempDmg = _hitData.Damage;
                checkDmg = false;
            }
            
            if (randNum < 2 ){
                RemoveEffects();
                _health.InstaDeath();
                MakeActive(0);
            }
            else if(randNum >= 2 && randNum <= 12){
                RemoveEffects();
                _WalkSpeed = 3;
                _RunSpeed = 5;
                MakeActive(1);
            }
            else if(randNum > 12 && randNum <= 22){
                RemoveEffects();
                gravity._GravityScale = gravity._GravityScale + 1;
                MakeActive(2);
            }
            else if(randNum > 22 && randNum <= 32){
                RemoveEffects();
                _health.SetMaximumHealth(50, Health.HealthChangeMode.Offset);
                MakeActive(3);
            }
            else if(randNum > 32 && randNum <= 42){
                RemoveEffects();
                _hitData.Damage = _hitData.Damage - 3;
                MakeActive(4);
            }
            else if(randNum > 42 && randNum <= 55.75){
                RemoveEffects();
                _WalkSpeed = 9;
                _RunSpeed = 13;
                MakeActive(5);
            }
            else if(randNum > 55.75 && randNum <= 69.5){
                RemoveEffects();
                _health.SetMaximumHealth(150, Health.HealthChangeMode.Offset);
                MakeActive(6);
            }
            else if(randNum > 69.5 && randNum <= 86.25){
                RemoveEffects();
                gravity._GravityScale = gravity._GravityScale - 1;
                MakeActive(7);
            }
            else if(randNum > 86.25 && randNum <= 100){
                RemoveEffects();
                _hitData.Damage = _hitData.Damage + 3;
                MakeActive(8);
            }
            else if(randNum > 100 && randNum <= 117.75){
                RemoveEffects();

                foreach (GameObject item in diceFaces)
                {
                    item.SetActive(false);
                }
            }
        }

        public void MakeActive(int itemIdx){
            for (int i = 0; i < diceFaces.Length; i++){
                if (i == itemIdx){
                    diceFaces[i].SetActive(true);
                }
                else{
                    diceFaces[i].SetActive(false);
                }
            }
        }

        public void RemoveEffects(){
            _WalkSpeed = 6;
            _RunSpeed = 9;
            gravity._GravityScale = tempGrav;
            _hitData.Damage = tempDmg;
        }
    }
}