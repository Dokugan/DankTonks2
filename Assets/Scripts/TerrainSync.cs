using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace Assets.Scripts
{
    class TerrainSync : NetworkBehaviour
    {

        [SyncVar] public int Seed;

        void Start()
        {
            GenSeed();
            UnityEngine.Random.InitState(Seed);
            gameObject.AddComponent<TerrainManager>();
        }

        void GenSeed()
        {
            if (isServer)
                Seed = UnityEngine.Random.Range(0, 100000);
            Debug.Log(Seed);
        }
    }
}
