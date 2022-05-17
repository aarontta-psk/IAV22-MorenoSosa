using System;

namespace UCM.IAV.Navegacion
{
    public class Node : IComparable<Node>
    {

        public int vertexId; // current path vertex id
        public int prevVertId; // prev vertex id
        public float costSoFar; // cost from temp path made
        public float estimatedTotalCost; // estimate cost 

        public int CompareTo(Node other)
        {
            return (int)(this.estimatedTotalCost - other.estimatedTotalCost);
        }

        public bool Equals(Node other)
        {
            return (other.vertexId == this.vertexId);
        }

        public override bool Equals(object obj)
        {
            Node other = (Node)obj;
            return (other.vertexId == this.vertexId);
        }

        public override int GetHashCode()
        {
            return this.vertexId.GetHashCode();
        }

        //// Define the is greater than operator.
        //public static bool operator >(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) > 0;
        //}

        //// Define the is less than operator.
        //public static bool operator <(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) < 0;
        //}

        //// Define the is greater than or equal to operator.
        //public static bool operator >=(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) >= 0;
        //}

        //// Define the is less than or equal to operator.
        //public static bool operator <=(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) <= 0;
        //}

        //// Define the equal to operator.
        //public static bool operator ==(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) == 0;
        //}

        //// Define the equal to operator.
        //public static bool operator !=(Node nodeA, Node nodeB)
        //{
        //    return nodeA.CompareTo(nodeB) != 0;
        //}
    }
}