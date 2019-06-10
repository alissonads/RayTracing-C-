using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;

namespace RayTracing.Util
{
    using Math;
    using Materials.Base;

    sealed class ShadeRec
    {
        private bool hitAnObject;
        private IMaterial material;
        private Vec3 hitPoint;
        private Vec3 localHitPoint;
        private Vec3 normal;
        private Vec3 color;
        private Ray ray;
        private int depth;
        private Vec3 dir;
        private SceneBase world;
        private double tmin;
        private double u, v;

        public ShadeRec()
        {
            hitAnObject = false;
            localHitPoint = new Vec3();
            hitPoint = new Vec3();
            normal = new Vec3();
            color = new Vec3();
            ray = new Ray();
            depth = 0;
            dir = new Vec3();
            tmin = 0.0;
        }

        public ShadeRec(SceneBase world)
        {
            hitAnObject = false;
            localHitPoint = new Vec3();
            hitPoint = new Vec3();
            normal = new Vec3();
            color = new Vec3();
            ray = new Ray();
            depth = 0;
            dir = new Vec3();
            this.world = world;
            tmin = 0.0;
        }

        public ShadeRec(ShadeRec other)
        {
            hitAnObject = other.hitAnObject;
            localHitPoint = other.localHitPoint.Clone();
            hitPoint = other.hitPoint.Clone();
            normal = other.normal.Clone();
            color = other.color.Clone();
            ray = other.ray;
            depth = other.depth;
            dir = other.dir.Clone();
            tmin = other.tmin;
            world = other.world;
            u = other.u;
            v = other.v;

            if(other.material != null)
                material = other.material.Clone();

    }

        public ShadeRec Clone()
        {
            return new ShadeRec(this);
        }

        public bool HitAnObject
        {
            get { return hitAnObject; }
            set { hitAnObject = value; }
        }

        public IMaterial Material
        {
            get { return material; }
            set
            {
                material = value;
            }
        }

        public Vec3 LocalHitPoint
        {
            get { return localHitPoint; }
            set { localHitPoint = value; }
        }

        public Vec3 HitPoint
        {
            get { return hitPoint; }
            set
            {
                hitPoint = value;
            }
        }

        public Vec3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public Vec3 Color
        {
            get { return color; }
            set { color = value; }
        }

        public SceneBase World
        {
            get { return world; }
            set { world = value; }
        }

        public Ray Ray
        {
            get { return ray; }
            set
            {
                ray = value;
            }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public Vec3 Dir
        {
            get { return dir; }
            set
            {
                dir = value;
            }
        }

        public double Tmin
        {
            get { return tmin; }
            set { tmin = value; }
        }

        public double U
        {
            get { return u; }
            set
            {
                u = value;
            }
        }

        public double V
        {
            get { return v; }
            set
            {
                v = value;
            }
        }
    }
}

