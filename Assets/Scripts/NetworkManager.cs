using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    private float _spawnOffset = 1f;

    public GameObject Terrain;
    private GameObject _spawnedTerrain;

    public override void OnStartServer()
    {
        _spawnedTerrain = Instantiate(Terrain);
        base.OnStartServer();
    }

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
        NetworkServer.Spawn(_spawnedTerrain);
    }
   
}
