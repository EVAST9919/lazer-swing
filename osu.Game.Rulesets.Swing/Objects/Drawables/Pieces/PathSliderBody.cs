using System;
using System.Runtime.InteropServices;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shaders.Types;
using osu.Game.Graphics;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public partial class PathSliderBody : Drawable, IHasAccentColour
    {
        private const float shadow_size = 7;
        private static readonly float radius = SwingHitObject.DEFAULT_SIZE / 4 + shadow_size;
        private static readonly float half_playfiled = SwingPlayfield.FULL_SIZE.Y / 2;

        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                accentColour = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        private float headAngle;

        public float HeadAngle
        {
            get => headAngle;
            set
            {
                headAngle = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        private float tailAngle;

        public float TailAngle
        {
            get => tailAngle;
            set
            {
                tailAngle = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        public PathSliderBody()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.Centre;
            Size = new Vector2((half_playfiled + radius) * 2);
            Rotation = 90;
        }

        private IShader shader;

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "SwingSlider");
        }

        protected override DrawNode CreateDrawNode() => new PathSliderBodyDrawNode(this);

        private class PathSliderBodyDrawNode : DrawNode
        {
            public new PathSliderBody Source => (PathSliderBody)base.Source;

            public PathSliderBodyDrawNode(PathSliderBody source)
                : base(source)
            {
            }

            private float headAngle;
            private float tailAngle;
            private Vector4 accent;
            private IShader shader;
            private Vector2 drawSize;
            private float texelSize;

            public override void ApplyState()
            {
                base.ApplyState();

                shader = Source.shader;
                drawSize = Source.DrawSize;

                headAngle = Source.headAngle / 180f * (float)Math.PI;
                tailAngle = Source.tailAngle / 180f * (float)Math.PI;
                accent = new Vector4(Source.accentColour.R, Source.accentColour.G, Source.accentColour.B, Source.accentColour.A);
                texelSize = 1.5f / drawSize.X;
            }

            private IUniformBuffer<SliderBodyParameters> parametersBuffer;

            public override void Draw(IRenderer renderer)
            {
                if (tailAngle > headAngle)
                    return;

                base.Draw(renderer);

                shader.Bind();

                parametersBuffer ??= renderer.CreateUniformBuffer<SliderBodyParameters>();
                parametersBuffer.Data = new SliderBodyParameters
                {
                    Accent = accent,
                    HeadAngle = headAngle,
                    TailAngle = tailAngle,
                    TexelSize = texelSize
                };
                shader.BindUniformBlock("m_SliderBodyParameters", parametersBuffer);

                Quad quad = new Quad(
                    Vector2Extensions.Transform(Vector2.Zero, DrawInfo.Matrix),
                    Vector2Extensions.Transform(new Vector2(drawSize.X, 0f), DrawInfo.Matrix),
                    Vector2Extensions.Transform(new Vector2(0f, drawSize.Y), DrawInfo.Matrix),
                    Vector2Extensions.Transform(drawSize, DrawInfo.Matrix)
                );

                renderer.DrawQuad(renderer.WhitePixel, quad, DrawColourInfo.Colour);

                shader.Unbind();
            }

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);
                parametersBuffer?.Dispose();
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private record struct SliderBodyParameters
            {
                public UniformVector4 Accent;
                public UniformFloat HeadAngle;
                public UniformFloat TailAngle;
                public UniformFloat TexelSize;
                private readonly UniformPadding4 pad;
            }
        }
    }
}
