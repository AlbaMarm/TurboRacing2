using System.Collections.Generic;
using System.Linq;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InicioController : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner red;
    private ListaJugadoresController LJC;
    private NetworkSceneManagerDefault sceneManager;

    private void Awake()
    {
        red = FindAnyObjectByType<NetworkRunner>();
        if (red){
            red.name = "TurboRacing";
            red.ProvideInput = true;
            red.AddCallbacks(this);
        }
        if(red.IsRunning) red.Shutdown();

        sceneManager = FindAnyObjectByType<NetworkSceneManagerDefault>();

        LJC = FindAnyObjectByType<ListaJugadoresController>();
    }

    public async void Conectar()
    {
        Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
        propiedades.Add("Ganador", (SessionProperty)0);

        StartGameArgs argumentos = new StartGameArgs();
        argumentos.GameMode = GameMode.AutoHostOrClient;
        argumentos.SessionName = "Carrera";
        argumentos.PlayerCount = 2;
        argumentos.SceneManager = sceneManager;
        argumentos.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        argumentos.SessionProperties = propiedades;

        await red.StartGame(argumentos);

    }

    /*public async void Desconectar()
    {
        if (red != null && red.IsRunning)
        {
            Debug.Log("Desconectando...");
            await red.Shutdown();
        }

        int sceneindex;
        sceneindex = 0;
        SceneManager.LoadScene(sceneindex);
    }*/


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
        Debug.Log("Se conecta");

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
                    runner.UnloadScene(SceneRef.FromIndex(0));
                    runner.LoadScene(SceneRef.FromIndex(3));
                }
                else
                {
                    runner.UnloadScene(SceneRef.FromIndex(0));
                    runner.LoadScene(SceneRef.FromIndex(1));
                }
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer && LJC.listaSJ[runner.SessionInfo].TryGetValue(player, out NetworkObject p))
        {
            if (p != null)
            {
                runner.Despawn(p);
            }

            LJC.listaSJ[runner.SessionInfo].Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

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