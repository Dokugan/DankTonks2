using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class TerrainManager : NetworkBehaviour
    {
        public Vector3[] Vertices;
        private int[] _triangles;

        [SerializeField] private SyncListVector2 _pointsList = new SyncListVector2();

        private const int InitPoints = 8;
        public static float MaxX = 25f;
        private const float MinHeight = 6f;
        private const float MaxHeight = 10f;
        private const int Segments = 256;

        // Use this for initialization
        void Start()
        {
            gameObject.tag = "Terrain";

            if (isServer)
            {
                List<Vector2> points = GenerateTerrain(InitPoints, Segments, MaxX, MinHeight, MaxHeight);
                foreach (var point in points)
                {
                    _pointsList.Add(point);
                }
            }

            SetMesh();
        }

        public override void OnStartClient()
        {
            _pointsList.Callback = OnTerrainChanged;
        }

        public void SetMesh()
        {
            Vertices = ToVector3Array(_pointsList);
            _triangles = CreateTriangles(Vertices);

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            mesh.Clear();
            mesh.vertices = Vertices;
            mesh.triangles = _triangles;
            mesh.RecalculateNormals();

            var mc = gameObject.GetComponent<MeshCollider>();

            if (mc == null)
            {
                gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
            }else{
                mc.sharedMesh = null;
                mc.sharedMesh = mesh;
            }
        }

        public static List<Vector2> GenerateTerrain(int initPoints, int segments, float maxX, float minHeight, float maxHeight)
        {
            List<Vector2> originalPoints = new List<Vector2>();
            var increment = MaxX / (initPoints - 1);
            var posX = 0f;

            for (var i = 0; i < initPoints; i++)
            {
                var height = minHeight + (UnityEngine.Random.value * (maxHeight - minHeight));
                originalPoints.Add(new Vector2(posX, height));
                posX += increment;
            }

            var points = new List<Vector2>();
            points.Add(originalPoints[0]);

            for (int i = 0; i < originalPoints.Count - 1; i++)
            {
                var p0 = originalPoints[i];
                var p1 = new Vector2(originalPoints[i].x + (originalPoints[i + 1].x - originalPoints[i].x) / 2, originalPoints[i].y);
                var p2 = new Vector2(originalPoints[i].x + (originalPoints[i + 1].x - originalPoints[i].x) / 2, originalPoints[i+1].y);
                var p3 = originalPoints[i + 1];

                print("p0x: " + p0.x);
                print("p1x: " + p1.x);
                print("p2x: " + p2.x);
                print("p3x: " + p3.x);
                
                for (var j = 0; j < segments / initPoints; j++)
                {
                    var t = j / (float)(segments / initPoints);
                    points.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
                }
            }

            points.Add(originalPoints[originalPoints.Count - 1]);

            return points;
        }

        public static Vector2 CalculateBezierPoint(float t,
            Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * p0; //first term
            p += 3 * uu * t * p1; //second term
            p += 3 * u * tt * p2; //third term
            p += ttt * p3; //fourth term

            return p;
        }

        private Vector3[] ToVector3Array(SyncListStruct<Vector2> points)
        {
            var vectors = new Vector3[points.Count * 4];
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

            for (var j = 1; j < _pointsList.Count * 2; j += 2)
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
            if (isServer)
            {
                for (int i = 0; i < _pointsList.Count; i++)
                {
                    if (Math.Pow((_pointsList[i].x - x), 2) + Math.Pow((_pointsList[i].y - y), 2) < Math.Pow(impactRadius, 2) ||
                        (_pointsList[i].y >= -Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(_pointsList[i].x - x, 2)) + y &&
                         (_pointsList[i].x > x - impactRadius && _pointsList[i].x < x + impactRadius)))
                    {
                        _pointsList[i] = new Vector2(_pointsList[i].x, (float)-Math.Sqrt(Math.Pow(impactRadius, 2) - Math.Pow(_pointsList[i].x - x, 2)) + y);
                    }
                }
                SetMesh();
            }
        }

        private void OnTerrainChanged(SyncListVector2.Operation operation, int index)
        {
            SetMesh();
        }
    }

    public class SyncListVector2 : SyncListStruct<Vector2>
    {
        
    }
}