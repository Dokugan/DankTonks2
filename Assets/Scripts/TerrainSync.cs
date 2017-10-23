using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    class TerrainSync : NetworkBehaviour
    {
        [SyncVar] private int seed;

        void Start()
        {
            if (isServer)
                seed = Random.Range(0, 50000);
            Random.InitState(seed);
        }

        void Update()
        {
            // DON'T REMOVE!
        }
    }
}
