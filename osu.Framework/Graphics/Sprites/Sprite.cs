//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using OpenTK.Graphics.ES20;
using OpenTK;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics.Sprites
{
    public class Sprite : Drawable
    {
        public event EventHandler OnDispose;

        public bool WrapTexture = false;

        public Sprite(Texture texture)
        {
            Texture = texture;
        }

        #region Disposal
        protected override void Dispose(bool isDisposing)
        {
            OnDispose?.Invoke(IsDisposable, null);

            if (IsDisposable && texture != null)
            {
                texture.Dispose();
                texture = null;
            }

            base.Dispose(isDisposing);
        }
        #endregion

        protected override DrawNode BaseDrawNode => new SpriteDrawNode(Game, DrawInfo, Texture, ScreenSpaceDrawQuad, WrapTexture);

        protected override bool CheckForcedPixelSnapping(Quad screenSpaceQuad)
        {
            return
                Rotation == 0
                && Math.Abs(screenSpaceQuad.Width - Math.Round(screenSpaceQuad.Width)) < 0.1f
                && Math.Abs(screenSpaceQuad.Height - Math.Round(screenSpaceQuad.Height)) < 0.1f;
        }

        private Texture texture;
        public virtual Texture Texture
        {
            get { return texture; }
            set
            {
                if (value == texture)
                    return;

                if (texture != null && IsDisposable)
                    texture.Dispose();

                texture = value;

                Size = new Vector2(texture?.DisplayWidth ?? 0, texture?.DisplayHeight ?? 0);
            }
        }

        public override Drawable Clone()
        {
            Sprite clone = (Sprite)base.Clone();
            clone.texture = texture;

            return clone;
        }

        public override string ToString()
        {
            return base.ToString() + $" tex: {texture?.AssetName}";
        }
    }
}
