using System.IO;

using Microsoft.SmallBasic.Library;

using ObjectTK.Shaders;

using OpenTK;


namespace SmallTK {
    public static class GameEngine {
        internal static SmallTkGame Game;

        public static SmallBasicCallback OnUpdate;
        public static SmallBasicCallback OnLoad;
        public static SmallBasicCallback OnStop;

        private static string _contentPath;
        public static string ContentPath {
            get { return _contentPath; }
            set {
                _contentPath = value;
                ProgramFactory.BasePath = Path.Combine(value, "Shaders");
            }
        }

        public static void Start(int fps) {
            using (Game = new SmallTkGame()) {
                Game.Run(fps, fps);
            }
        }
        public static void Stop() {
            Game.Close();
            Unload();
        }

        internal static void Unload() {
            if (OnStop != null) {
                OnStop();
            }
        }

        internal static void Load() {
            if (OnLoad != null) {
                OnLoad();
            }
        }

        internal static void Update(FrameEventArgs e) {
            if (OnUpdate != null) {
                OnUpdate();
            }
        }
    }
}
