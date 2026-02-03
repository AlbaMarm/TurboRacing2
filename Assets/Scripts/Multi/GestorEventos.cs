using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GestorEventos : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Inputs")]
    [SerializeField] InputActionReference rigthTrigger;
    [SerializeField] XRKnob steeringWheel;

    //Input variables
    bool accelerationTriggerPressed = false;
    
    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;

    public GameObject jugador1;
    public GameObject jugador2;

    private InputData inputData;
    
    private ListaJugadoresController LJC;
    private NetworkRunner red;

    int numPlayerIn = 0;


    private void Awake()
    {
        red = FindAnyObjectByType<NetworkRunner>();
        red.AddCallbacks(this);

        LJC = FindAnyObjectByType<ListaJugadoresController>();

        inputData = new InputData();
        numPlayerIn = 0;
    }

    private void OnEnable()
    {
        rigthTrigger.action.performed += CarAccelate;
        rigthTrigger.action.canceled += CarAccelateStop;
    }


    private void OnDisable()
    {
        rigthTrigger.action.performed -= CarAccelate;
        rigthTrigger.action.canceled -= CarAccelateStop;
    }

    private void CarAccelate(InputAction.CallbackContext context)
    {
        //Debug.Log("Gatillo presionado");
        accelerationTriggerPressed = true;
    }

    private void CarAccelateStop(InputAction.CallbackContext context)
    {
        //Debug.Log("Gatillo soltado");
        accelerationTriggerPressed = false;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Conectado al server");

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
        Debug.Log("Desconectado del server");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Boolean camina = false;

        ReadOnlyDictionary<string, SessionProperty> c = runner.SessionInfo.Properties;

        if (c.TryGetValue("Contador", out SessionProperty p))
        {
            int contador = (int)p.PropertyValue;

            if (contador <= 0)
            {
                camina = true;
            }
        }

        inputData.movimiento = camina ? 1 : 0;

        inputData.triggerPressed = false;
        inputData.knobValue = 0.0f;

        if(steeringWheel != null)
        {
            inputData.knobValue = steeringWheel.value;
        }

        inputData.triggerPressed = accelerationTriggerPressed;

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
        numPlayerIn++;
        //Debug.Log($"Numero jugadores: {numPlayerIn}");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //Debug.Log("Desconecta jugador1");

        numPlayerIn--;
        // Debug.Log($"Numero jugadores: {numPlayerIn}");
        if (numPlayerIn <= 0) runner.Shutdown();


        if (runner.IsServer && LJC.listaSJ[runner.SessionInfo].TryGetValue(player, out NetworkObject p))
        {
            if (p != null)
            {
                runner.Despawn(p);
                //Debug.Log("Desconecta jugador");
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
            int numJugador = 1;

            foreach(PlayerRef pR in LJC.listaSJ[runner.SessionInfo].Keys.ToList())
            {
                if (LJC.listaSJ[runner.SessionInfo][pR]==null)
                {
                    if (numJugador == 1)
                    {
                        //jugador1.GetComponentInChildren<CarControllerMulti>().playerID = pR.AsIndex;
                        NetworkObject p = runner.Spawn(jugador1, SpawnPoint1.transform.position, Quaternion.identity, pR);
                        
                        LJC.listaSJ[runner.SessionInfo][pR] = p;
                    }
                    else
                    {
                        //jugador2.GetComponentInChildren<CarControllerMulti>().playerID = pR.AsIndex;
                        NetworkObject p = runner.Spawn(jugador2, SpawnPoint2.transform.position, Quaternion.identity, pR);
                        LJC.listaSJ[runner.SessionInfo][pR] = p;
                    }
                }
                numJugador++;
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
        Debug.Log("Shutdown del server");

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

}
