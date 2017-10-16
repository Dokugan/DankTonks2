using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainManager : MonoBehaviour
    {

        public int InitPoints = 8;
        public float MostRightX = 50f;
        public float MaxY = 5f;
        public float MinY = 2f;
        public int Iterations = 5;

        // Use this for initialization
        void Start ()
        {

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            var terrainGenerator = new TerrainGenerator(InitPoints, MostRightX, MaxY, MinY, Iterations);

            mesh.Clear();
            mesh.vertices = terrainGenerator.Vertices;
            mesh.triangles = terrainGenerator.Triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>();
//            foreach (var tri in terrainGenerator.Vertices)
//            {
//                Debug.Log(tri);
//            }
//
//            Debug.Log(terrainGenerator.Triangles);
//            Debug.Log(terrainGenerator.Vertices.ToString());
        } 
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
