using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using NetworkManager = UnityEngine.Networking.NetworkManager;

namespace Assets.Scripts
{
    public class TerrainManager : NetworkBehaviour
    {
        public Vector3[] Vertices;
        private int[] _triangles;
        public Vector2[] PointsList;

        // Use this for initialization
        void Start()
        {
            gameObject.tag = "Terrain";
        }

        [ClientRpc (channel = 3)]
        public void RpcSetTerrainPoints(Vector2[] points)
        {
            PointsList = points;
        }

        public void SetMesh()
        {
            _triangles = CreateTriangles(Vertices);

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            mesh.Clear();
            mesh.vertices = Vertices;
            mesh.triangles = _triangles;
            mesh.RecalculateNormals();
            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }

        public static Vector2[] GenerateTerrain([CanBeNull] List<Vector2> points, int iterations, int initPoints, float maxX, float minHeight, float maxHeight)
        {
            if (points == null)
            {
                points = new List<Vector2>();
                var increment = NetworkManager.MaxX / (initPoints - 1);
                var posX = 0f;

                for (var i = 0; i < initPoints; i++)
                {
                    var height = minHeight + (UnityEngine.Random.value * (maxHeight - minHeight));
                    points.Add(new Vector2(posX, height));
                    posX += increment;
                }
            }

            if (iterations != 0)
            {
                int pointsSize = points.Count;
                float pointxSpacing = points[1].x / 2;
                for (var i = 0; i < pointsSize * 2 - 2; i += 2)
                {
                    float pointYMax;
                    float pointYMin;

                    if (points[i].y > points[i + 1].y)
                    {
                        pointYMax = points[i].y;
                        pointYMin = points[i + 1].y;
                    }
                    else
                    {
                        pointYMin = points[i].y;
                        pointYMax = points[i + 1].y;
                    }

                    var pointY = pointYMin + UnityEngine.Random.value * (pointYMax - pointYMin);
                    points.Insert(i + 1, new Vector2(pointxSpacing * (i + 1), pointY));
                }
                GenerateTerrain(points, iterations - 1, initPoints, maxX, minHeight, maxHeight);
            }

            var pointsList = new Vector2[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                pointsList[i] = new Vector2(points[i].x, points[i].y);
            }

            return pointsList;
        }

        private Vector3[] ToVector3Array(Vector2[] points)
        {
            var vectors = new Vector3[points.Length * 4];
            var index = 0;
            var reverse = vectors.Length - 2;

            foreach (var point in points)
            {
                vectors[index] = new Vector3(point.x, 0, 0);
                vectors[index + 1] = new Vector3(point.x, point.y, 0);
                vectors[reverse] = new Vector3(point.x, 0, 1);
                vectors[reverse + 1] = new Vector3(point.x, point.y, 1);
                index += 2;
                reverse -= 2;
            }

            return vectors;
        }

        private int[] CreateTriangles(Vector3[] vertices)
        {
            //+ (_pointsList.Count -1) * 2 * 3
            var triangles = new int[vertices.Length * 3 + (Vertices.Length / 4 - 1) * 2 * 3];
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

            for (var j = 1; j < Vertices.Length * 2; j += 2)
            {
                triangles[index] = j;
                triangles[index + 1] = vertices.Length - j;
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

            for (int i = 0; i < PointsList.Length; i++)
            {
                if (Math.Pow((PointsList[i].x - x), 2) + Math.Pow((PointsList[i].y - y), 2) < Math.Pow(impactRadius, 2) ||
                    (PointsList[i].y >= -Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(PointsList[i].x - x, 2)) + y &&
                     (PointsList[i].x > x - impactRadius && PointsList[i].x < x + impactRadius)))
                {
                    PointsList[i].y = (float)-Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(PointsList[i].x - x, 2)) + y;
                }
            }

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            Vertices = ToVector3Array(PointsList);
            _triangles = CreateTriangles(Vertices);
            mesh.Clear();
            mesh.vertices = Vertices;
            mesh.triangles = _triangles;
            mesh.RecalculateNormals();
            var mc = gameObject.GetComponent<MeshCollider>();
            mc.sharedMesh = null;
            mc.sharedMesh = mesh;

        }
    }
}