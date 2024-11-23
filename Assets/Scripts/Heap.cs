using UnityEngine;
using System.Collections;
using System;
//A Heap class where we will save all our objects. This allows for faster computing due to not having to make multiple copies of the same object, and 
//due to using a tree structure for optimization. The class is general and can hold any type T that implements the necessary subfunctions.
public class Heap<T> where T : IHeapItem<T> {
    //holds all the items
    T[] items;
    //holds the actual amount of used items thats in the heap
    // (since we dont necessarily use all maxheapsize items)
    int currentItemCount;
    public Heap(int maxHeapSize){
        items = new T[maxHeapSize];

    }
    //adds given item to the last place of the heap, and sorts the heap so that it will be at the correct place in the heap. 

    public void Add(T item){
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++; // Updates the currentitemcount 
    }

    // Returns the first item of the heap (the most efficient node) and removes it from the openset, in addition to updating the itemcount.
    public T RemoveFirst(){
        T firstItem = items[0];
        currentItemCount--;
        //Makes the element with a big chance of being the least effective, the first. Then it sorts the graph to make its structure be the same as earlier.
        //We essentially just use the previously implemented functions with the last item, for regaining the graphs structure. We want to use specifically
        // the last item because this makes it so we can reduce the size of the heap without losing the last Node. 
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    // Gives out 1 if the item is in the heap of interest.
    public bool Contains(T item){
        return Equals(items[item.HeapIndex], item);

    }
    //If we have added an item, we want to make sure it is placed sufficiently high in the graph.
    public void UpdateItem(T item){
        SortUp(item);
        
    }
    //Used as a variable in the algorithm.
    public int Count {
        get {
            return currentItemCount;
        }
    }

    //
    // a function that sorts the heap downwards, from a start parent node. It stops only when the given input node has come to a place where 
    // the node below is not more efficient. Essentially just the SortUp function, but for the reverse order.
    void SortDown(T item) {
		while (true) {//iterate over all nodes top to bottom from the start node. 
			int childIndexLeft = item.HeapIndex * 2 + 1; //math expression from graph theory giving the index of the given node's children.
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0; //swapindex always holds the most efficient of the childnodes.

			if (childIndexLeft < currentItemCount) { // checks if the left index actually is within the heap. 
                                                     //If it is, set it as the standard node to be swapped with the parentnode
				swapIndex = childIndexLeft; 
				if (childIndexRight < currentItemCount) { //checks if the right index is actually within the heap 
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) { // check if the right node is actually the most efficient. if it is,
                        // make this the swapindex.
						swapIndex = childIndexRight; // 
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) { // if one of the child nodes is actually more efficient, swap the parentnode with the most efficient childnode. 
					Swap (item,items[swapIndex]); // swaps the item with the most efficient childnode. Since this function updates item's heapindex, it will 
                    //make it so that for the next loop, the given node is at the place where the more efficient child node used to be.
				}
				else { //if the parent node is actually the most efficient, the sorting will be over.
					return;
				}

			}
			else { // if the left child is not in the heap, neither is the right, and we can just exit the function.
				return;
			}

		}
	}

    //Sorts the heap so that the most efficient node is always at the top. It does this from a bottom - top approach.
    void SortUp(T item){
        // this formula is used to find a childs parentnode in an arbitrary graph. It works regardless of whether the node is the right or left node, because
        // the formula uses the fact that c# rounds down integer divisions (so Parentindex = n + 1 gives the same result as Parentindex = n)
        int parentIndex = (item.HeapIndex - 1) / 2;

        //want to do this until the graph / heap is completely sorted. When it is, the break; will exit the loop.
        while (true){
            //
            T parentItem = items[parentIndex];
            //Remember : Result is ; 1 if this node (the left node) is the most efficient, 0 if both are as efficient, -1 if this is the least efficient.
            //This means that we say that if the childnode is more efficient (has a lower fcost and possibly hcost), we swap the childnode with the parentnode
            // making the more efficient node be at the top of the graph.
            if (item.CompareTo(parentItem) > 0){
                Swap(item, parentItem);
            }

            else {
                //If we ALWAYS sort the graph, so that before adding a new element, it is always sorted correctly, then that will mean that if the parentnode
                // is not more efficient than the childnode, the heap will be sorted correctly. (this is not necessarily true if we dont keep the heap sorted
                // by all times. Since we do this though, the algorithm will work correctly.)
                break;
            }
            //if the childnode was moved up, we have to check once more, and then we will need the newest parentnode. 
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // Swaps two items by swapping their contents 
    // with each others on the heap, then updating 
    //each others heapindex.
    void Swap(T itemA, T itemB){
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

//an interface (a pure virtual class) that makes sure all objects thats put in the heap has a HeapIndex. It also makes sure it implements the icomparable methods.
//The icomparable method has syntax T1.CompareTo(T2) and gives out [-1,0,1] for different scenarios depending on how it was overloaded.
// The implementation of CompareTo for the node class is as follows: Gives out a value in [-1,0,1] based on the two nodes f and hcosts. 
//Result is ; 1 if this node (the left node) is the most efficient, 0 if both are as efficient, -1 if this is the least efficient.
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}