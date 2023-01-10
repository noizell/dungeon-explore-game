using KinematicCharacterController;
using NPP.DE.Core.Factory;
using System;
using UnityEngine;

namespace NPP.DE.Core.Character
{

    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class BaseCharacter : MonoBehaviour
    {
        BaseController _controller;
        KinematicCharacterMotor _motor;
        Vector3 _moveInputs;
        bool _triggerOnce;

        public void Initialize(PlayerControllerFactory playerFactory)
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            _controller = playerFactory.Create();
            _motor.CharacterController = _controller;
            _controller.Initialize(_motor, 4f, 6f, 8f);
        }

        private void Update()
        {

            _moveInputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            _controller.Fire(_moveInputs);

            Debug.Log(_controller.CurrentState);
        }

    }
}