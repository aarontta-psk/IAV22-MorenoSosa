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
namespace UCM.IAV.Navegacion
{

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Abstract class for graphs
    /// </summary>
    public abstract class Graph : MonoBehaviour
    {
        public GameObject vertexPrefab;
        protected List<Vertex> vertices;
        protected List<List<Vertex>> neighbourVertex;
        protected List<List<float>> costs;
        protected bool[,] mapVertices;
        protected float[,] costsVertices;
        protected int numCols, numRows;

        // this is for informed search like A*
        public delegate float Heuristic(Vertex a, Vertex b);

        // Used for getting path in frames
        protected List<Vertex> path;


        public virtual void Start()
        {
            //Load();
        }

        public virtual void Load() { }

        public virtual int GetSize()
        {
            if (ReferenceEquals(vertices, null))
                return 0;
            return vertices.Count;
        }

        public int GetCols() { return numCols; }
        public int GetRows() { return numRows; }

        public virtual void UpdateVertexCost(Vector3 position, float costMultipliyer) { }

        public virtual Vertex GetNearestVertex(Vector3 position)
        {
            return null;
        }

        public virtual GameObject GetRandomPos()
        {
            return null;
        }

        public virtual Vertex[] GetNeighbours(Vertex v)
        {
            if (ReferenceEquals(neighbourVertex, null) || neighbourVertex.Count == 0 ||
                v.id < 0 || v.id >= neighbourVertex.Count)
                return new Vertex[0];
            return neighbourVertex[v.id].ToArray();
        }

        public virtual float[] GetNeighboursCosts(Vertex v)
        {
            if (ReferenceEquals(neighbourVertex, null) || neighbourVertex.Count == 0 ||
                v.id < 0 || v.id >= neighbourVertex.Count)
                return new float[0];

            Vertex[] neighs = neighbourVertex[v.id].ToArray();
            float[] costsV = new float[neighs.Length];
            for (int neighbour = 0; neighbour < neighs.Length; neighbour++) {
                int j = (int)Mathf.Floor(neighs[neighbour].id / numCols);
                int i = (int)Mathf.Floor(neighs[neighbour].id % numCols);
                costsV[neighbour] = costsVertices[j, i];
            }

            return costsV;
        }

        // Encuentra caminos óptimos
        public List<Vertex> GetPathBFS(GameObject srcO, GameObject dstO)
        {
            if (srcO == null || dstO == null)
                return new List<Vertex>();
            Vertex[] neighbours;
            Queue<Vertex> q = new Queue<Vertex>();
            Vertex src = GetNearestVertex(srcO.transform.position);
            Vertex dst = GetNearestVertex(dstO.transform.position);
            Vertex v;
            int[] previous = new int[vertices.Count];
            for (int i = 0; i < previous.Length; i++)
                previous[i] = -1;
            previous[src.id] = src.id; // El vértice que tenga de previo a sí mismo, es el vértice origen
            q.Enqueue(src);
            while (q.Count != 0)
            {
                v = q.Dequeue();
                if (ReferenceEquals(v, dst))
                {
                    return BuildPath(src.id, v.id, ref previous);
                }

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id; // El vecino n tiene de 'padre' a v
                    q.Enqueue(n);
                }
            }
            return new List<Vertex>();
        }

