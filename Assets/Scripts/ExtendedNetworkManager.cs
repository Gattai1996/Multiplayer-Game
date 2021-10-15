using Mirror;
using UnityEngine;

public class ExtendedNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        var player = conn.identity.GetComponent<NetworkPlayer>();
        
        player.SetName("Player " + numPlayers);

        var randomColor = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            1f);
        
        player.SetColor(randomColor);
        
        Debug.Log(player.name + " joined to the server.\n" +
                  "Total of " + numPlayers + " Players online."); 
    }
}
