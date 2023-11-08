using Coimbra.Services;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JIH.Input
{
    public readonly partial struct StartInputMoveEvent : IEvent
    {
        public readonly Vector2 MoveAxisMovement;
        public readonly bool IsMoving;

        public StartInputMoveEvent(Vector2 moveAxisMovement, bool isMoving)
        {
            MoveAxisMovement = moveAxisMovement;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct PerformInputMoveEvent : IEvent
    {
        public readonly Vector2 MoveAxisMovement;
        public readonly bool IsMoving;

        public PerformInputMoveEvent(Vector2 moveAxisMovement, bool isMoving)
        {
            MoveAxisMovement = moveAxisMovement;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct CancelInputMoveEvent : IEvent
    {
        public readonly Vector2 MoveAxisMovement;
        public readonly bool IsMoving;

        public CancelInputMoveEvent(Vector2 moveAxisMovement, bool isMoving)
        {
            IsMoving = isMoving;
            MoveAxisMovement = moveAxisMovement;
        }
    }

    public class InputManager : MonoBehaviour
    {
        private InputActions _inputsActions;
        private readonly List<EventHandle> _eventHandles = new();

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            _inputsActions = new InputActions();
            _inputsActions.Player.Enable();
            _inputsActions.Player.Move.started += MoveOnStarted;
            _inputsActions.Player.Move.performed += MoveOnPerformed;
            _inputsActions.Player.Move.canceled += MoveOnCanceled;
        }

        private void MoveOnStarted(InputAction.CallbackContext context)
        {
            new StartInputMoveEvent(context.ReadValue<Vector2>(), context.performed).Invoke(this);
        }

        private void MoveOnPerformed(InputAction.CallbackContext context)
        {
            new PerformInputMoveEvent(context.ReadValue<Vector2>(), context.performed).Invoke(this);
        }

        private void MoveOnCanceled(InputAction.CallbackContext context)
        {
            new CancelInputMoveEvent(context.ReadValue<Vector2>(), context.performed).Invoke(this);
        }

        private static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }

        private void Dispose()
        {
            _inputsActions.Player.Move.Dispose();

            IEventService eventService = ServiceLocator.GetChecked<IEventService>();

            foreach (EventHandle eventHandle in _eventHandles)
            {
                eventService.RemoveListener(eventHandle);
            }

            _eventHandles.Clear();
        }
    }
}
