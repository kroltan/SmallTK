using System.Runtime.InteropServices;

using OpenTK;

namespace OpenTKLearn {
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex {
        public readonly Vector3 Position;
        public readonly Vector2 TexCoord;

        public Vertex(float x, float y, float z, float u, float v) {
            Position = new Vector3(x, y, z);
            TexCoord = new Vector2(u, v);
        }
    }
}
