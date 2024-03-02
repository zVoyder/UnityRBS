namespace RBS.Editor.Utility
{
    using System;
    using UnityEngine;

    public static class RBSInputs
    {
        #region EVENTS

        public static event Action<Vector2> OnLeftMouseButtonDown;

        public static event Action<Vector2> OnRightMouseButtonDown;

        public static event Action<float> OnShiftScrollWheel;

        public static event Action<float> OnShiftAltScrollWheel;

        public static event Action OnShiftRKeyDown;

        public static event Action OnShiftFKeyDow;

        public static event Action OnSpaceKeyDown;

        #endregion EVENTS

        public static void Process()
        {
            Event currentEvent = Event.current;
            EventType eventType = currentEvent.type;

            if (eventType == EventType.MouseDown)
            {
                switch (currentEvent.button)
                {
                    case 0:
                        OnLeftMouseButtonDown?.Invoke(currentEvent.mousePosition);
                        break;

                    case 1:
                        OnRightMouseButtonDown?.Invoke(currentEvent.mousePosition);
                        break;
                }
            }

            if (eventType == EventType.KeyDown)
            {
                if (currentEvent.shift)
                {
                    if (currentEvent.keyCode == KeyCode.R)
                        OnShiftRKeyDown?.Invoke();

                    if (currentEvent.keyCode == KeyCode.F)
                        OnShiftFKeyDow?.Invoke();
                }

                if (currentEvent.keyCode == KeyCode.Space)
                    OnSpaceKeyDown?.Invoke();
            }

            if (eventType == EventType.ScrollWheel)
            {
                if (currentEvent.shift)
                {
                    if (currentEvent.alt)
                    {
                        OnShiftAltScrollWheel?.Invoke(-Mathf.Sign(currentEvent.delta.x));
                        return;
                    }

                    OnShiftScrollWheel?.Invoke(-Mathf.Sign(currentEvent.delta.x));
                }
            }
        }
    }
}