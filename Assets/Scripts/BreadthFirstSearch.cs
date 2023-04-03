using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script by Ian 'Scotch' Bell | last modified 3/4/23 by IB
 *  This script will contain the main algorithm for generating a path through the graph of gridcells
 *  using the BreadthFirstSearch algorithm
 */

public class BreadthFirstSearch : MonoBehaviour
{
    [SerializeField] GridCell rootCell;

    [SerializeField] GridCell destCell;

    List<GridCell> queue = new List<GridCell>();

    [SerializeField] List<GridCell> gridCellPath = new List<GridCell>();

    GridCell currentGridCellBeingSearched;

    bool searching = false;

    bool destinationFound = false;

    bool pathMade = false;

    //delegates & events
    public delegate void SetRootCellDelegate(GridCell _gridCell);
    public static SetRootCellDelegate setRootCellEvent = delegate { };

    public delegate void SetDestinationCellDelegate(GridCell _gridCell);
    public static SetDestinationCellDelegate setDestinationCellEvent = delegate { };

    public delegate void BreadthFirstSearchDelegate();
    public static BreadthFirstSearchDelegate breadthFirstSearchEvent = delegate { };

    public delegate void ClearColourFromCellsDelegate();
    public static ClearColourFromCellsDelegate clearColourFromCellsEvent = delegate { };

    private List<GridCell> checkedGridCells = new List<GridCell>();

    private void OnEnable()
    {
        //subscribe to events
        setDestinationCellEvent += SetDestCell;
        setRootCellEvent += SetRootCell;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && searching == false)
        {
            if (destCell != null)
            {
                searching = true;
                gridCellPath.Clear();
                //invoke the clear colour from nodes event
                breadthFirstSearchEvent.Invoke();
                clearColourFromCellsEvent.Invoke();
                queue.Clear();
                checkedGridCells.Clear();
                queue.Add(rootCell); //or the dynamically assigned 'starting point'
                currentGridCellBeingSearched = queue[0];

                destinationFound = false;
                pathMade = false;
                BFSAlgorithm();
            }
        }

        /*
        if(searching == true)
        {
        }*/


    }

    private void BFSAlgorithm()
    {
        while (searching)
        {
            if (destinationFound == false)
            {

                if (currentGridCellBeingSearched == destCell)
                {
                    Debug.Log("Found the destination");
                    //clear colour from nodes event
                    destinationFound = true;
                }
                else
                {
                    for (int i = 0; i < currentGridCellBeingSearched.neighbours.Count; i++)
                    {
                        if (currentGridCellBeingSearched.neighbours[i].pathCell == null)
                        {
                            currentGridCellBeingSearched.neighbours[i].SetPathCell(currentGridCellBeingSearched);
                        }
                        if (!checkedGridCells.Contains(currentGridCellBeingSearched.neighbours[i]))
                        {
                            queue.Add(currentGridCellBeingSearched.neighbours[i]);
                        }
                    }
                    checkedGridCells.Add(currentGridCellBeingSearched);
                    queue.Remove(currentGridCellBeingSearched);
                }

                if (queue.Count > 0)
                {
                    int lastNumber = queue.Count - 1;
                    currentGridCellBeingSearched = queue[lastNumber]; //if this was DFS: currentGridCellBeingDerped = stack.Last;
                    currentGridCellBeingSearched.SetColour(2);
                }
                else
                {
                    Debug.Log("There is no path.");
                    searching = false;
                }
            }
            else
            {
                if (!pathMade)
                {
                    if (currentGridCellBeingSearched == rootCell)
                    {
                        gridCellPath.Add(rootCell);
                        rootCell.SetColour(1);
                        pathMade = true;
                        searching = false;
                        Debug.Log("Finished making the path");
                    }
                    else
                    {
                        if (!gridCellPath.Contains(currentGridCellBeingSearched))
                        {
                            gridCellPath.Insert(0, currentGridCellBeingSearched);
                        }
                        currentGridCellBeingSearched.SetColour(1);
                        currentGridCellBeingSearched = currentGridCellBeingSearched.pathCell;
                    }
                }
            }
        }
    }

    private void SetDestCell(GridCell _gridCell)
    {
        destCell = _gridCell;
    }

    private void SetRootCell(GridCell _gridCell)
    {
        rootCell = _gridCell;
    }

    private void OnDestroy()
    {
        setDestinationCellEvent -= SetDestCell;
        setRootCellEvent -= SetRootCell;
    }

}
