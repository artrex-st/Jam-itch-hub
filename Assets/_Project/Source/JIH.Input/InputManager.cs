using Coimbra.Services;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JIH.Input
{
    public readonly partial struct StartInputXEvent : IEvent
    {
        public readonly float AxisX;
        public readonly bool IsMoving;

        public StartInputXEvent(float axisX, bool isMoving)
        {
            AxisX = axisX;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct PerformInputXEvent : IEvent
    {
        public readonly float AxisX;
        public readonly bool IsMoving;

        public PerformInputXEvent(float axisX, bool isMoving)
        {
            AxisX = axisX;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct CancelInputXEvent : IEvent
    {
        public readonly float AxisX;
        public readonly bool IsMoving;

        public CancelInputXEvent(float axisX, bool isMoving)
        {
            IsMoving = isMoving;
            AxisX = axisX;
        }
    }

    public readonly partial struct StartInputYEvent : IEvent
    {
        public readonly float AxisY;
        public readonly bool IsMoving;

        public StartInputYEvent(float axisY, bool isMoving)
        {
            AxisY = axisY;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct PerformInputYEvent : IEvent
    {
        public readonly float AxisY;
        public readonly bool IsMoving;

        public PerformInputYEvent(float axisY, bool isMoving)
        {
            AxisY = axisY;
            IsMoving = isMoving;
        }
    }

    public readonly partial struct CancelInputYEvent : IEvent
    {
        public readonly float AxisY;
        public readonly bool IsMoving;

        public CancelInputYEvent(float axisY, bool isMoving)
        {
            IsMoving = isMoving;
            AxisY = axisY;
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

            _inputsActions.Player.Axis_X.started += MoveXStarted;
            _inputsActions.Player.Axis_X.performed += MoveXPerformed;
            _inputsActions.Player.Axis_X.canceled += MoveXCanceled;

            _inputsActions.Player.Axis_Y.started += MoveYStarted;
            _inputsActions.Player.Axis_Y.performed += MoveYPerformed;
            _inputsActions.Player.Axis_Y.canceled += MoveYCanceled;
        }

        private void MoveXStarted(InputAction.CallbackContext context)
        {
            new StartInputXEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private void MoveXPerformed(InputAction.CallbackContext context)
        {
            new PerformInputXEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private void MoveXCanceled(InputAction.CallbackContext context)
        {
            new CancelInputXEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private void MoveYStarted(InputAction.CallbackContext context)
        {
            new StartInputYEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private void MoveYPerformed(InputAction.CallbackContext context)
        {
            new PerformInputYEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private void MoveYCanceled(InputAction.CallbackContext context)
        {
            new CancelInputYEvent(context.ReadValue<float>(), context.performed).Invoke(this);
        }

        private static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }

        private void Dispose()
        {
            _inputsActions.Player.Axis_X.Dispose();
            _inputsActions.Player.Axis_Y.Dispose();

            IEventService eventService = ServiceLocator.GetChecked<IEventService>();

            foreach (EventHandle eventHandle in _eventHandles)
            {
                eventService.RemoveListener(eventHandle);
            }

            _eventHandles.Clear();
        }
    }
}
