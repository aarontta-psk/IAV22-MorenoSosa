/*    
    Obra original:
        Copyright (c) 2018 Packt
        Unity 2018 Artificial Intelligence Cookbook - Second Edition, by Jorge Palacios
        https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition
        MIT License

    Modificaciones:
        Copyright (C) 2020-2022 Federico Peinado
        http://www.federicopeinado.com

        Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
        Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
        Contacto: email@federicopeinado.com
*/
using es.ucm.fdi.iav.rts;

namespace UCM.IAV.Navegacion
{
    using UnityEngine;
    using System;

    // Puntos representativos o vértice (común a todos los esquemas de división, o a la mayoría de ellos)
    [System.Serializable]
    public class Vertex : MonoBehaviour, IComparable<Vertex>
    {
        const float MAX_VALUE = 100.0f;
        public float relaxMultiplyer = 3.0f;


        //public TEAM team = TEAM.NONE; 
        /// <summary>
        /// Identificador del vértice 
        /// </summary>
        public int id;

        // Todos los valores de cada equipo
        public float[] values;

        // Valor más alto del vertex
        public float value;

        Vertex()
        {
            //values = new float[Enum.GetNames(typeof(TEAM)).Length];

            //for(int i = 0; i < values.Length; i++){
            //    values[i] = 0.0f;
            //}
            
            //value = 0.0f;
        }

        public void AddValue(float val, int teamN)
        {
            //values[teamN] += val;
            //if (values[teamN] > MAX_VALUE)
            //    values[teamN] = MAX_VALUE;

            //if (value < values[teamN]){
            //    team = (TEAM)teamN;
            //    value = values[teamN];
            //}
        }

        public int CompareTo(Vertex other)
        {
            float result = this.value - other.value;
            return (int)(Mathf.Sign(result) * Mathf.Ceil(Mathf.Abs(result)));
        }

        public bool Equals(Vertex other)
        {
            return (other.id == this.id);
        }

        public override bool Equals(object obj)
        {
            Vertex other = (Vertex)obj;
            if (ReferenceEquals(obj, null)) return false;
            return (other.id == this.id);
        }

        public void RelaxValues()
        {
            //for (int i = 0; i < values.Length; i++)
            //{
            //    values[i] = values[i] - Time.deltaTime * relaxMultiplyer / RTSScenarioManager.Instance.TimeScale;
            //    if (values[i] < 0.0f) values[i] = 0.0f;
            //    value = Math.Max(values[i], value - Time.deltaTime * relaxMultiplyer / RTSScenarioManager.Instance.TimeScale);
            //}

            //if (value <= 0.0f) team = TEAM.NONE;
        }

        public void ResetValues()
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 0.0f;
            }

            value = 0.0f;
            //team = TEAM.NONE;
        }
    }
}
