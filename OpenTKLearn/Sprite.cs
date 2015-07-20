using System;
using System.Drawing;
using System.Drawing.Imaging;

using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKLearn {
    public class Sprite : IDisposable {
        private SpriteShaderProgram _program;
        private Buffer<Vertex> _vbo;
        private VertexArray _vao;
        private Sampler _sampler;
        private Texture2D _texture;

        private Vector3 _position, _scale;
        private float _angle;
        private bool _transformDirty;
        private Matrix4 _transform;
        private bool _initialized;

        public Texture2D Image {
            get { return _texture; }
            set {
                _texture = value;
                GenerateMesh(_texture.Width, _texture.Height);
                _program.Use();
                _sampler.Bind(TextureUnit.Texture0);
                _program.Texture.BindTexture(TextureUnit.Texture0, _texture);
            }
        }

        public Vector3 Scale {
            get { return _scale; }
            set {
                _scale = value;
                _transformDirty = true;
            }
        }

        public Vector3 Position {
            get { return _position; }
            set { 
                _position = value;
                _transformDirty = true;
            }
        }

        public float Angle {
            get { return _angle; }
            set {
                _angle = value;
                _transformDirty = true;
            }
        }

        private void RefreshTransform() {
            if (!_transformDirty) return;

            var position = Matrix4.CreateTranslation(_position);
            var scale = Matrix4.CreateScale(_scale);
            var rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_angle));
            _transform = position * scale * rotation;
            
            _transformDirty = false;
        }

        public Sprite() {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Angle = 0;
        }

        private void GenerateMesh(int width, int height) {
            if (_initialized) Dispose();
            
            _sampler = new Sampler();
            _sampler.SetWrapMode(TextureWrapMode.Clamp);

            _program = ProgramFactory.Create<SpriteShaderProgram>();

            var w = width / 2;
            var h = height / 2;

            var vertices = new[] {
                new Vertex(-w, h, 0, 0, 0),
                new Vertex( w, h, 0, 1, 0),
                new Vertex(-w,-h, 0, 0, 1),
                new Vertex( w,-h, 0, 1, 1)
            };

            _vbo = new Buffer<Vertex>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertices);

            _vao = new VertexArray();
            _vao.Bind();
            _vao.BindAttribute(_program.InPosition, _vbo);
            _vao.BindAttribute(_program.InTexCoord, _vbo, 12);
            _initialized = true;
        }

        public void SetBitmap(Bitmap bitmap, bool premultiply = true) {
            Texture2D tex;
            using (bitmap) {
                BitmapTexture.CreateCompatible(bitmap, out tex);
                if (premultiply) PremultiplyAlpha(bitmap);
                tex.LoadBitmap(bitmap);
            }
            tex.GenerateMipMaps();
            tex.Bind();
            tex.SetParameter(TextureParameterName.TextureMinFilter, (int)(tex.Levels > 1 ? TextureMinFilter.NearestMipmapNearest : TextureMinFilter.Nearest));
            tex.SetParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            Image = tex;
        }

        private void PremultiplyAlpha(Bitmap bitmap) {
            for (var x = bitmap.Width - 1; x >= 0; x--) {
                for (var y = bitmap.Height - 1; y >= 0; y--) {
                    var pixel = bitmap.GetPixel(x, y);
                    var pix = new Vector4(pixel.R, pixel.G, pixel.B, pixel.A) / 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(
                        pixel.A,
                        (int)(pix.X * pix.W * 255),
                        (int)(pix.Y * pix.W * 255),
                        (int)(pix.Z * pix.W * 255)
                    ));
                }
            }
        }

        public void Render(Matrix4 mvp) {
            if (!_initialized) {
                return;
            }
            RefreshTransform();
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            _program.Use();
            _program.ModelViewProjectionMatrix.Set(_transform * mvp);
            _vao.DrawArrays(PrimitiveType.TriangleStrip, 0, _vbo.ElementCount);
        }

        public void Dispose() {
            if (_initialized) {
                _sampler.Dispose();
                _texture.Dispose();
                _program.Dispose();
                _vao.Dispose();
                _vbo.Dispose();
            }
            _initialized = false;
        }
    }
}
