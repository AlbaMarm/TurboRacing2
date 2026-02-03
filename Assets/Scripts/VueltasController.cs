using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class VueltasController : NetworkBehaviour
{

    public CarControllerMulti myCar;
    public TMP_Text textoContador;
    public bool haChocado;

    [Networked]
    public int contador { get; set; }

    private void Awake()
    {
        myCar = GetComponentInChildren<CarControllerMulti>();
        haChocado = false;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            //Debug.Log("Player: " + Runner.LocalPlayer.AsIndex);
            contador = 1;
        }
    }

    public override void Render()
    {
        textoContador.text = contador.ToString() + "/3";
    }

    public void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && haChocado == false && other.gameObject.CompareTag("Meta"))
        {
            Debug.Log("choca");
            contador++;
            haChocado = true;
            StartCoroutine("recuperaChoque");
        }
    }

    IEnumerator recuperaChoque()
    {
        yield return new WaitForSeconds(3);
        if (haChocado)
        {
            haChocado = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log($"Soy el jugador {Runner.LocalPlayer.AsIndex}");

        ReadOnlyDictionary<string, SessionProperty> ganador = Runner.SessionInfo.Properties;
        if (ganador.TryGetValue("Ganador", out SessionProperty data))
        {
            int numGanador = (int)data.PropertyValue;

            if (numGanador != 0)
            {
                EndGame(numGanador);
                
                return;
            }

            //Solo el HOST puede comprobar quien ha ganado
            if (HasStateAuthority)
            {
                if (contador >= 3)
                {
                    Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
                    propiedades.Add("Ganador", (SessionProperty)myCar.playerID);
                    Runner.SessionInfo.UpdateCustomProperties(propiedades);
                    //EndGame(numGanador);

                    return;
                }
            }
        } 
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void Rpc_EndGame(int numGanador)
    {
        //Debug.Log($"LeaveGame llamado por jugador {Runner.LocalPlayer.AsIndex} para cargar escena {sceneIndex}");
        Debug.Log("Player Llamando: " + Runner.LocalPlayer.AsIndex);
        EndGame(numGanador);
    }

    public void EndGame(int numGanador) {
        int sceneIndex;
        if (numGanador == Runner.LocalPlayer.AsIndex)
        {
            Debug.Log("gana");
            sceneIndex = 4;
        }
        else
        {
            Debug.Log("pierde");
            sceneIndex = 5;
        }

        Debug.Log($"Ganador: {numGanador}. Cambio a escena {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
                
        // Desconectar al jugador que llamó (si es necesario)
        //Runner.Disconnect(Runner.LocalPlayer);

        //Runner.Shutdown();

    }
}

