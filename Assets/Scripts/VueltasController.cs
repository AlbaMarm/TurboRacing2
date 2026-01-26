using Fusion;
using System.Collections;
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
    }
