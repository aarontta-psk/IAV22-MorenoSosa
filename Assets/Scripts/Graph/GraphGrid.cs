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
    using System.Collections.Generic;
    using System.IO;
    //using UCM.IAV.Movimiento;
    using UnityEditor;

    public class GraphGrid : Graph
    {
        public float cellSize = 1f;


        protected GameObject[] vertexObjs;


        public GameObject[] GetVertexObjs() { return vertexObjs; }

        private int GridToId(int x, int y)
        {
            return Math.Max(numRows, numCols) * y + x;
        }

        private Vector2 IdToGrid(int id)
        {
            Vector2 location = Vector2.zero;
            location.y = Mathf.Floor(id / numCols);
            location.x = Mathf.Floor(id % numCols);
            return location;
        }

        public void StartMap()
        {
            //Vector3 terrainSize = RTSScenarioManager.Instance.Scenario.terrainData.size;

            //int width = (int)(terrainSize.x / cellSize);
            //int depth = (int)(terrainSize.z / cellSize);

            //LoadMap(width, depth);
        }

        private void LoadMap(int width, int depth)
        {
            numRows = depth + 1;
            numCols = width + 1;

            int j = 0, i = 0, id = 0;

            Vector3 position = Vector3.zero;
            Vector3 scale = Vector3.zero;

            vertices = new List<Vertex>(numRows * numCols);
            neighbourVertex = new List<List<Vertex>>(numRows * numCols);
            vertexObjs = new GameObject[numRows * numCols];

            for (i = 0; i < numRows; i++){
                for (j = 0; j < numCols; j++){
                    position.x = j * cellSize;
                    position.z = i * cellSize;
                    id = GridToId(j, i);

                    vertexObjs[id] = Instantiate(vertexPrefab, position, Quaternion.identity, transform) as GameObject;
                    vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());

                    Vertex v = vertexObjs[id].AddComponent<Vertex>();
                    v.id = id;
                    vertices.Add(v);
                    neighbourVertex.Add(new List<Vertex>());
                    scale = vertexObjs[id].transform.localScale;
                    scale *= cellSize;
                    scale.y = 0.2f;
                    vertexObjs[id].transform.localScale = scale;
                }
            }

            //Leemos vecinos
            for (i = 0; i < numRows; i++)
                for (j = 0; j < numCols; j++)
                    SetNeighbours(j, i);
        }

        protected void SetNeighbours(int x, int y, bool get8 = false)
        {
            int col = x;
            int row = y;

            int i, j;
            int vertexId = GridToId(x, y);
            neighbourVertex[vertexId] = new List<Vertex>();
            Vector2[] pos = new Vector2[0];
            if (get8) {
                pos = new Vector2[8];
                int c = 0;
                for (i = row - 1; i <= row + 1; i++) {
                    for (j = col - 1; j <= col; j++) {
                        pos[c] = new Vector2(j, i);
                        c++;
                    }
                }
            }
            else {
                pos = new Vector2[4];
                pos[0] = new Vector2(col, row - 1);
                pos[1] = new Vector2(col - 1, row);
                pos[2] = new Vector2(col + 1, row);
                pos[3] = new Vector2(col, row + 1);
            }

            foreach (Vector2 p in pos) {
                i = (int)p.y;
                j = (int)p.x;

                if (i < 0 || j < 0)
                    continue;
                if (i >= numRows || j >= numCols)
                    continue;
                if (i == row && j == col)
                    continue;

                int id = GridToId(j, i);
                neighbourVertex[vertexId].Add(vertices[id]);
                //costsVertices[i, j] = defaultCost;
            }
        }

        public override Vertex GetNearestVertex(Vector3 position)
        {
            int col = (int) Math.Round(position.x / cellSize);
            int row = (int) Math.Round(position.z / cellSize);
            Vector2 p = new Vector2(col, row);

            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            
            if(id < vertices.Count)
                return vertices[id];

            return null;
        }

        public override GameObject GetRandomPos()
        {
          
            return null;
        }

        public void RelaxVertexs()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].RelaxValues();
            }
        }

        public void ResetVertexs()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].ResetValues();
            }
        }
    }
}
