using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VueltasController : NetworkBehaviour
{
    public TMP_Text textoContador;

    [Networked] public int contador { get; set; }
    [Networked] private bool haChocado { get; set; }

    // 🔑 Ganador global (solo lo escribe el host)
    [Networked] private PlayerRef ganador { get; set; }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            contador = 1;
            ganador = PlayerRef.None;
        }
    }

    public override void Render()
    {
        textoContador.text = contador + "/3";
    }

    private void OnTriggerEnter(Collider other)
    {
        // ⚠️ Cada jugador cuenta SUS vueltas (InputAuthority)
        if (!HasInputAuthority) return;

        if (!haChocado && other.CompareTag("Meta"))
        {
            contador++;
            haChocado = true;
            StartCoroutine(RecuperaChoque());

            Debug.Log($"Jugador {Object.InputAuthority.AsIndex} pasó meta. Vueltas: {contador}");

            if (contador >= 3)
            {
                Rpc_PlayerFinished(Object.InputAuthority);
            }
        }
    }

    IEnumerator RecuperaChoque()
    {
        yield return new WaitForSeconds(3f);
        haChocado = false;
    }

    // 🏁 El jugador avisa que terminó → SOLO HOST recibe esto
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void Rpc_PlayerFinished(PlayerRef player)
    {
        // El primero que llega gana
        if (ganador != PlayerRef.None) return;

        ganador = player;
        Debug.Log($"🏆 Ganador decidido por HOST: Player {ganador.AsIndex}");

        Rpc_EndGame(ganador);
    }

    // 📢 El host avisa a TODOS quién ganó
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_EndGame(PlayerRef ganadorFinal)
    {
        bool soyGanador = Runner.LocalPlayer == ganadorFinal;

        Debug.Log(soyGanador
            ? "🎉 GANASTE LA CARRERA"
            : "💀 PERDISTE LA CARRERA");

        int sceneIndex = soyGanador ? 4 : 5;
        SceneManager.LoadScene(sceneIndex);
    }
}
