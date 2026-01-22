using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GestorEventos : MonoBehaviour, INetworkRunnerCallbacks
{
    public XRKnob steeringWheelKnob;
    private InputDevice rightController;
    public GameObject SpawnPoint;

    public GameObject jugador;
    private InputData inputData;
    
    private ListaJugadoresController LJC;
    private NetworkRunner red;

    private void Awake()
    {
        red = FindAnyObjectByType<NetworkRunner>();
        red.AddCallbacks(this);

        LJC = FindAnyObjectByType<ListaJugadoresController>();

        inputData = new InputData();

        // Obtener controlador derecho
        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
        {
            rightController = rightHandedControllers[0];
        }
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
        inputData.triggerPressed = false;
        inputData.knobValue = 0.0f;

        if(steeringWheelKnob!=null)
        {
            inputData.knobValue = steeringWheelKnob.value;
        }

        if (rightController.isValid)
        {
            bool triggerPressed = false;
            if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
            {
                inputData.triggerPressed = triggerPressed;
            }
        }
        input.Set(inputData);
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
        if(runner.IsServer && SceneManager.GetActiveScene().buildIndex == 3)
        {
            foreach(PlayerRef pR in LJC.listaSJ[runner.SessionInfo].Keys.ToList())
            {
                if (LJC.listaSJ[runner.SessionInfo][pR]==null)
                {
                    NetworkObject p = runner.Spawn(jugador, SpawnPoint.transform.position, Quaternion.identity, pR);
                    LJC.listaSJ[runner.SessionInfo][pR] = p;
                }
            }
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
