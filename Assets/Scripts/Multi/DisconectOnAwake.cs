using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class DisconectOnAwake : MonoBehaviour, INetworkRunnerCallbacks
{
    float waitTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        StartCoroutine(Disconect(runner));
    }
    IEnumerator Disconect(NetworkRunner runner)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        runner.Disconnect(runner.LocalPlayer);
    }


    #region Multi
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
    #endregion

}
