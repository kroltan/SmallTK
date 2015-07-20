using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTKLearn {
    class TestGame : GameWindow {
        private Sprite _sprite;
        private Vector3 _speed;

        public TestGame() {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private static Vector3 GetDirection() {
            var direction = Vector3.Zero;
            var state = OpenTK.Input.Keyboard.GetState();
            if (state[Key.Up] || state[Key.W]) {
                direction += Vector3.UnitY;
            }
            if (state[Key.Down] || state[Key.S]) {
                direction -= Vector3.UnitY;
            }
            if (state[Key.Left] || state[Key.A]) {
                direction -= Vector3.UnitX;
            }
            if (state[Key.Right] || state[Key.D]) {
                direction += Vector3.UnitX;
            }
            if (direction.LengthSquared > 0.1f) {
                return direction.Normalized();
            }
            return Vector3.Zero;
        }

        public void OnLoad(object sender, EventArgs e) {
            _sprite = new Sprite();
            _sprite.SetBitmap(new Bitmap("Data/Textures/Test.png"));
            _sprite.Scale = Vector3.One * 1;

            Camera.DefaultState.Position = new Vector3(0, 0, 100);
            Camera.DefaultState.Up = Vector3.UnitY;
            Camera.DefaultState.LookAt = -Vector3.UnitZ;
            Camera.ResetToDefault();

            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
        }

        public void OnUnload(object sender, EventArgs e) {
            _sprite.Dispose();
        }

        public void OnRenderFrame(object sender, FrameEventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            SetupOrthographic(Height);
            var mvp = ModelView * Projection;

            _speed = GetDirection() * 50;
            _sprite.Position += _speed * MicroTime;
            _sprite.Render(mvp);

            SwapBuffers();
        }
    }
}
