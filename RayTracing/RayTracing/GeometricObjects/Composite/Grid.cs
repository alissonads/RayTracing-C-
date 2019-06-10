using RayTracing.GeometricObjects.Base;
using RayTracing.GeometricObjects.Triangles;
using RayTracing.Materials.Base;
using RayTracing.Util;
using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.GeometricObjects.Composite
{
    class Grid : Compound
    {
        private List<GeometricObject> cells;
        private BBox bbox;
        private int nx, ny, nz;
        private static double kEpsilon = 0.0001;

        public override BBox BoundingBox
        {
            get { return bbox; }

            set
            {
                bbox = value;
            }
        }

        public Grid() :
            base()
        {
            cells = new List<GeometricObject>();
        }

        public Grid(Grid other) :
            base(other)
        {
            cells = other.cells.ToList();
            nx = other.nx;
            ny = other.ny;
            nz = other.nz;

            if (other.bbox != null)
                bbox = other.bbox.Clone();
        }

        public override GeometricObject Clone()
        {
            return new Grid(this);
        }

        public void SetupCells()
        {
            Vec3 p0 = MinCoordinates();
            Vec3 p1 = MaxCoordinates();

            bbox = new BBox(p0, p1);

            int numObjects = objects.Count;

            double wx = p1.X - p0.X;
            double wy = p1.Y - p0.Y;
            double wz = p1.Z - p0.Z;

            double multiplier = 2.0;

            double s = Math.Pow(wx * wy * wz / numObjects, 0.3333333);
            nx = (int)(multiplier * wx / s + 1);
            ny = (int)(multiplier * wy / s + 1);
            nz = (int)(multiplier * wz / s + 1);

            int numCells = nx * ny * nz;
            cells.Capacity = numObjects;

            for (int j = 0; j < numCells; j++)
                cells.Add(null);

            List<int> counts = new List<int>(numCells);

            for (int j = 0; j < numCells; j++)
                counts.Add(0);

            BBox objBbox;
            int index;

            for(int j = 0; j < numObjects; j++)
            {
                objBbox = objects[j].BoundingBox;

                int ixmin = (int)MathUtils.Clamp((objBbox.x0 - p0.X) * nx / (p1.X - p0.X), 0, nx - 1);
                int iymin = (int)MathUtils.Clamp((objBbox.y0 - p0.Y) * ny / (p1.Y - p0.Y), 0, ny - 1);
                int izmin = (int)MathUtils.Clamp((objBbox.z0 - p0.Z) * nz / (p1.Z - p0.Z), 0, nz - 1);

                int ixmax = (int)MathUtils.Clamp((objBbox.x1 - p0.X) * nx / (p1.X - p0.X), 0, nx - 1);
                int iymax = (int)MathUtils.Clamp((objBbox.y1 - p0.Y) * ny / (p1.Y - p0.Y), 0, ny - 1);
                int izmax = (int)MathUtils.Clamp((objBbox.z1 - p0.Z) * nz / (p1.Z - p0.Z), 0, nz - 1);

                for(int iz = izmin; iz <= izmax; iz++)
                    for(int iy = iymin; iy <= iymax; iy++)
                        for(int ix = ixmin; ix <= ixmax; ix++)
                        {
                            index = ix + nx * iy + nx * ny * iz;

                            if(counts[index] == 0)
                            {
                                cells[index] = objects[j];
                                counts[index] += 1;
                            }
                            else
                            {
                                if(counts[index] == 1)
                                {
                                    Compound c = new Compound();
                                    c.AddObject(cells[index]);
                                    c.AddObject(objects[j]);
                                    cells[index] = c;
                                    counts[index] += 1;
                                }
                                else
                                {
                                    if (cells[index] is Compound)
                                        ((Compound)cells[index]).AddObject(objects[j]);
                                    counts[index] += 1;
                                }
                            }
                        }
            }

            objects.Clear();
            counts.Clear();
        }

        public void StoreMaterial(IMaterial material, int index)
        {
            objects[index].Material = material;
        }
        
        public override bool Hit(Ray ray, ref double tmin, ShadeRec sr)
        {
            if (!bbox.Hit(ray))
                return false;

            double ox = ray.O.X;
            double oy = ray.O.Y;
            double oz = ray.O.Z;
            double dx = ray.D.X;
            double dy = ray.D.Y;
            double dz = ray.D.Z;

            double x0 = bbox.x0;
            double y0 = bbox.y0;
            double z0 = bbox.z0;
            double x1 = bbox.x1;
            double y1 = bbox.y1;
            double z1 = bbox.z1;

            double txMin = 0, tyMin = 0, tzMin = 0;
            double txMax = 0, tyMax = 0, tzMax = 0;

            double a = 1.0 / dx;
            if(a >= 0)
            {
                txMin = (x0 - ox) * a;
                txMax = (x1 - ox) * a;
            }
            else
            {
                txMin = (x1 - ox) * a;
                txMax = (x0 - ox) * a;
            }

            double b = 1.0 / dy;
            if (b >= 0)
            {
                tyMin = (y0 - oy) * b;
                tyMax = (y1 - oy) * b;
            }
            else
            {
                tyMin = (y1 - oy) * b;
                tyMax = (y0 - oy) * b;
            }

            double c = 1.0 / dz;
            if (c >= 0)
            {
                tzMin = (z0 - oz) * c;
                tzMax = (z1 - oz) * c;
            }
            else
            {
                tzMin = (z1 - oz) * c;
                tzMax = (z0 - oz) * c;
            }

            double t0, t1;

            if (txMin > tyMin)
                t0 = txMin;
            else
                t0 = tyMin;

            if (tzMin > t0)
                t0 = tzMin;

            if (txMax < tyMax)
                t1 = txMax;
            else
                t1 = tyMax;

            if (tzMax < t1)
                t1 = tzMax;

            if (t0 > t1)
                return false;

            int ix, iy, iz;

            if (bbox.Inside(ray.O))
            {
                ix = (int)MathUtils.Clamp((ox - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = (int)MathUtils.Clamp((oy - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = (int)MathUtils.Clamp((oz - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            else
            {
                Vec3 p = ray.HitPoint(t0);
                ix = (int)MathUtils.Clamp((p.X - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = (int)MathUtils.Clamp((p.Y - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = (int)MathUtils.Clamp((p.Z - z0) * nz / (z1 - z0), 0, nz - 1);
            }

            double dtx = (txMax - txMin) / nx;
            double dty = (tyMax - tyMin) / ny;
            double dtz = (tzMax - tzMin) / nz;

            double txNext, tyNext, tzNext;
            int ixStep, iyStep, izStep;
            int ixStop, iyStop, izStop;

            if(dx > 0)
            {
                txNext = txMin + (ix + 1) * dtx;
                ixStep = +1;
                ixStop = nx;
            }
            else
            {
                txNext = txMin + (nx - ix) * dtx;
                ixStep = -1;
                ixStop = -1;
            }

            if(dx == 0.0)
            {
                txNext = MathUtils.HugeValue;
                ixStep = -1;
                ixStop = -1;
            }

            if (dy > 0)
            {
                tyNext = tyMin + (iy + 1) * dty;
                iyStep = +1;
                iyStop = ny;
            }
            else
            {
                tyNext = tyMin + (ny - iy) * dty;
                iyStep = -1;
                iyStop = -1;
            }

            if (dy == 0.0)
            {
                tyNext = MathUtils.HugeValue;
                iyStep = -1;
                iyStop = -1;
            }

            if (dz > 0)
            {
                tzNext = tzMin + (iz + 1) * dtz;
                izStep = +1;
                izStop = nz;
            }
            else
            {
                tzNext = tzMin + (nz - iz) * dtz;
                izStep = -1;
                izStop = -1;
            }

            if (dz == 0.0)
            {
                tzNext = MathUtils.HugeValue;
                izStep = -1;
                izStop = -1;
            }

            while (true)
            {
                GeometricObject obj = cells[ix + nx * iy + nx * ny * iz];

                if(txNext < tyNext && txNext < tzNext)
                {
                    if(obj != null && obj.Hit(ray, ref tmin, sr) && tmin < txNext)
                    {
                        material = obj.Material;
                        return true;
                    }

                    txNext += dtx;
                    ix += ixStep;

                    if (ix == ixStop)
                        return false;
                }
                else
                {
                    if(tyNext < tzNext)
                    {
                        if(obj != null && obj.Hit(ray, ref tmin, sr) && tmin < tyNext)
                        {
                            material = obj.Material;
                            return true;
                        }

                        tyNext += dty;
                        iy += iyStep;

                        if (iy == iyStop)
                            return false;
                    }
                    else
                    {
                        if (obj != null && obj.Hit(ray, ref tmin, sr) && tmin < tzNext)
                        {
                            material = obj.Material;
                            return true;
                        }

                        tzNext += dtz;
                        iz += izStep;

                        if (iz == izStop)
                            return false;
                    }
                }
            }
        }

        public override bool ShadowHit(Ray ray, ref double tmin)
        {
            if (!shadows || !bbox.Hit(ray))
                return false;

            double ox = ray.O.X;
            double oy = ray.O.Y;
            double oz = ray.O.Z;
            double dx = ray.D.X;
            double dy = ray.D.Y;
            double dz = ray.D.Z;

            double x0 = bbox.x0;
            double y0 = bbox.y0;
            double z0 = bbox.z0;
            double x1 = bbox.x0;
            double y1 = bbox.y0;
            double z1 = bbox.z0;

            double txMin = 0, tyMin = 0, tzMin = 0;
            double txMax = 0, tyMax = 0, tzMax = 0;

            double a = 1.0 / dx;
            if (a >= 0)
            {
                txMin = (x0 - ox) * a;
                txMax = (x1 - ox) * a;
            }
            else
            {
                txMin = (x1 - ox) * a;
                txMax = (x0 - ox) * a;
            }

            double b = 1.0 / dy;
            if (b >= 0)
            {
                tyMin = (y0 - oy) * b;
                tyMax = (y1 - oy) * b;
            }
            else
            {
                txMin = (y1 - oy) * b;
                txMax = (y0 - oy) * b;
            }

            double c = 1.0 / dz;
            if (c >= 0)
            {
                tzMin = (z0 - oz) * c;
                tzMax = (z1 - oz) * c;
            }
            else
            {
                tzMin = (z1 - oz) * c;
                tzMax = (z0 - oz) * c;
            }

            double t0, t1;

            if (txMin > tyMin)
                t0 = txMax;
            else
                t0 = tyMin;

            if (tzMin > t0)
                t0 = tzMin;

            if (txMax < tyMax)
                t1 = txMax;
            else
                t1 = tyMax;

            if (tzMax < t1)
                t1 = tzMax;

            if (t0 > t1)
                return false;

            int ix, iy, iz;

            if (bbox.Inside(ray.O))
            {
                ix = (int)MathUtils.Clamp((ox - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = (int)MathUtils.Clamp((oy - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = (int)MathUtils.Clamp((oz - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            else
            {
                Vec3 p = ray.HitPoint(t0);
                ix = (int)MathUtils.Clamp((p.X - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = (int)MathUtils.Clamp((p.Y - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = (int)MathUtils.Clamp((p.Z - z0) * nz / (z1 - z0), 0, nz - 1);
            }

            double dtx = (txMax - txMin) / nx;
            double dty = (tyMax - tyMin) / ny;
            double dtz = (tzMax - tzMin) / nz;

            double txNext, tyNext, tzNext;
            int ixStep, iyStep, izStep;
            int ixStop, iyStop, izStop;

            if (dx > 0)
            {
                txNext = txMin + (ix + 1) * dtx;
                ixStep = +1;
                ixStop = nx;
            }
            else
            {
                txNext = txMin + (nx - ix) * dtx;
                ixStep = -1;
                ixStop = -1;
            }

            if (dx == 0.0)
            {
                txNext = MathUtils.HugeValue;
                ixStep = -1;
                ixStop = -1;
            }

            if (dy > 0)
            {
                tyNext = tyMin + (iy + 1) * dty;
                iyStep = +1;
                iyStop = ny;
            }
            else
            {
                tyNext = tyMin + (ny - iy) * dty;
                iyStep = -1;
                iyStop = -1;
            }

            if (dy == 0.0)
            {
                tyNext = MathUtils.HugeValue;
                iyStep = -1;
                iyStop = -1;
            }

            if (dz > 0)
            {
                tzNext = tzMin + (iz + 1) * dtz;
                izStep = +1;
                izStop = nz;
            }
            else
            {
                tzNext = tzMin + (nz - iz) * dtz;
                izStep = -1;
                izStop = -1;
            }

            if (dz == 0.0)
            {
                tzNext = MathUtils.HugeValue;
                izStep = -1;
                izStop = -1;
            }

            while (true)
            {
                GeometricObject obj = cells[ix + nx * iy + nx * ny * iz];

                if (txNext < tyNext && txNext < tzNext)
                {
                    if (obj != null && obj.ShadowHit(ray, ref tmin) && tmin < txNext)
                    {
                        material = obj.Material;
                        return true;
                    }

                    txNext += dtx;
                    ix += ixStep;

                    if (ix == ixStop)
                        return false;
                }
                else
                {
                    if (tyNext < tzNext)
                    {
                        if (obj != null && obj.ShadowHit(ray, ref tmin) && tmin < tyNext)
                        {
                            material = obj.Material;
                            return true;
                        }

                        tyNext += dty;
                        iy += iyStep;

                        if (iy == iyStop)
                            return false;
                    }
                    else
                    {
                        if (obj != null && obj.ShadowHit(ray, ref tmin) && tmin < tzNext)
                        {
                            material = obj.Material;
                            return true;
                        }

                        tzNext += dtz;
                        iz += izStep;

                        if (iz == izStop)
                            return false;
                    }
                }
            }
        }

        private Vec3 MinCoordinates()
        {
            BBox objBox;
            Vec3 p = new Vec3(MathUtils.HugeValue);

            foreach (var obj in objects)
            {
                objBox = obj.BoundingBox;

                if (objBox.x0 < p.X)
                    p.X = objBox.x0;
                if (objBox.y0 < p.Y)
                    p.Y = objBox.y0;
                if (objBox.z0 < p.Z)
                    p.Z = objBox.z0;
            }

            p.X -= kEpsilon;
            p.Y -= kEpsilon;
            p.Z -= kEpsilon;

            return p;
        }

        private Vec3 MaxCoordinates()
        {
            BBox objBox;
            Vec3 p = new Vec3(-MathUtils.HugeValue);

            foreach (var obj in objects)
            {
                objBox = obj.BoundingBox;

                if (objBox.x1 > p.X)
                    p.X = objBox.x1;
                if (objBox.y1 > p.Y)
                    p.Y = objBox.y1;
                if (objBox.z1 > p.Z)
                    p.Z = objBox.z1;
            }

            p.X += kEpsilon;
            p.Y += kEpsilon;
            p.Z += kEpsilon;

            return p;
        }

        //Test
        public void TessellateFlatSphere(int horizontalSteps, int verticalSteps)
        {
            double pi = 3.1415926535897932384;
            int k = 1;

            for(int j = 0; j <= horizontalSteps - 1; j++)
            {
                Vec3 v0 = new Vec3(0, 1, 0);

                Vec3 v1 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Triangle triangle = new Triangle(v0, v1, v2);
                AddObject(triangle);
            }

            k = verticalSteps - 1;

            for (int j = 0; j <= horizontalSteps - 1; j++)
            {
                Vec3 v0 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Vec3 v1 = new Vec3(0, -1, 0);

                Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Triangle triangle = new Triangle(v0, v1, v2);
                AddObject(triangle);
            }

            for(k = 1; k <= verticalSteps - 2; k++)
            {
                for(int j = 0; j <= horizontalSteps -1; j++)
                {
                    Vec3 v0 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps),
                                       Math.Cos(pi * (k+1) / verticalSteps),
                                       Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps));

                    Vec3 v1 = new Vec3(Math.Sin(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps),
                                       Math.Cos(pi * (k + 1) / verticalSteps),
                                       Math.Cos(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps));

                    Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                       Math.Cos(pi * k / verticalSteps),
                                       Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    Triangle triangle = new Triangle(v0, v1, v2);
                    AddObject(triangle);

                    v0 = new Vec3(Math.Sin(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                  Math.Cos(pi * k / verticalSteps),
                                  Math.Cos(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    v1 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                  Math.Cos(pi * k / verticalSteps),
                                  Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    v2 = new Vec3(Math.Sin(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps),
                                       Math.Cos(pi * (k+1) / verticalSteps),
                                       Math.Cos(2.0 * pi * (j+1) / horizontalSteps) * Math.Sin(pi * (k+1) / verticalSteps));

                    triangle = new Triangle(v0, v1, v2);
                    AddObject(triangle);
                }
            }
        }

        public void TessellateSmoothSphere(int horizontalSteps, int verticalSteps)
        {
            double pi = 3.1415926535897932384;
            int k = 1;

            for (int j = 0; j <= horizontalSteps - 1; j++)
            {
                Vec3 v0 = new Vec3(0, 1, 0);

                Vec3 v1 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                SmoothTriangle triangle = new SmoothTriangle(v0, v1, v2);
                triangle.N0 = v0;
                triangle.N1 = v1;
                triangle.N2 = v2;
                AddObject(triangle);
            }

            k = verticalSteps - 1;

            for (int j = 0; j <= horizontalSteps - 1; j++)
            {
                Vec3 v0 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                Vec3 v1 = new Vec3(0, -1, 0);

                Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                   Math.Cos(pi * k / verticalSteps),
                                   Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                SmoothTriangle triangle = new SmoothTriangle(v0, v1, v2);
                triangle.N0 = v0;
                triangle.N1 = v1;
                triangle.N2 = v2;
                AddObject(triangle);
            }

            for (k = 1; k <= verticalSteps - 2; k++)
            {
                for (int j = 0; j <= horizontalSteps - 1; j++)
                {
                    Vec3 v0 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps),
                                       Math.Cos(pi * (k + 1) / verticalSteps),
                                       Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps));

                    Vec3 v1 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps),
                                       Math.Cos(pi * (k + 1) / verticalSteps),
                                       Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps));

                    Vec3 v2 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                       Math.Cos(pi * k / verticalSteps),
                                       Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    SmoothTriangle triangle = new SmoothTriangle(v0, v1, v2);
                    triangle.N0 = v0;
                    triangle.N1 = v1;
                    triangle.N2 = v2;
                    AddObject(triangle);

                    v0 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                  Math.Cos(pi * k / verticalSteps),
                                  Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    v1 = new Vec3(Math.Sin(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps),
                                  Math.Cos(pi * k / verticalSteps),
                                  Math.Cos(2.0 * pi * j / horizontalSteps) * Math.Sin(pi * k / verticalSteps));

                    v2 = new Vec3(Math.Sin(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps),
                                       Math.Cos(pi * (k + 1) / verticalSteps),
                                       Math.Cos(2.0 * pi * (j + 1) / horizontalSteps) * Math.Sin(pi * (k + 1) / verticalSteps));

                    triangle = new SmoothTriangle(v0, v1, v2);
                    triangle.N0 = v0;
                    triangle.N1 = v1;
                    triangle.N2 = v2;
                    AddObject(triangle);
                }
            }
        }
    }
}
