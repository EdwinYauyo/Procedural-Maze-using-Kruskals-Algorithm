using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generating_frame : MonoBehaviour
{

    public GameObject prefab;
    public float scaleMaze;
    [Range(1,30)]
    public int width;

    [Range(1, 30)]
    public int height;

    [HideInInspector] public Tile[] tiles;

    public List<Edge> edges;

    [HideInInspector] public int EdgeIndex = 0;
    [SerializeField] private int orderRandom = 0;


    public void Start()
    {
        prefab.transform.localScale = new Vector3(10 * scaleMaze, prefab.transform.localScale.y, prefab.transform.localScale.z);

        tiles = new Tile[width * height];

        for (int i = 0; i < width * height; i++)
        {
            tiles[i] = new Tile();
        }

        edges = new List<Edge>();


        Ex(0);
        
    }

    void Ex(float t)
    {
        StartCoroutine(Execution(t));
    }

    IEnumerator Execution(float t)
    {
        spawnLeftRightBoundaries();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        spawnUpDownBoundaries();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        spawnInnerEdgesLeftRight();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        spawnInnerEdgesUpDown();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        StartCoroutine(removeEdgeCoroutine());
        
        
    }

    // Generating frame (outter boundaries)
    public void spawnLeftRightBoundaries()
    {
        for (int z = 1; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                if (x==0 || x== width)
                {
                    //Instantiate(prefab, new Vector3((x * 10*scaleMaze) + 5*scaleMaze, 0, (z * 10*scaleMaze) + 5*scaleMaze), Quaternion.Euler(0, 90, 0));
                    Instantiate(prefab, new Vector3((x * 10*scaleMaze), 0, (z * 10*scaleMaze)), Quaternion.Euler(0, 90, 0));
                }
                
            }
        }
        
    }

    public void spawnUpDownBoundaries()
    {
        for (int z = 0; z <= height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (z == 0 || z == height)
                {
                    Instantiate(prefab, new Vector3((x * 10*scaleMaze) + 5*scaleMaze, 0, (z * 10*scaleMaze) + 5*scaleMaze), Quaternion.Euler(0, 0, 0));
                }

            }
        }

    }


    // Generating edges (innner Boundaries)
    public void spawnInnerEdgesLeftRight()
    {
        EdgeIndex = 0;

        for (int z = 1; z <= height; z++)
        {
            for (int x = 1; x < width; x++)
            {
                GameObject go = (GameObject)Instantiate(prefab, new Vector3((x * 10*scaleMaze), 0, (z * 10*scaleMaze)), Quaternion.Euler(0, 90, 0));

                Edge edge = go.AddComponent<Edge>() as Edge;

               

                go.GetComponent<Edge>().tiles = new Tile[2];
                go.GetComponent<Edge>().tiles[0] = tiles[EdgeIndex];
                go.GetComponent<Edge>().tiles[1] = tiles[EdgeIndex+1];

                

                edges.Add(go.GetComponent<Edge>());

                EdgeIndex++;
                
            }
            EdgeIndex++;
        }
    }

    public void spawnInnerEdgesUpDown()
    {
        EdgeIndex = 0;

        for (int z = 1; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject go = (GameObject)Instantiate(prefab, new Vector3((x * 10*scaleMaze) + 5*scaleMaze, 0, (z * 10*scaleMaze) + 5*scaleMaze), Quaternion.Euler(0, 0, 0));

                Edge edge = go.AddComponent<Edge>() as Edge;

                go.GetComponent<Edge>().tiles = new Tile[2];
                go.GetComponent<Edge>().tiles[0] = tiles[EdgeIndex];
                go.GetComponent<Edge>().tiles[1] = tiles[EdgeIndex + 1];

                edges.Add(go.GetComponent<Edge>());

                EdgeIndex++;
                     
            }

        }
    }


    public void removeEdges()
    {
        // Get a random edge

        int randInt = Random.Range(0, edges.Count);
        orderRandom = randInt;
        Edge randomEdge = edges[randInt];

        // Remove random edge from list
        edges.RemoveAt(randInt);

        // Both null
        if (Tile.getHighestParent(randomEdge.tiles[0]) == Tile.getHighestParent(randomEdge.tiles[1]))
        {
            if (Tile.getHighestParent(randomEdge.tiles[0]) == null && Tile.getHighestParent(randomEdge.tiles[1]) == null)
            {
                randomEdge.tiles[0].parent = randomEdge.tiles[1];
                randomEdge.disableEdge();
                Debug.Log("ta null 2");

            }
        }

        else
        {
            if (Tile.getHighestParent(randomEdge.tiles[0]) == null && Tile.getHighestParent(randomEdge.tiles[1]) == null)
            {
                Tile.getHighestParent(randomEdge.tiles[0]).parent = randomEdge.tiles[1];
                randomEdge.disableEdge();
                Debug.Log("ta null 2");

            }

            else
            {
                Tile.getHighestParent(randomEdge.tiles[1]).parent = randomEdge.tiles[0];
                randomEdge.disableEdge();
            }
        }
        




    }

    public IEnumerator removeEdgeCoroutine()
    {
        int loopNum = edges.Count;

        for (int i = 0; i < loopNum; i++)
        { 
            removeEdges();
            yield return new WaitForSeconds(0f);
        }
    }

}




[System.Serializable]
public class Tile
{
   
    public Tile parent;

    public static Tile getHighestParent(Tile tile)
    {

        

        if (tile.parent == null)
        {
            return tile;
        }

        else
        {
            return getHighestParent(tile.parent);
        }

        
    }

}
