using RayTracing.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Util
{
    class Mesh
    {
        public List<Vec3> Vertices;
        public List<int> Indices;
        public List<Vec3> Normals;
        public List<List<int>> VertexFaces;
        public List<float> U;
        public List<float> V;
        public int NumVertices;
        public int NumTriangles;

        public Mesh()
        {
            Vertices = new List<Vec3>();
            Indices = new List<int>();
            Normals = new List<Vec3>();
            VertexFaces = new List<List<int>>();
            U = new List<float>();
            V = new List<float>();
            NumVertices = 0;
            NumTriangles = 0;
        }

        public Mesh(Mesh other)
        {
            Vertices = other.Vertices.ToList();
            Indices = other.Indices.ToList();
            Normals = other.Normals.ToList();
            VertexFaces = other.VertexFaces.ToList();
            U = other.U.ToList();
            V = other.V.ToList();
            NumVertices = other.NumVertices;
            NumTriangles = other.NumTriangles;
        }
    }
}
