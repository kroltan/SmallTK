using Microsoft.SmallBasic.Library;

using OpenTK;

namespace SmallTK {
    /// <summary>
    /// Master object, where the main game events are defined
    /// </summary>
    [SmallBasicType]
    public static class GameEngine {
        internal static SmallTkGame Game;

        /// <summary>
        /// This is called every frame, and is where most of the game happens.
        /// </summary>
        public static event SmallBasicCallback OnUpdate;

        /// <summary>
        /// This is called when the engine starts and is ready to go.
        /// </summary>
        public static event SmallBasicCallback OnLoad;

        /// <summary>
        /// This is called after the engine has finished closing.
        /// </summary>
        public static event SmallBasicCallback OnStop;

        /// <summary>
        /// Starts the engine, trying to run at the given framerate.
        /// </summary>
        /// <param name="fps">The target framerate</param>
        public static void Start(Primitive fps) {
            using (Game = new SmallTkGame()) {
                Game.Run(fps, fps);
            }
        }

        /// <summary>
        /// Stops the game, and calls OnStop.
        /// </summary>
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
