using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
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
        textoContador.text = contador.ToString()+"/3";
    }

    public void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && haChocado==false && other.gameObject.CompareTag("Meta")) {
            Debug.Log("choca");
            contador++;
            haChocado = true;
            StartCoroutine("recuperaChoque");
        }
    }

       IEnumerator recuperaChoque()
       {
       yield return new WaitForSeconds(3);
           if(haChocado)
           { 
               haChocado = false;
           }
       }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority && contador == 3) { 
                ReadOnlyDictionary<string, SessionProperty> ganador = Runner.SessionInfo.Properties;

                if (ganador.TryGetValue("Ganador", out SessionProperty data))
                {
                    int numGanador = (int)data.PropertyValue;
                    if (numGanador == 0 && Runner.LocalPlayer.IsRealPlayer)
                    {
                        numGanador = Runner.LocalPlayer.AsIndex;
                        Debug.Log(numGanador);
                    }

                    Dictionary<string, SessionProperty> propiedades = new Dictionary<string, SessionProperty>();
                    propiedades.Add("Ganador", (SessionProperty)numGanador);

                    Runner.SessionInfo.UpdateCustomProperties(propiedades);

                    Runner.Despawn(this.Object);
                }
            }
        }

    }
