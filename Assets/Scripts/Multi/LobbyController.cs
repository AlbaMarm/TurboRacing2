using Fusion.Sockets;
using Fusion;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LobbyController : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner red;
    private ListaJugadoresController LJC;

    public Text mensaje;

    private void Awake()
    {
        red = FindAnyObjectByType<NetworkRunner>();
        red.AddCallbacks(this);

        LJC = FindAnyObjectByType<ListaJugadoresController>();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            if (!LJC.listaSJ.ContainsKey(runner.SessionInfo))
            {
                LJC.listaSJ.Add(runner.SessionInfo, new Dictionary<PlayerRef, NetworkObject>());
            }

            LJC.listaSJ[runner.SessionInfo].Add(player, null);

            if (runner.IsSceneAuthority)
            {
                if (runner.SessionInfo.PlayerCount == runner.SessionInfo.MaxPlayers)
                {
                    runner.UnloadScene(SceneRef.FromIndex(1));
                    runner.LoadScene(SceneRef.FromIndex(3));
                }
            }
        }

        mensaje.text = "Esperando..." + runner.ActivePlayers.Count() + "/" + runner.SessionInfo.MaxPlayers;

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            mensaje.text = "Esperando..." + runner.ActivePlayers.Count() + "/" + runner.SessionInfo.MaxPlayers;
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

}