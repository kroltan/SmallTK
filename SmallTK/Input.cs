using System;

using OpenTK;
using OpenTK.Input;

using Mouse = OpenTK.Input.Mouse;

namespace SmallTK {
    public static class Input {

        private static Vector3 MousePosition {
            get {
                var state = Mouse.GetCursorState();
                return new Vector3(state.X, state.Y, 0);
            }
        }
        public static bool IsKeyDown(string key) {
            Key actualKey;
            return Enum.TryParse(key, true, out actualKey) 
                && Keyboard.GetState().IsKeyDown(actualKey);
        }

        public static float MouseX {
            get { return MousePosition.X; }
        }

        public static float MouseY {
            get { return MousePosition.Y; }
        }

        public static float ScrollWheel {
            get { return Mouse.GetState().WheelPrecise; }
        }

        public static bool IsMouseDown(string button) {
            var btn = button.ToLower();
            switch (btn) {
                case "left":
                    return Mouse.GetState().LeftButton == ButtonState.Pressed;
                case "right":
                    return Mouse.GetState().RightButton == ButtonState.Pressed;
                case "middle":
                    return Mouse.GetState().MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
    }
}
