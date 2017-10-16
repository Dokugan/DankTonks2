using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainManager : MonoBehaviour {

        // Use this for initialization
        void Start ()
        {

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            var terrainGenerator = new TerrainGenerator(5, 10f, 5f, 2f, 5);

            mesh.Clear();
            mesh.vertices = terrainGenerator.Vertices;
            mesh.triangles = terrainGenerator.Triangles;
            mesh.RecalculateNormals();

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
