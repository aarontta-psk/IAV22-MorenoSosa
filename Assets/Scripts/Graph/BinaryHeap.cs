using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Un montículo binario, ¡útil para ordenar datos e implementar colas de prioridad!
/// </summary>
/// <typeparam name="T"><![CDATA[IComparable<T> type of item in the heap]]>.</typeparam>
public class BinaryHeap<T> : ICollection<T> where T : IComparable<T>
{
    // Constantes
    private const int DEFAULT_SIZE = 4;
    // Campos
    private T[] data = new T[DEFAULT_SIZE];
    private int inUse = 0;
    private int size = DEFAULT_SIZE;
    private bool isInternallySorted;

    public T Top => data[0];

    // Propiedades
    /// <summary>
    /// Devuelve los valores que hay en el montículo.
    /// </summary>
    public int Count
    {
        get { return inUse; }
    }

    /// <summary>
    /// Devuelve (o cambia) la capacidad del montículo.
    /// </summary>
    public int Capacity
    {
        get { return size; }
        set
        {
            int previousCapacity = size;
            size = Math.Max(value, inUse);
            if (size != previousCapacity)
            {
                T[] temp = new T[size];
                Array.Copy(data, temp, inUse);
                data = temp;
            }
        }
    }

    /// <summary>
    /// Crea un montículo binario.
    /// </summary>
    public BinaryHeap() { }

    /// <summary>
    /// Crea un montículo binario.
    /// </summary>
    /// <param name="data">Datos que se van a meter en el montículo.</param>
    /// <param name="count">Capacidad y numero de elementos del montículo.</param>
    private BinaryHeap(T[] data, int count)
    {
        Capacity = count;
        inUse = count;
        Array.Copy(data, this.data, count);
    }


    /// <summary>
    /// Elimina todos los elementos del montículo
    /// </summary>
    public void Clear()
    {
        this.inUse = 0;
        data = new T[size];
    }

    /// <summary>
    /// Anade un elemento al montículo
    /// </summary>
    /// <param name="item">El elemento que se va a añadir</param>
    public void Add(T item)
    {
        if (inUse == size)
        {
            Capacity *= 2;
        }
        data[inUse] = item;
        Float();
        inUse++;
    }

    /// <summary>
    /// Elimina el primer elemento del montículo.
    /// </summary>
    /// <returns>El elemento eliminado.</returns>
    public T Remove()
    {
        if (this.inUse == 0)
        {
            throw new InvalidOperationException("Cannot remove item, heap is empty.");
        }
        T v = data[0];
        inUse--;
        data[0] = data[inUse];
        data[inUse] = default(T); //Limpia el último nodo
        Drown();
        return v;
    }

    /// <summary>
    /// Operacion de flotación del monticulo
    /// </summary>
    private void Float()
    {
        isInternallySorted = false;
        int p = inUse;
        T item = data[p];
        int par = Parent(p);
        while (par > -1 && item.CompareTo(data[par]) < 0)
        {
            data[p] = data[par]; //Intercambia nodos
            p = par;
            par = Parent(p);
        }
        data[p] = item;
    }

    /// <summary>
    /// Operacion de hundido del montículo
    /// </summary>
    private void Drown()
    {
        isInternallySorted = false;
        int n;
        int p = 0;
        T item = data[p];
        while (true)
        {
            int ch1 = Child1(p);
            if (ch1 >= inUse) break;
            int ch2 = Child2(p);
            if (ch2 >= inUse)
            {
                n = ch1;
            }
            else
            {
                n = data[ch1].CompareTo(data[ch2]) < 0 ? ch1 : ch2;
            }
            if (item.CompareTo(data[n]) > 0)
            {
                data[p] = data[n]; //Intercambia nodos
                p = n;
            }
            else
            {
                break;
            }
        }
        data[p] = item;
    }

    private void EnsureSort()
    {
        if (isInternallySorted) return;
        Array.Sort(data, 0, inUse);
        isInternallySorted = true;
    }

    //Función auxiliar que calcula el padre de un nodo
    private static int Parent(int index)
    {
        return (index - 1) >> 1;
    }

    //Función auxiliar que calcula el primer hijo de un nodo
    private static int Child1(int index)
    {
        return (index << 1) + 1;
    }

    //Función auxiliar que calcula el segundo hijo de un nodo
    private static int Child2(int index)
    {
        return (index << 1) + 2;
    }


    public IEnumerator<T> GetEnumerator()
    {
        EnsureSort();
        for (int i = 0; i < inUse; i++)
        {
            yield return data[i];
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Comprueba si el monticulo contiene un elemento.
    /// </summary>
    /// <param name="item">Elemento que se va a buscar</param>
    /// <returns>true si el monticulo contiene el elemento; false en caso contrario.</returns>
    public bool Contains(T item)
    {
        EnsureSort();
        return data.Contains(item);
        //return Array.BinarySearch<T>(data, 0, inUse, item) >= 0;
    }

    /// <summary>
    /// Copia el monticulo binario a un array.
    /// </summary>
    /// <param name="array">Array en el que se va a copiar el monticulo.</param>
    /// <param name="arrayIndex">Posicion del array en el que se va a empezar a copiar.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        EnsureSort();
        Array.Copy(data, array, inUse);
    }

    /// <summary>
    /// Comprueba si es de solo-lectura
    /// </summary>
    public bool IsReadOnly
    {
        get { return false; }
    }

    /// <summary>
    /// Elimina un elemento del monticulo. 
    /// </summary>
    /// <param name="item">Elemento que se va a borrar.</param>
    /// <returns>true si se ha podido eliminar; false en caso contrario.</returns>
    public bool Remove(T item)
    {
        EnsureSort();
        int i = Array.BinarySearch<T>(data, 0, inUse, item);
        if (i < 0) return false;
        Array.Copy(data, i + 1, data, i, inUse - i - 1);
        inUse--;
        data[inUse] = default(T);
        return true;
    }

    public T Find(T item)
    {
        int i = Array.BinarySearch<T>(data, 0, inUse, item);
        if (i < 0) return default(T);
        return data[i];
    }
}