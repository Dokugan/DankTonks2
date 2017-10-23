using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        private float _spawnOffset = 1f;

        public GameObject Terrain;

        public int InitPoints = 10;
        public static float MaxX = 25f;
        public float MinHeight = 2f;
        public float MaxHeight = 8f;

        public int Iterations = 10;
        public static int Channel;

        public Vector2[] PointsList;

        void Start()
        {
            Channel = channels.Count;
            channels.Add(QosType.UnreliableFragmented);
            Debug.Log(Channel + " " + channels.Count);
        
        }

        public override void OnStartServer()
        {
            PointsList = TerrainManager.GenerateTerrain(null, Iterations, InitPoints, MaxX, MinHeight, MaxHeight);
            base.OnStartServer();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            var terrainManager = (TerrainManager) Terrain.gameObject.GetComponent(typeof(TerrainManager));
            terrainManager.RpcSetTerrainPoints(PointsList);
            base.OnClientConnect(conn);
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
                spawnX = MaxX - (numPlayers * _spawnOffset);
            }

            var player = Instantiate(playerPrefab);
            player.transform.position = new Vector3(spawnX, 10, 0);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
