using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        private float _spawnOffset = 1f;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            float spawnX;
            if (numPlayers % 2 == 0)
            {
                spawnX = numPlayers + 1 * _spawnOffset;
            }
            else
            {
                spawnX = TerrainManager.MaxX - (numPlayers * _spawnOffset);
            }

            var player = Instantiate(playerPrefab);
            player.transform.position = new Vector3(spawnX, 10, 0);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