        // No encuentra caminos óptimos
        public List<Vertex> GetPathDFS(GameObject srcO, GameObject dstO)
        {
            if (srcO == null || dstO == null)
                return new List<Vertex>();
            Vertex src = GetNearestVertex(srcO.transform.position);
            Vertex dst = GetNearestVertex(dstO.transform.position);

            if (src == null || dst == null)
                return new List<Vertex>();

            Vertex[] neighbours;
            Vertex v;
            int[] previous = new int[vertices.Count];
            for (int i = 0; i < previous.Length; i++)
                previous[i] = -1;
            previous[src.id] = src.id;
            Stack<Vertex> s = new Stack<Vertex>();
            s.Push(src);
            while (s.Count != 0)
            {
                v = s.Pop();
                if (ReferenceEquals(v, dst))
                {
                    return BuildPath(src.id, v.id, ref previous);
                }

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id;
                    s.Push(n);
                }
            }
            return new List<Vertex>();
        }

        public List<Vertex> GetPathAstar(GameObject srcO, GameObject dstO, Heuristic h = null)
        {
            Vertex srcVtx = GetNearestVertex(srcO.transform.position);
            Vertex dstVtx = GetNearestVertex(dstO.transform.position);

            //Si el destino y el origen son el mismo, se devuelve una lista vacía
            if (srcVtx == dstVtx) return new List<Vertex>();

            BinaryHeap<Vertex> openSet = new BinaryHeap<Vertex>();
            
            //Inicializamos el resto de posibles nodos del camino
            int[] cameFrom = new int[vertices.Count];
            float[] gScore = new float[vertices.Count];
            float[] fScore = new float[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                cameFrom[i] = -1;
                gScore[i] = float.PositiveInfinity;
                fScore[i] = float.PositiveInfinity;
            }

            //Añadimos a las listas el primer nodo con su coste
            srcVtx.value = h(srcVtx, dstVtx);
            openSet.Add(srcVtx);

            cameFrom[srcVtx.id] = srcVtx.id;
            gScore[srcVtx.id] = 0;
            fScore[srcVtx.id] = h(srcVtx, dstVtx);

            while (openSet.Count > 0)
            {
                //Al usar un BinaryHeap, la operación se hace en O(logN)
                Vertex currentVtx = openSet.Remove();

                //Si hemos llegado al final, se construye el camino
                if (currentVtx.id == dstVtx.id)
                    return BuildPath(srcVtx.id, currentVtx.id, ref cameFrom);

                //Tomamos los vecinos y sus costes
                Vertex[] neighbours = GetNeighbours(vertices[currentVtx.id]);
                float[] neighCosts = GetNeighboursCosts(vertices[currentVtx.id]);

                for (int neighbour = 0; neighbour < neighbours.Length; neighbour++)
                {
                    //tentative_gScore es la distancia desde el inicio al vecino actual
                    float tentative_gScore = gScore[currentVtx.id] + neighCosts[neighbour];
                    if (tentative_gScore <= gScore[neighbours[neighbour].id])
                    {
                        //Al ser mejor camino, nos guardamos su información
                        cameFrom[neighbours[neighbour].id] = currentVtx.id;
                        gScore[neighbours[neighbour].id] = tentative_gScore;
                        fScore[neighbours[neighbour].id] = tentative_gScore + h(neighbours[neighbour], dstVtx);
                        neighbours[neighbour].value = fScore[neighbours[neighbour].id];

                        //Y si no estaba ya considerado, se añade a la cola
                        if (!openSet.Contains(neighbours[neighbour]))
                            openSet.Add(neighbours[neighbour]);
                    }
                }
            }

            //Si no se puede llegar al final, entonces no se puede avanzar
            return new List<Vertex>();
        }

        public List<Vertex> Smooth(List<Vertex> inputPath)
        {
            //Si el camino es sólo de 2 o menos nodos, entonces no se puede suavizar
            if (inputPath.Count <= 2)
                return inputPath;

            List<Vertex> outputPath = new List<Vertex>();

            outputPath.Add(inputPath[0]);

            //Referencia por dónde vamos, empieza en 2 porque se asume que se puede llegar desde el primer nodo
            int inputIndex = 2;

            //Hasta llegar al último nodo
            while (inputIndex < inputPath.Count - 1)
            {
                Vertex fromPt = outputPath[outputPath.Count - 1];
                Vertex toPt = inputPath[inputIndex];
                
                //Dirección del origen al punto 
                Vector3 dir = toPt.transform.position - fromPt.transform.position;

                //Layer 6 del jugador
                LayerMask layer = 1 << 6;

                //Si colisiona, entonces necesitamos el nodo en el camino
                if (Physics.Raycast(fromPt.transform.position, dir.normalized, out RaycastHit hit, dir.magnitude, layer))
                    outputPath.Add(inputPath[inputIndex - 1]);

                //De lo contrario, avanzamos al siguiente porque este se puede saltar
                inputIndex++;
            }

            //Una vez acabado, se añade el último que no se comprueba
            outputPath.Add(inputPath[inputPath.Count - 1]);

            return outputPath; 
        }

        // Reconstruir el camino, dando la vuelta a la lista de nodos 'padres' /previos que hemos ido anotando
        private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
        {
            List<Vertex> path = new List<Vertex>();

            if (dstId < 0 || dstId >= vertices.Count) 
                return path;

            int prev = dstId;
            do
            {
                path.Add(vertices[prev]);
                prev = prevList[prev];
            } while (prev != srcId);
            return path;
        }

        // Heurística de distancia euclídea
        public float EuclidDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Vector3.Distance(posA, posB);
        }

        // Heurística de distancia Manhattan
        public float ManhattanDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        }
    }
}