using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UCM.IAV.Navegacion;

namespace es.ucm.fdi.iav.rts
{
    public class InfluenceMap : GraphGrid
    {
        private enum DROPOFF_TYPE { LINEAR,  LONG, SHORT};

        private DROPOFF_TYPE dropoffType = DROPOFF_TYPE.LINEAR;

        private List<Influence> fremenUnits = new List<Influence>();
        private List<Influence> harkonnenUnits = new List<Influence>();
        private List<Influence> grabenUnits = new List<Influence>();

        private bool[] influenceShow;

        // Start is called before the first frame update
        private void Start () 
        {
            StartMap();

            influenceShow = new bool[4] {true, true, true, true};
            //influenceShow = new bool[4];
        }

        // Update is called once per frame

        void Update()
        {
            ComputeTotalInfluence();
        }

        public void ComputeTotalInfluence() 
        {
            RelaxVertexs();
            ComputeInfluence(grabenUnits, 1);
            ComputeInfluence(harkonnenUnits, 2);
            ComputeInfluence(fremenUnits, 3);

            ColorInfluence();
        }


        void ComputeInfluence(List<Influence> units, int team)
        {
            float dropOff;
            List<Vertex> pending = new List<Vertex>();
            List<Vertex> visited = new List<Vertex>();
            List<Vertex> frontier;
            Vertex[] neighbours;
            foreach (Influence u in units)
            {
                if (u == null)
                    continue;

                Vector3 uPos = u.transform.position;
                Vertex vert = GetNearestVertex(uPos);

                if (vert == null)
                    continue;

                pending.Add(vert);

                for (int i = 1; i <= u.radius_; i++)
                {
                    frontier = new List<Vertex>();
                    foreach (Vertex p in pending)
                    {
                        if (visited.Contains(p))
                            continue;
                        visited.Add(p);
                        dropOff = GetDropOff(u, i);
                        vertices[p.id].AddValue(dropOff, team);
                        neighbours = GetNeighbours(vert);
                        frontier.AddRange(neighbours);
                    }
                    pending = new List<Vertex>(frontier);
                }
                
            }
        }

        Color ColorToRGB(float R, float G, float B, float A)
        {
            return new Color(R/255, G/255, B/255, A);
        }

        void ColorInfluence()
        {
            for(int i = 0; i < vertices.Count; i++)
            {
                vertices[i].GetComponent<MeshRenderer>().material.color = ColorToRGB(255, 255, 255, 0.0f);
                //switch (vertices[i].team)
                //{
                    //case TEAM.NONE:
                    //    break;       
                    //case TEAM.GRABEN:
                    //    if (influenceShow[(int)TEAM.GRABEN])
                    //        vertices[i].GetComponent<MeshRenderer>().material.color = ColorToRGB(26, 148, 49, Math.Min(vertices[i].value / 100.0f, 0.7f));
                    //    break;       
                    //case TEAM.HARKONNEN:
                    //    if (influenceShow[(int)TEAM.HARKONNEN])
                    //        vertices[i].GetComponent<MeshRenderer>().material.color = ColorToRGB(8, 96, 168, Math.Min(vertices[i].value / 100.0f, 0.7f));
                    //    break;
                    //case TEAM.FREMEN:
                    //    if (influenceShow[(int)TEAM.FREMEN])
                    //        vertices[i].GetComponent<MeshRenderer>().material.color = ColorToRGB(255, 216, 1, Math.Min(vertices[i].value / 100.0f, 0.7f));
                    //    break;       
                //}
            }
        }

        float GetDropOff(Influence u, int i)
        {
            switch (dropoffType)
            {
                case DROPOFF_TYPE.LINEAR:
                    return u.LinearDropoff(i);
                case DROPOFF_TYPE.LONG:
                    return u.LongerDropoff(i);
                case DROPOFF_TYPE.SHORT:
                    return u.ShorterDropoff(i);
            }
            return 0.0f;
        }

        //public void ShowInfluence(TEAM team)
        //{
        //    influenceShow[(int)team] = !influenceShow[(int)team];
        //}

        public void ShowAllInfluence(bool show)
        {
            for (int i = 0; i < influenceShow.Length; i++)
            {
                influenceShow[i] = show;
            }
        }

        //public void AddUnit(Influence u, TEAM team)
        //{
        //    switch (team)
        //    {
        //        case TEAM.NONE:
        //            break;
        //        case TEAM.FREMEN:
        //            //Debug.Log("A�adido a fremen");
        //            fremenUnits.Add(u);
        //            break;
        //        case TEAM.GRABEN:
        //            //Debug.Log("A�adido a graben");
        //            grabenUnits.Add(u);
        //            break;
        //        case TEAM.HARKONNEN:
        //            //Debug.Log("A�adido a harkonnen");
        //            harkonnenUnits.Add(u);
        //            break;
        //    }
        //}

        //public void RemoveUnit(Influence u, TEAM team)
        //{
        //    switch (team)
        //    {
        //        case TEAM.NONE:
        //            break;
        //        case TEAM.FREMEN:
        //            //Debug.Log("Eliminado a fremen");
        //            fremenUnits.Remove(u);
        //            break;
        //        case TEAM.GRABEN:
        //            //Debug.Log("Eliminado a graben");
        //            grabenUnits.Remove(u);
        //            break;
        //        case TEAM.HARKONNEN:
        //            //Debug.Log("Eliminado a harkonnen");
        //            harkonnenUnits.Remove(u);
        //            break;
        //    }
        //}
    }
}
