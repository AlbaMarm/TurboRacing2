using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ListaJugadoresController : MonoBehaviour
{
    public Dictionary<SessionInfo, Dictionary<PlayerRef, NetworkObject>> listaSJ;

    private void Awake()
    {
        if (listaSJ == null)
        {
            listaSJ = new Dictionary<SessionInfo, Dictionary<PlayerRef, NetworkObject>>();
        }

        DontDestroyOnLoad(this);
    }
}