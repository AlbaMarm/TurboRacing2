using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion;
using UnityEngine;

public class ContadorController : NetworkBehaviour
{
    public TextMesh texto;

    [Networked]
    public int contador {  get; set; }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            int contadorInicial = 3;

            Dictionary<string, SessionProperty> Propiedades = new Dictionary<string, SessionProperty>();
            Propiedades.Add("Contador", (SessionProperty)contadorInicial);
            Runner.SessionInfo.UpdateCustomProperties(Propiedades);

            StartCoroutine("cuentaAtras");
        }
    }

    public override void Render()
    {
        if (contador > 0)
        {
            texto.text = contador.ToString();
        }
        else
        {
            texto.text = "ADELANTE!!!!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(HasStateAuthority)
        {
            ReadOnlyDictionary<string, SessionProperty> c = Runner.SessionInfo.Properties;

            if(c.TryGetValue("Contador", out SessionProperty p))
            {
                contador = (int)p.PropertyValue;
            }
        }
    }

    IEnumerator cuentaAtras()
    {
        while(contador>3)
        {
            if (HasStateAuthority)
            {
                ReadOnlyDictionary<string, SessionProperty> c = Runner.SessionInfo.Properties;

                if (c.TryGetValue("Contador", out SessionProperty p))
                {
                    int contadorAux = (int)p.PropertyValue;

                    Dictionary<string, SessionProperty> Propiedades = new Dictionary<string, SessionProperty>();
                    Propiedades.Add("Contador", (SessionProperty)contadorAux);
                    Runner.SessionInfo.UpdateCustomProperties(Propiedades);
                }
            }
            yield return new WaitForSeconds(1);
        }
        
    }
}
