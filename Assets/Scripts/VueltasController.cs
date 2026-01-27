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
    public TMP_Text textoContador;
    public bool haChocado;

    [Networked]
    public int contador { get; set; }

    private void Awake()
    {
        haChocado = false;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
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
        if (HasStateAuthority && contador == 3)
        {
            ReadOnlyDictionary<string, SessionProperty> ganador = Runner.SessionInfo.Properties;

            if (ganador.TryGetValue("Ganador", out SessionProperty data))
            {
                int numGanador = (int)data.PropertyValue;
                if (numGanador == 0 && Runner.LocalPlayer.IsRealPlayer)
                {
                    numGanador = Runner.LocalPlayer.AsIndex;
                    Debug.Log(numGanador);
                }

                Rpc_EndGame(Runner.LocalPlayer, numGanador);
            }
        }
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_EndGame(PlayerRef caller, int numGanador)
    {

        // Aquí puedes manejar la lógica en el servidor, por ejemplo:
        // - Cambiar escena sincronizadamente
        // - Desconectar jugador
        // - Otras acciones necesarias

        // Ejemplo de carga de escena sincronizada
        //Runner.LoadScene(Runner.SceneManager.GetSceneRef(SceneManager.GetSceneByBuildIndex(sceneIndex).name));

        int sceneIndex;
        if (Runner.IsSceneAuthority && numGanador == Runner.LocalPlayer.AsIndex)
        {
            Debug.Log("gana");
            sceneIndex = 4;
            //Runner.UnloadScene(Runner.SceneManager.GetSceneRef(SceneManager.GetSceneByBuildIndex(3).name));
            //Runner.LoadScene(Runner.SceneManager.GetSceneRef(SceneManager.GetSceneByBuildIndex(4).name));
        }
        else
        {
            Debug.Log("pierde");
            sceneIndex = 5;
        }

        Debug.Log($"LeaveGame llamado por jugador {caller} para cargar escena {sceneIndex}");

        SceneManager.LoadScene(sceneIndex);

        // Desconectar al jugador que llamó (si es necesario)
        Runner.Disconnect(caller);
    }

}
