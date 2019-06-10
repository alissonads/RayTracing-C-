using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Textures.Base;
using RayTracing.Util;
using RayTracing.Util.Math;

namespace RayTracing.Textures
{
    class TInstance : ITexture
    {
        private ITexture texture;
        private Mat4 invMatrix;

        public ITexture Texture
        {
            get { return texture; }
            set
            {
                texture = value;
            }
        }

        public TInstance()
        {
            invMatrix = Mat4.NewIdentity();
        }

        public TInstance(ITexture texture)
        {
            this.texture = texture;
            invMatrix = Mat4.NewIdentity();
        }

        public TInstance(TInstance other)
        {
            if(other.texture != null)
                texture = other.texture.Clone();
            invMatrix = Mat4.NewIdentity();
        }

        public ITexture Clone()
        {
            return new TInstance(this);
        }

        public Vec3 GetColor(ShadeRec sr)
        {
            ShadeRec localSr = sr.Clone();
            localSr.LocalHitPoint = MathUtils.TransformPoint(invMatrix, sr.LocalHitPoint);
            return texture.GetColor(localSr);
        }

        public TInstance Translate(float x, float y, float z)
        {
            invMatrix *= Mat4.NewAffTranslation(-x, -y, -z);

            return this;
        }

        public TInstance Translate(Vec3 pos)
        {
            invMatrix *= Mat4.NewAffTranslation(pos.Neg());

            return this;
        }

        public TInstance RotateX(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationX(ang).Transpose();

            return this;
        }

        public TInstance RotateY(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationY(ang).Transpose();

            return this;
        }

        public TInstance RotateZ(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationZ(ang).Transpose();

            return this;
        }

        public TInstance Scale(float x, float y, float z)
        {
            invMatrix *= Mat4.NewAffScale(1 / x, 1 / y, 1 / z);

            return this;
        }

        public TInstance Scale(Vec3 scale)
        {
            return Scale((float)scale.X, (float)scale.Y, (float)scale.Z);
        }

        public TInstance Scale(float scale)
        {
            return Scale(scale, scale, scale);
        }
    }
}
