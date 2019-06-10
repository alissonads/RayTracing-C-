using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.GeometricObjects.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using RayTracing.Materials.Base;

namespace RayTracing.GeometricObjects
{
    class Instance : GeometricObject
    {
        private GeometricObject obj;
        private Mat4 invMatrix;
        private bool transformTheTexture;
        private BBox bbox;
        private static Mat4 forwardMatrix;

        public GeometricObject Object
        {
            get { return obj; }
            set
            {
                obj = value;
                material = obj.Material;
            }
        }

        public bool TransformTheTexture
        {
            get { return transformTheTexture; }
            set
            {
                transformTheTexture = value;
            }
        }

        public override BBox BoundingBox
        {
            get { return bbox; }
            set
            {
                bbox = value;
            }
        }

        public override IMaterial Material
        {
            get { return material; }

            set
            {
                obj.Material = value;
                base.Material = value;
            }
        }
        public Instance() :
            this(null)
        {}

        public Instance(GeometricObject obj) :
            this(obj, obj.Material)
        {
            this.obj = obj;
            invMatrix = Mat4.NewIdentity();
            transformTheTexture = true;
            forwardMatrix = Mat4.NewIdentity();
        }

        public Instance(GeometricObject obj, IMaterial material) :
            base()
        {
            this.obj = obj;
            this.material = material;
            invMatrix = Mat4.NewIdentity();
            transformTheTexture = true;
            forwardMatrix = Mat4.NewIdentity();
        }

        public Instance(Instance other) :
            base(other)
        {
            if(other.obj != null)
                obj = other.obj.Clone();
            if (other.bbox != null)
                bbox = other.bbox.Clone();

            invMatrix = other.invMatrix.Clone();
            transformTheTexture = other.transformTheTexture;
        }

        public override GeometricObject Clone()
        {
            return new Instance(this);
        }

        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            Ray invRay = new Ray(MathUtils.TransformPoint(invMatrix, ray.O),
                                 MathUtils.TransformDirection(invMatrix, ray.D));

            if(obj.Hit(invRay, ref tmin, sr))
            {
                sr.Normal = MathUtils.TransformNormal(invMatrix, sr.Normal);

                if (obj.Material != null)
                    material = obj.Material;

                if (!transformTheTexture)
                    sr.LocalHitPoint = ray.HitPoint(tmin);

                return true;
            }

            return false;
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows)
                return false;

            Ray invRay = new Ray(MathUtils.TransformPoint(invMatrix, ray.O),
                                 MathUtils.TransformDirection(invMatrix, ray.D));

            return obj.ShadowHit(invRay, ref tmin);
        }

        public void ComputeBoundingBox()
        {
            BBox obj_bbox = obj.BoundingBox;

            Vec3[] v = new Vec3[]
            {
                new Vec3(obj_bbox.x0, obj_bbox.y0, obj_bbox.z0),
                new Vec3(obj_bbox.x1, obj_bbox.y0, obj_bbox.z0),
                new Vec3(obj_bbox.x1, obj_bbox.y1, obj_bbox.z0),
                new Vec3(obj_bbox.x0, obj_bbox.y1, obj_bbox.z0),

                new Vec3(obj_bbox.x0, obj_bbox.y0, obj_bbox.z1),
                new Vec3(obj_bbox.x1, obj_bbox.y0, obj_bbox.z1),
                new Vec3(obj_bbox.x1, obj_bbox.y1, obj_bbox.z1),
                new Vec3(obj_bbox.x0, obj_bbox.y1, obj_bbox.z1)
            };

            v[0] = MathUtils.TransformPoint(forwardMatrix, v[0]);
            v[1] = MathUtils.TransformPoint(forwardMatrix, v[1]);
            v[2] = MathUtils.TransformPoint(forwardMatrix, v[2]);
            v[3] = MathUtils.TransformPoint(forwardMatrix, v[3]);
            v[4] = MathUtils.TransformPoint(forwardMatrix, v[4]);
            v[5] = MathUtils.TransformPoint(forwardMatrix, v[5]);
            v[6] = MathUtils.TransformPoint(forwardMatrix, v[6]);
            v[7] = MathUtils.TransformPoint(forwardMatrix, v[7]);

            forwardMatrix.Identity();

            double x0 = MathUtils.HugeValue;
            double y0 = MathUtils.HugeValue;
            double z0 = MathUtils.HugeValue;

            for(int j = 0; j <= 7; j++)
            {
                if (v[j].X < x0)
                    x0 = v[j].X;
                if (v[j].Y < y0)
                    y0 = v[j].Y;
                if (v[j].Z < z0)
                    z0 = v[j].Z;
            }

            double x1 = -MathUtils.HugeValue;
            double y1 = -MathUtils.HugeValue;
            double z1 = -MathUtils.HugeValue;

            for (int j = 0; j <= 7; j++)
            {
                if (v[j].X > x1)
                    x1 = v[j].X;
                if (v[j].Y > y1)
                    y1 = v[j].Y;
                if (v[j].Z > z1)
                    z1 = v[j].Z;
            }

            bbox = new BBox(x0, x1, y0, y1, z0, z1);
        }

        public Instance Translate(float x, float y, float z)
        {
            invMatrix *= Mat4.NewAffTranslation(-x, -y, -z);
            forwardMatrix = Mat4.NewAffTranslation(x, y, z) * forwardMatrix;

            return this;
        }

        public Instance Translate(Vec3 pos)
        {
            invMatrix *= Mat4.NewAffTranslation(pos.Neg());
            forwardMatrix = Mat4.NewAffTranslation(pos) * forwardMatrix;

            return this;
        }

        public Instance RotateX(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationX(ang).Transpose();
            forwardMatrix = Mat4.NewRotationX(ang) * forwardMatrix;

            return this;
        }

        public Instance RotateY(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationY(ang).Transpose();
            forwardMatrix = Mat4.NewRotationY(ang) * forwardMatrix;

            return this;
        }

        public Instance RotateZ(float angle)
        {
            double ang = MathUtils.ToRadians(angle);
            invMatrix *= Mat4.NewRotationZ(ang).Transpose();
            forwardMatrix = Mat4.NewRotationZ(ang) * forwardMatrix;

            return this;
        }

        public Instance Scale(float x, float y, float z)
        {
            invMatrix *= Mat4.NewAffScale(1 / x, 1 / y, 1 / z);
            forwardMatrix = Mat4.NewAffScale(1 / x, 1 / y, 1 / z) * forwardMatrix;

            return this;
        }

        public Instance Scale(Vec3 scale)
        {
            return Scale((float)scale.X, (float)scale.Y, (float)scale.Z);
        }

        public Instance Scale(float scale)
        {
            return Scale(scale, scale, scale);
        }
    }
}
