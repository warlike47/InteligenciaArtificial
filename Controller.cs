using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private float difuso;
    public List<string> sintomas = new List<string>();
    public List<string> enfermedades = new List<string>();
    public List<float> contadores = new List<float>();

    //public List<string> agenda = new List<string>();
    public List<(float, string)> agenda = new List<(float, string)>();

    public InputField sintomaField;
    public GameObject panelAvisoEnfermedad;
    public GameObject panelSintomaIngresado;
    public Text textoDiagnostico;

    private void Awake()
    {
        //if(sintomas[0])
    }

    // Start is called before the first frame update
    void Start()
    {
        agenda.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(sintomas.Count == 0)
        {
            panelAvisoEnfermedad.SetActive(true);

            if (agenda.Count == 0)
                textoDiagnostico.text = "Diagnostico: no se ha encontrado una enfermedad posible";
            else if (Iguales())
                textoDiagnostico.text = "Diagnostico: empate de enfermedades";
            else
                textoDiagnostico.text = "Diagnostico: " + agenda[MayorNumero()].Item2 + "( en un " + contadores[MayorNumero()] * 100 + "%)";
        }
    }

    public int MayorNumero()
    {
        float mayor = 0;
        int posicion = 0;
        for (int i = 0; i < contadores.Count; i++){
            if (contadores[i] > mayor)
            {
                mayor = contadores[i];
                posicion = i;
            }
                
        }

        return posicion;
    }

    public bool Iguales()
    {
        float numero = contadores[0];
        for(int i = 1; i < contadores.Count; i++)
        {
            if (numero != contadores[i])
                return false;
        }

        return true;
    }

    private void ModificarAgenda(float contador, string enfermedad)
    {
        if (agenda.Contains((contador, enfermedad)))
            agenda[agenda.IndexOf((contador, enfermedad))] = (contador, enfermedad);
        else
            agenda.Add((contador, enfermedad));
    }

    public void ValidarReglas(string sintoma)
    {
        if(sintoma == "fiebre")
        {
            difuso = 0.2f;
            contadores[0] = contadores[0] + (1- contadores[0]) * difuso;
            ModificarAgenda(contadores[0], enfermedades[0]);
        }

        if (sintoma == "malestar")
        {
            difuso = 0.7f;
            contadores[0] = contadores[0] + (1 - contadores[0]) * difuso;
            ModificarAgenda(contadores[0], enfermedades[0]);

            difuso = 0.6f;
            contadores[1] = contadores[1] + (1 - contadores[1]) * difuso;
            ModificarAgenda(contadores[1], enfermedades[1]);
        }

        /*if (sintoma == "tos")
        {
            contadores[0]++;
            ModificarAgenda(contadores[0], enfermedades[0]);
        }*/

        if (sintoma == "dolor de garganta")
        {
            difuso = 0.8f;
            contadores[1] = contadores[1] + (1 - contadores[1]) * difuso;
            ModificarAgenda(contadores[1], enfermedades[1]);
        }
    }

    public void TerminaSintomas()
    {
        sintomas.Clear();
    }

    public IEnumerator AgregarSintoma()
    {
        panelSintomaIngresado.SetActive(true);
        yield return new WaitForSeconds(1);
        panelSintomaIngresado.SetActive(false);
    }

    public void Continuar()
    {
        if(sintomas.Contains(sintomaField.text))
        {
            sintomas.Remove(sintomaField.text);
            ValidarReglas(sintomaField.text);
            
        }
        StartCoroutine(AgregarSintoma());
    }

    public void RecargarEscena()
    {
        SceneManager.LoadScene("Principal");
    }
}
