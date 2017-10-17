using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    class TerrainGenerator
    {
        private int InitPoints { get; set; }
        private float MaxX { get; set; }
        private float MaxHeight { get; set; }
        private float MinHeight { get; set; }
        public Vector3[] Vertices;
        public int[] Triangles;

        public TerrainGenerator(int initPoints, float maxX, float minHeight, float maxHeight, int itterations)
        {
            InitPoints = initPoints;
            MaxHeight = maxHeight;
            MinHeight = minHeight;
            MaxX = maxX;

            var pointsList = GenerateTerrain(null, itterations);
//            foreach (var point in pointsList)
//            {
//                Debug.Log(point);
//            }
            Vertices = ToVectorArray(pointsList);
            Triangles = CreateTriangles(Vertices);
        }

        public int[] GetTriangles()
        {
            return Triangles;
        }

        public Vector3[] GetVertices()
        {
            return Vertices;
        }

        private List<Point2D> GenerateTerrain([CanBeNull] List<Point2D> points, int itterations)
        {
            if (points == null)
            {
                points = new List<Point2D>();
                var increment = MaxX / (InitPoints - 1);
                var posX = 0f;
                var rand = new Random();

                for (var i = 0; i < InitPoints; i++)
                {
                    var height = (float) (MinHeight + (rand.NextDouble() * (MaxHeight - MinHeight)));
                    points.Add(new Point2D(posX,height));
                    //Debug.Log(posX + ", " + height);
                    posX += increment;
                }
            }

            if (itterations != 0)
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

                    var pointY = (float) (pointYMin + new Random().NextDouble() * (pointYMax - pointYMin));
                    points.Insert(i + 1, new Point2D(pointxSpacing * (i + 1), pointY));
                }
                GenerateTerrain(points, itterations - 1);
            }
            return points;
        }

        private Vector3[] ToVectorArray(List<Point2D> points)
        {
            var vectors = new Vector3[points.Count * 2];
            var index = 0;

            foreach (var point in points)
            {
                vectors[index] = new Vector3(point.PosX, 0, 0);
                vectors[index + 1] = new Vector3(point.PosX, point.PosY, 0);
                index += 2;
            }

            return vectors;
        }

//        private Mesh ToMesh(Vector3[] vectors)
//        {
//            Mesh mesh = new Mesh {vertices = vectors};
//            var triangles = new int[vectors.Length * 3];
//            int index = 0;
//
//            for (var i = 0; i < vectors.Length - 2; i++)
//            {
//                if (i % 2 == 0)
//                {
//                    triangles[index] = i;
//                    triangles[index + 1] = i + 1;
//                    triangles[index + 2] = i + 2;
//                }
//                else
//                {
//                    triangles[index] = i;
//                    triangles[index + 1] = i + 2;
//                    triangles[index + 2] = i + 1;
//                }
//                index += 3;
//            }
//
//            mesh.triangles = triangles;
//            return mesh;
//        }

        private int[] CreateTriangles(Vector3[] vertices)
        {
            var triangles = new int[vertices.Length * 3];
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

            return triangles;
        }
    }

    public class Point2D
    {
        public float PosX { get; set; }
        public float PosY { get; set; }

        public Point2D(float posX, float posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public override string ToString()
        {
            return "PosX: " + PosX + ", PosY: " + PosY;
        }
    }
}
