﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainManager : MonoBehaviour
    {
        public int InitPoints = 8;
        public int Iterations = 5;
        public float Width = 25f;
        public float MinHeight = 2f;
        public float MaxHeight = 5f;

        // Use this for initialization
        void Start ()
        {

            var mf = GetComponent<MeshFilter>();
            var mesh = mf.mesh;

            var terrainGenerator = new TerrainGenerator(InitPoints, Width, MinHeight, MaxHeight, Iterations);

            mesh.Clear();
            mesh.vertices = terrainGenerator.Vertices;
            mesh.triangles = terrainGenerator.Triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>();

            //transform.position = new Vector3(p.x, -p.y, transform.position.z);
        } 
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
