using Animancer;
using KinematicCharacterController;
using NPP.DE.Core.Factory;
using Stateless;
using System.Collections.Generic;
using UnityEngine;

namespace NPP.DE.Core.Character
{
    public enum CharacterState
    {
        Idle,
        Move,
        Attack
    }

    public enum CharacterTrigger
    {
        OnIdle,
        OnMove,
        OnAttack
    }

    public abstract class BaseController : ICharacterController, IFactoryMember
    {
        public class AnimationController
        {
            private AnimancerComponent _animancerComponent;
            private AnimancerClip[] _clips;
            private Dictionary<string, AnimationClip> _clipDict = new Dictionary<string, AnimationClip>();

            public AnimationController(AnimancerComponent animancerComponent, AnimancerClip[] clips)
            {
                _animancerComponent = animancerComponent;
                _clips = clips;

                for (int i = 0; i < _clips.Length; i++)
                {
                    _clipDict.Add(_clips[i].AnimationName, _clips[i].Clip);
                }
            }

            public void Play(string Name)
            {
                _animancerComponent.Play(_clipDict[Name]);
            }

            public void Crossfade(string name, float fadeDuration = .2f)
            {
                _animancerComponent.Play(_clipDict[name], fadeDuration);
            }

        }

        //States
        protected StateMachine<CharacterState, CharacterTrigger> Machine;
        protected StateMachine<CharacterState, CharacterTrigger>.TriggerWithParameters<Vector3> MoveTrigger;
        public CharacterState CurrentState => Machine.State;

        //Movement
        protected KinematicCharacterMotor Motor;
        protected Vector3 MoveInputVector;
        protected float MaxStableMoveSpeed;
        protected float StableMovementSharpness;
        protected float OrientationSharpness;

        //Misc
        protected bool EnableAnimationController;
        protected AnimationController AnimationControllers;

        protected BaseController()
        {

        }

        public void Initialize(KinematicCharacterMotor motor, float maxMoveSpeed, float movementSharpness, float orientationSharpness, bool enableAnimationController = true)
        {
            InitializeState();

            Motor = motor;
            Motor.CharacterController = this;
            MaxStableMoveSpeed = maxMoveSpeed;
            StableMovementSharpness = movementSharpness;
            OrientationSharpness = orientationSharpness;
            EnableAnimationController = enableAnimationController;
        }

        public void InitializeAnimationController(AnimancerComponent animancerComponent, AnimancerClip[] clips)
        {
            AnimationControllers = new AnimationController(animancerComponent, clips);
        }

        private void InitializeState()
        {
            Machine = new StateMachine<CharacterState, CharacterTrigger>(CharacterState.Idle);
            MoveTrigger = Machine.SetTriggerParameters<Vector3>(CharacterTrigger.OnMove);

            Machine.Configure(CharacterState.Idle)
                .PermitDynamic(CharacterTrigger.OnMove, OnCheckMovement)
                .OnEntryFrom(MoveTrigger, OnMoveExecuted)
                .OnEntry(() =>
                {
                    if (EnableAnimationController)
                    {
                        AnimationControllers.Crossfade("Idle");
                    }
                });

            Machine.Configure(CharacterState.Move)
                .PermitDynamic(CharacterTrigger.OnMove, OnCheckMovementFromMove)
                .OnEntryFrom(MoveTrigger, OnMoveExecuted)
                .OnEntry(() =>
                {
                    if (EnableAnimationController)
                    {
                        AnimationControllers.Crossfade("Run");
                    }
                });
        }

        private CharacterState OnCheckMovementFromMove()
        {
            if (Machine.IsInState(CharacterState.Move) && IsValidMove())
                return CharacterState.Move;

            return CharacterState.Idle;
        }

        private CharacterState OnCheckMovement()
        {
            if (IsValidMove() && !Machine.IsInState(CharacterState.Move))
                return CharacterState.Move;

            return CharacterState.Idle;
        }

        public void Fire(CharacterTrigger trigger)
        {
            Machine.Fire(trigger);
        }

        public void Fire(Vector3 param)
        {
            Machine.Fire(MoveTrigger, param);
        }

        protected void OnMoveExecuted(Vector3 move)
        {
            MoveInputVector = move;
        }

        protected bool IsValidMove()
        {
            return MoveInputVector != Vector3.zero;
        }

        public virtual void AfterCharacterUpdate(float deltaTime)
        {

        }

        public virtual void BeforeCharacterUpdate(float deltaTime)
        {

        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void PostGroundingUpdate(float deltaTime)
        {

        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, MoveInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {

            float currentVelocityMagnitude = currentVelocity.magnitude;

            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            // Reorient velocity on slope
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(MoveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * MoveInputVector.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));

        }
    }
}