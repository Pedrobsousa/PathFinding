using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generic class that stores other objs with IComparable
//Sorts on Enqueue and Dequeue using Binary Heap. Node.CompareTo used to check priority 
//to move Node on the Priority Queue
//Lowest Priority Always on Top/Front of Queue
public class PriorityQueue<T> where T: IComparable<T>
{
    List<T> data;

    public int Count { get { return data.Count; } }

    //Constructor
    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        //We start on 0 position/array index
        int childIndex = data.Count - 1;

        while(childIndex > 0)//not at the top
        {
            //One level above child if 5 then 4 then 2
            int parentIndex = (childIndex - 1) / 2;

            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)//same priority
            {
                break;
            }
            //Swap priorities values
            T tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex;
            
        }
    }

    //Remove front of the Queue while keeping other Nodes sorted
    public T Dequeue()
    {
        int lastIndex = data.Count - 1;
        //Top of the heap
        T frontItem = data[0];

        data[0] = data[lastIndex];

        data.RemoveAt(lastIndex);
        lastIndex--;//Since we removed the last T

        int parentIndex = 0;

        //Traverse heap downward and Compare to other Nodes prio

        while (true)
        {
            int childIndex = parentIndex * 2 + 1;//child on the left

            if(childIndex > lastIndex)//Bottom of Heap
            {
                break;
            }

            int rightChild = childIndex + 1;//left + 1

            //Right child is higher Priority
            if(rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)
            {
                childIndex = rightChild;
            }

            if(data[parentIndex].CompareTo(data[childIndex]) <= 0)//Parent and Child are in correct order
            {
                break;
            }

            //If they arent, required Swap
            T tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;
        }

        return frontItem;
    }
    //Check first Item without removing it
    public T Peek()
    {
        T frontItem = data[0];
        return frontItem;
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public List<T> ToList()
    {
        return data;
    }
}
