using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        private float _spawnOffset = 1f;

        void Start()
        {
            //networkAddress = "84.31.140.72";
            //networkPort = 7777;
        }
        
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {

            var player = Instantiate(playerPrefab);

            float spawnX;
            if (numPlayers % 2 == 0)
            {
                spawnX = numPlayers + 1 * _spawnOffset;
                player.transform.position = new Vector3(spawnX, 10, 0);
            }
            else
            {
                spawnX = TerrainManager.MaxX - (numPlayers * _spawnOffset);
                player.transform.position = new Vector3(spawnX, 10, 0);
                player.transform.GetChild(0).rotation = Quaternion.Euler(0,0, player.transform.GetChild(0).rotation.eulerAngles.z * -1);

            }

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        }

        public override void OnStartClient(NetworkClient client)
        {
            Network.Connect("84.31.140.72", 7777);
        }

        public override void OnStartServer()
        {
            Network.InitializeServer(4, 7777, true);
        }
    }
}
