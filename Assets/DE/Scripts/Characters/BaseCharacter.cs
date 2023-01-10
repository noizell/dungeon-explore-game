using Animancer;
using KinematicCharacterController;
using UnityEngine;

namespace NPP.DE.Core.Character
{
    [System.Serializable]
    public struct AnimancerClip
    {
        public AnimationClip Clip;
        public string AnimationName;

        public AnimancerClip(AnimationClip clip, string animationName)
        {
            Clip = clip;
            AnimationName = animationName;
        }
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimancerComponent))]
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class BaseCharacter : MonoBehaviour
    {
        BaseController _controller;
        KinematicCharacterMotor _motor;
        Vector3 _moveInputs;

        [SerializeField]
        AnimancerClip[] _clips;

        public AnimancerClip[] Clips => _clips;

        public void Initialize(PlayerControllerFactory playerFactory)
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            _controller = playerFactory.Create();
            _motor.CharacterController = _controller;
            _controller.Initialize(_motor, 14f, 14f, 14f);
            _controller.InitializeAnimationController(GetComponent<AnimancerComponent>(), Clips);
        }

        private void Update()
        {
            _moveInputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            _controller.Fire(_moveInputs);

            Debug.Log(_controller.CurrentState);
        }

    }
}