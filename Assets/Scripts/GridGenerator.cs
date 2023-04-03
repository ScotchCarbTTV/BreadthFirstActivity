using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script by Ian 'Scotch' Bell | last modified date 3/4/23 by IB
 * This script is going to taken in a prefab game object (grid node or similar) and spawn them
 * in a regular square grid. The script will allow us to modify the width and heigh in the Unity Editor * 
 */


public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject gridCellPrefab;

    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(gridWidth, gridHeight);
    }

    private void GenerateGrid(int _width, int _height)
    {
        GameObject newGridCell;

        GridCell rootGridCell;

        bool rootGridCellSet = false;

        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                newGridCell = Instantiate(gridCellPrefab, new Vector3(x, 1, y), gameObject.transform.rotation, gameObject.transform);
                newGridCell.name = "Gridcell " + x.ToString() + "," + y.ToString();

                if (!rootGridCellSet)
                {
                    //set this newGridCell instance as the rootnode in the BFS algorithm
                    if (newGridCell.TryGetComponent<GridCell>(out rootGridCell))
                    {
                        BreadthFirstSearch.setRootCellEvent(rootGridCell);
                        rootGridCellSet = true;
                    }
                    
                }
            }
        }

    }

}
