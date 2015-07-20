using System;
using System.Linq;

using ObjectTK;
using ObjectTK.Shaders;
using ObjectTK.Tools;
using ObjectTK.Tools.Cameras;

using OpenTK;
using OpenTK.Graphics;

namespace OpenTKLearn {
    /// <summary>
    /// Provides common functionality for the examples.
    /// </summary>
    public class GameWindow : DerpWindow {
        protected Camera Camera;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;
        protected string OriginalTitle { get; private set; }

        private double _microTime;
        private readonly double[] _updateTimeSamples;
        private int _updateCursor;
        private bool _microTimeDirty;

        protected float MicroTime {
            get {
                if (_microTimeDirty) {
                    _microTime = _updateTimeSamples.Average() * 1000000;
                }
                return (float)_microTime;
                
            }
        }

        public GameWindow()
            : base(800, 600, GraphicsMode.Default, "") {
            _updateTimeSamples = new double[60];
            for (var i = _updateTimeSamples.Length - 1; i >= 0; i--) {
                _updateTimeSamples[i] = TargetUpdatePeriod;
            }
            // disable vsync
            VSync = VSyncMode.On;

            

            // set up camera
            Camera = new Camera();
            Camera.SetBehavior(new NullCameraBehaviour());
            Camera.ResetToDefault();
            Camera.Enable(this);
            ResetMatrices();
            // hook up events
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e) {
            // maximize window
            WindowState = WindowState.Maximized;
            // remember original title
            OriginalTitle = Title;
            // set search path for shader files and extension
            ProgramFactory.BasePath = "Data/Shaders/";
            ProgramFactory.Extension = "glsl";
        }

        private void OnUnload(object sender, EventArgs e) {
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e) {
            // display FPS in the window title
            Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
            _updateTimeSamples[_updateCursor] = UpdateTime;
            _updateCursor = _updateCursor % _updateTimeSamples.Length;
            _microTimeDirty = true;
        }

        /// <summary>
        /// Resets the ModelView and Projection matrices to the identity.
        /// </summary>
        protected void ResetMatrices() {
            ModelView = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        /// <summary>
        /// Sets a perspective projection matrix and applies the camera transformation on the modelview matrix.
        /// </summary>
        protected void SetupPerspective(float fovy) {
            // setup perspective projection
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(fovy, aspectRatio, 0.1f, 1000);
            ModelView = Matrix4.Identity;
            // apply camera transform
            ModelView = Camera.GetCameraTransform();
        }

        protected void SetupOrthographic(float size) {
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreateOrthographic(size * aspectRatio, size, 0.1f, float.MaxValue);
            ModelView = Matrix4.Identity;
            // apply camera transform
            ModelView = Camera.GetCameraTransform();
        }

        public static void Main(string[] args) {
            using (var start = new TestGame()) {
                start.Run(60.0, 60.0);
            }
        }
    }
}