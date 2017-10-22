using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainManager : MonoBehaviour
    {
        public int InitPoints = 10;
        public int Iterations = 6;
        public static float MaxX = 25f;
        public float MinHeight = 2f;
        public float MaxHeight = 8f;
        private Vector3[] _vertices;
        private int[] _triangles;
        private List<Point3D> _pointsList;

        // Use this for initialization
        void Start ()
        {
            _pointsList = GenerateTerrain(null, Iterations);

            gameObject.tag = "Terrain";

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            GenerateTerrain(null, Iterations);
            _vertices = ToVectorArray(_pointsList);
            _triangles = CreateTriangles(_vertices);

            mesh.Clear();
            mesh.vertices = _vertices;
            mesh.triangles = _triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;

        }

        private List<Point3D> GenerateTerrain([CanBeNull] List<Point3D> points, int iterations)
        {
            if (points == null)
            {
                points = new List<Point3D>();
                var increment = MaxX / (InitPoints - 1);
                var posX = 0f;

                for (var i = 0; i < InitPoints; i++)
                {
                    var height = MinHeight + (UnityEngine.Random.value * (MaxHeight - MinHeight));
                    points.Add(new Point3D(posX, height,0));
                    posX += increment;
                }
            }

            if (iterations != 0)
            {
                int pointsSize = points.Count;
                float pointxSpacing = points[1].PosX / 2;
                for (var i = 0; i < pointsSize * 2 - 2; i += 2)
                {
                    float pointYMax;
                    float pointYMin;

                    if (points[i].PosY > points[i + 1].PosY)
                    {
                        pointYMax = points[i].PosY;
                        pointYMin = points[i + 1].PosY;
                    }
                    else
                    {
                        pointYMin = points[i].PosY;
                        pointYMax = points[i + 1].PosY;
                    }

                    var pointY = pointYMin + UnityEngine.Random.value * (pointYMax - pointYMin);
                    points.Insert(i + 1, new Point3D(pointxSpacing * (i + 1), pointY,0));
                }
                GenerateTerrain(points, iterations - 1);
            }
            return points;
        }

        private Vector3[] ToVectorArray(List<Point3D> points)
        {
            var vectors = new Vector3[points.Count * 4];
            var index = 0;
            var reverse = vectors.Length -2;

            foreach (var point in points)
            {
                vectors[index] = new Vector3(point.PosX, 0, 0);
                vectors[index + 1] = new Vector3(point.PosX, point.PosY, 0);
                vectors[reverse] = new Vector3(point.PosX, 0, 1);
                vectors[reverse + 1] = new Vector3(point.PosX, point.PosY, 1);
                index += 2;
                reverse -= 2;
            }

            return vectors;
        }

        private int[] CreateTriangles(Vector3[] vertices)
        {
            //+ (_pointsList.Count -1) * 2 * 3
            var triangles = new int[vertices.Length * 3 + (_pointsList.Count - 1) * 2 * 3];
            int index = 0;

            for (var i = 0; i < vertices.Length - 2; i++)
            {
                if (i % 2 == 0)
                {
                    triangles[index] = i;
                    triangles[index + 1] = i + 1;
                    triangles[index + 2] = i + 2;
                }
                else
                {
                    triangles[index] = i;
                    triangles[index + 1] = i + 2;
                    triangles[index + 2] = i + 1;
                }
                index += 3;
            }

            for (var j = 1; j < _pointsList.Count * 2 ; j += 2)
            {
                triangles[index] = j;
                triangles[index + 1] = vertices.Length - j ;
                triangles[index + 2] = vertices.Length - j - 2;
                triangles[index + 3] = vertices.Length - j - 2;
                triangles[index + 4] = j + 2;
                triangles[index + 5] = j;

                index += 6;
            }

            return triangles;
        }

//        private void CreateCollider()
//        {
//            for (var i = 0; i < _pointsList.Count; i += 2)
//            {
//                _pointsList[i].PosX = _pointsList[i + 1].PosX;
//                _pointsList[i].PosY = _pointsList[i + 1].PosY;
//                _pointsList[i].PosZ = -1;
//                _pointsList[i + 1].PosZ = 1;
//            }
//        }

        public void CalculateImpact(float x, float y, float impactRadius)
        {

            foreach (var point in _pointsList)
            {
                if (Math.Pow((point.PosX - x),2) + Math.Pow((point.PosY - y),2) < Math.Pow(impactRadius,2) ||
                    (point.PosY >= -Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(point.PosX - x, 2)) + y &&
                    (point.PosX > x - impactRadius && point.PosX < x + impactRadius)))
                {
                    point.PosY = (float) -Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(point.PosX - x, 2)) + y;
                }
            }

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            _vertices = ToVectorArray(_pointsList);
            _triangles = CreateTriangles(_vertices);
            mesh.Clear();
            mesh.vertices = _vertices;
            mesh.triangles = _triangles;
            mesh.RecalculateNormals();
            var mc = gameObject.GetComponent<MeshCollider>();
            mc.sharedMesh = null;
            mc.sharedMesh = mesh;

        }
    }

    public class Point3D
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public Point3D(float posX, float posY, float posZ)
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }

        public override string ToString()
        {
            return "PosX: " + PosX + ", PosY: " + PosY;
        }
    }
}
