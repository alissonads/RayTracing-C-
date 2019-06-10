using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.Util.Math;
using RayTracing.World.Base;

namespace RayTracing.Cameras.Base
{
    abstract class Camera
    {
        protected Vec3 eye;
        protected Vec3 lookat;
        protected Vec3 up;
        protected Vec3 u, v, w;
        protected float exposureTime;
        protected float ra;

        public Vec3 Eye
        {
            get { return eye; }
            set
            {
                eye = value;
            }
        }

        public Vec3 Lookat
        {
            get { return lookat; }
            set
            {
                lookat = value;
            }
        }

        public Vec3 Up
        {
            get { return up; }
            set
            {
                up = value;
            }
        }

        public float ExposureTime
        {
            set
            {
                exposureTime = value;
            }
        }

        public float RollAngle
        {
            set
            {
                ra = value;
            }
        }

        public Camera()
        {
            eye = new Vec3(0.0, 0.0, -500.0);
            lookat = new Vec3(0.0);
            up = new Vec3(0.0, 1.0, 0.0);
            u = new Vec3(1.0, 0.0, 0.0);
            v = new Vec3(0.0, 1.0, 0.0);
            w = new Vec3(0.0, 0.0, 1.0);
            exposureTime = 1.0f;
            ra = 0.0f;
        }

        public Camera(Vec3 eye, Vec3 lookat, Vec3 up)
        {
            this.eye = eye;
            this.lookat = lookat;
            this.up = up;
            exposureTime = 1.0f;
            ra = 0.0f;
        }

        public Camera(Camera other)
        {
            eye = other.eye.Clone();
            lookat = other.lookat.Clone();
            up = other.up.Clone();
            u = other.u.Clone();
            v = other.v.Clone();
            w = other.w.Clone();
            exposureTime = other.exposureTime;
            ra = other.ra;
        }

        public void SetEye(double x, double y, double z)
        {
            eye.Set(x, y, z);
        }

        public void SetLookat(double x, double y, double z)
        {
            lookat.Set(x, y, z);
        }

        public void SetUp(double x, double y, double z)
        {
            up.Set(x, y, z);
        }

        public void ComputeUVW()
        {
            w = (eye - lookat).Normalize();

            Vec3 _up = Vec3.Rotate(up, w, ra);

            if (eye.X == lookat.X && eye.Z == lookat.Z && eye.Y > lookat.Y)
            {
                u.Set(0.0, 0.0, 1.0);
                v.Set(1.0, 0.0, 0.0);
                w.Set(0.0, 1.0, 0.0);
                return;
            }

            if (eye.X == lookat.X && eye.Z == lookat.Z && eye.Y < lookat.Y)
            {
                u.Set(1.0, 0.0, 0.0);
                v.Set(0.0, 0.0, 1.0);
                w.Set(0.0, -1.0, 0.0);
                return;
            }

            u = Vec3.Cross(_up, w).Normalize();
            v = Vec3.Cross(w, u).Normalize();
            
        }

        public abstract Camera Clone();

        public abstract void RenderScene(SceneBase world);

        public abstract void MultiThreadRenderScene(SceneBase world);
    }
}
