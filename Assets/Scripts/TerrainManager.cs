using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainManager : MonoBehaviour
    {
        public int InitPoints = 8;
        public int Iterations = 5;

        // Use this for initialization
        void Start ()
        {
            var cam = Camera.main;
            var p = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            var terrainGenerator = new TerrainGenerator(InitPoints, Mathf.Abs(p.x * 2), Mathf.Abs(p.y), Mathf.Abs(p.y) / 2, Iterations);

            mesh.Clear();
            mesh.vertices = terrainGenerator.Vertices;
            mesh.triangles = terrainGenerator.Triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>();

            transform.position = new Vector3(p.x, -p.y, transform.position.z);
        } 
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
