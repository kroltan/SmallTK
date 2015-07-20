using System.Collections.Generic;
using System.Drawing;

using Microsoft.SmallBasic.Library;

using OpenTK;

using OpenTKLearn;

namespace SmallTK {
    /// <summary>
    /// Handles the creation and edition of sprites in the game, including movement, scaling and rotation.
    /// </summary>
    [SmallBasicType]
    public static class Sprites {
        private static readonly Dictionary<string, Bitmap> SpriteCache = new Dictionary<string, Bitmap>();
        /// <summary>
        /// Adds a new sprite to the game at the center of the screen
        /// </summary>
        /// <param name="imageLocation">The path to the image to use</param>
        /// <returns>The sprite that was just created</returns>
        public static Primitive Create(Primitive imageLocation) {
            var sprite = new Sprite();
            CacheSetSprite(sprite, imageLocation);
            GameEngine.Game.Sprites.Add(sprite);
            return GameEngine.Game.Sprites.Count - 1;
        }

        /// <summary>
        /// Removes the given sprite from the game.
        /// </summary>
        /// <param name="sprite">The sprite to remove</param>
        /// <returns>True if the sprite was removed, false otherwise.</returns>
        public static Primitive Remove(Primitive sprite) {
            if (GameEngine.Game.Sprites.Count <= sprite) {
                return false;
            }
            GameEngine.Game.Sprites.RemoveAt(sprite);
            return true;
        }

        /// <summary>
        /// Changes the image of an existing sprite.
        /// </summary>
        /// <param name="sprite">The sprite to change</param>
        /// <param name="imageLocation">The new image</param>
        /// <returns>True if the image was sucessfully changed, False otherwise.</returns>
        public static Primitive ChangeImage(Primitive sprite, Primitive imageLocation) {
            Sprite spr;
            if (!TryGetSprite(sprite, out spr)) {
                return false;
            }
            CacheSetSprite(spr, imageLocation);
            return true;
        }

        /// <summary>
        /// Changes the position of the sprite.
        /// </summary>
        /// <param name="sprite">The sprite to move</param>
        /// <param name="x">New horizontal coordinate of sprite</param>
        /// <param name="y">New vertical coordinate of sprite</param>
        /// <returns>True if the sprite could be moved, false otherwise</returns>
        public static Primitive Move(Primitive sprite, Primitive x, Primitive y) {
            Sprite spr;
            if (!TryGetSprite(sprite, out spr)) {
                return false;
            }
            spr.Position = new Vector3(x, y, spr.Position.Z);
            return true;
        }

        /// <summary>
        /// Changes the size of the sprite on each axis.
        /// </summary>
        /// <param name="sprite">The sprite to resize</param>
        /// <param name="x">The horizontal size</param>
        /// <param name="y">The vertical size</param>
        /// <returns>True if the sprite could be resized, False otherwise</returns>
        public static Primitive Resize(Primitive sprite, Primitive x, Primitive y) {
            Sprite spr;
            if (!TryGetSprite(sprite, out spr)) {
                return false;
            }
            spr.Scale = new Vector3(x, y, 1);
            return true;
        }

        /// <summary>
        /// Changes the rotation angle of the sprite to the given value.
        /// </summary>
        /// <param name="sprite">The sprite to rotate</param>
        /// <param name="angle">The new angle</param>
        /// <returns>True if the sprite could be rotated, false otherwise</returns>
        public static Primitive Rotate(Primitive sprite, Primitive angle) {
            Sprite spr;
            if (!TryGetSprite(sprite, out spr)) {
                return false;
            }
            spr.Angle = angle;
            return true;
        }

        private static bool TryGetSprite(int spriteIndex, out Sprite sprite) {
            if (GameEngine.Game.Sprites.Count > spriteIndex) {
                sprite = GameEngine.Game.Sprites[spriteIndex];
                return true;
            }
            sprite = null;
            return false;
        }

        internal static void CacheSetSprite(Sprite sprite, string imageLocation) {
            if (SpriteCache.ContainsKey(imageLocation)) {
                sprite.SetBitmap(SpriteCache[imageLocation]);
            } else {
                var bitmap = new Bitmap(imageLocation);
                SpriteCache[imageLocation] = bitmap;
                sprite.SetBitmap(bitmap);
            }
        }

        internal static void ClearCache() {
            foreach (var kvp in SpriteCache) {
                kvp.Value.Dispose();
            }
            SpriteCache.Clear();
        }
    }
}
