using System;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;


using GameWindow = OpenTKLearn.GameWindow;

namespace SmallTK {
    class SmallTkGame : GameWindow {
        public List<OpenTKLearn.Sprite> Sprites;

        public SmallTkGame() {
            Sprites = new List<OpenTKLearn.Sprite>();
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private void OnRenderFrame(object sender, FrameEventArgs e) {
            GameEngine.Update(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupOrthographic(Height);

            var mvp = ModelView * Projection;
            foreach (var sprite in Sprites) {
                sprite.Render(mvp);
            }

            SwapBuffers();
        }

        private void OnLoad(object sender, EventArgs e) {
            Camera.DefaultState.Position = new Vector3(0, 0, 100000);
            Camera.DefaultState.Up = Vector3.UnitY;
            Camera.DefaultState.LookAt = -Vector3.UnitZ;
            Camera.ResetToDefault();

            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            GameEngine.Load();
        }

        private void OnUnload(object sender, EventArgs e) {
            foreach (var sprite in Sprites) {
                sprite.Dispose();
            }
            Sprites.Clear();
            SmallTK.Sprites.ClearCache();
            GameEngine.Unload();
        }
    }
}
