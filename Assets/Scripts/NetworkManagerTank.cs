using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NetworkManagerTank : NetworkManager
{

    public Transform firstPlayerSpawn;
    public Transform secondPlayerSpawn;


    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        Transform spawnPoint = numPlayers == 0 ? firstPlayerSpawn : secondPlayerSpawn;
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(connection, player);
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
    }

}
