using System;

using Microsoft.SmallBasic.Library;

namespace SmallGame {
    /// <summary>
    /// The master object, controls the state of the game.
    /// With GameEngine, you can use events to create your game.
    /// </summary>
    [SmallBasicType]
    public static class GameEngine {
        /// <summary>
        /// This is called every frame, and is where most of the game happens.
        /// </summary>
        public static event SmallBasicCallback OnUpdate {
            add { SmallTK.GameEngine.OnUpdate += value; }
            // ReSharper disable once ValueParameterNotUsed
            remove { SmallTK.GameEngine.OnUpdate = null; }
        }

        /// <summary>
        /// This is called when the engine starts and is ready to go.
        /// </summary>
        public static event SmallBasicCallback OnLoad {
            add { SmallTK.GameEngine.OnLoad += value; }
            // ReSharper disable once ValueParameterNotUsed
            remove { SmallTK.GameEngine.OnLoad = null; }
        }

        /// <summary>
        /// This is called after the engine has finished closing.
        /// </summary>
        public static event SmallBasicCallback OnStop {
            add { SmallTK.GameEngine.OnStop += value; }
            // ReSharper disable once ValueParameterNotUsed
            remove { SmallTK.GameEngine.OnStop = null; }
        }

        /// <summary>
        /// Starts the engine, trying to run at the given framerate.
        /// </summary>
        /// <param name="fps">The target framerate</param>
        public static void Start(Primitive fps) {
            try {
                SmallTK.GameEngine.Start(fps);
            }
            catch (Exception e) {
                TextWindow.Show();
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Stops the game, and calls OnStop.
        /// </summary>
        public static void Stop() {
            SmallTK.GameEngine.Stop();
        }
    }
}
