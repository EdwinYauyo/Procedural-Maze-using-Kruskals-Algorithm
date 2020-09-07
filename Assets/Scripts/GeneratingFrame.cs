using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratingFrame : MonoBehaviour
{
    public float delayTime=0.1f;
    public GameObject prefab;
    public float scaleMaze;
    [Range(1,80)]
    public int width;

    [Range(1, 80)]
    public int height;

    [HideInInspector] public Tile[] tiles;

    public List<Edge> edges;

    [HideInInspector] public int EdgeIndex = 0;
    [SerializeField] private int orderRandom = 0;
    public bool active;

    public void Execution(float t)
    {
        prefab.transform.localScale = new Vector3(10 * scaleMaze, 4 * scaleMaze, 1.5f * scaleMaze);

        tiles = new Tile[width * height];

        for (int i = 0; i < width * height; i++)
        {
            tiles[i] = new Tile();
        }

        edges = new List<Edge>();
        StartCoroutine(ExecutionCoroutine(t));
    }

    IEnumerator ExecutionCoroutine(float t)
    {
        yield return new WaitForSeconds(t);
        SpawnLeftRightBoundaries();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        SpawnUpDownBoundaries();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        SpawnInnerEdgesLeftRight();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        SpawnInnerEdgesUpDown();
        yield return new WaitForSeconds(t);
        Debug.Log("next");
        if(active)StartCoroutine(RemoveEdgeCoroutine());
        
        
    }

    // Generating frame (outter boundaries)
    public void SpawnLeftRightBoundaries()
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

    public void SpawnUpDownBoundaries()
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
    public void SpawnInnerEdgesLeftRight()
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

    public void SpawnInnerEdgesUpDown()
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
                go.GetComponent<Edge>().tiles[1] = tiles[EdgeIndex + width];

                edges.Add(go.GetComponent<Edge>());

                EdgeIndex++;
                     
            }

        }
    }


    public void RemoveEdges()
    {
        // Get a random edge

        int randInt = Random.Range(0, edges.Count);
        orderRandom = randInt;
        Edge randomEdge = edges[randInt];

        // Remove random edge from list
        edges.RemoveAt(randInt);

        // Both null
        if (Tile.GetHighestParent(randomEdge.tiles[0]) == Tile.GetHighestParent(randomEdge.tiles[1]))
        {
            if (Tile.GetHighestParent(randomEdge.tiles[0]) == null && Tile.GetHighestParent(randomEdge.tiles[1]) == null)
            {
                randomEdge.tiles[0].parent = randomEdge.tiles[1];
                randomEdge.DisableEdge();
                Debug.Log("ta null 2");

            }
        }

        else
        {
            if (Tile.GetHighestParent(randomEdge.tiles[0]) == null && Tile.GetHighestParent(randomEdge.tiles[1]) == null)
            {
                Tile.GetHighestParent(randomEdge.tiles[0]).parent = randomEdge.tiles[1];
                randomEdge.DisableEdge();
                Debug.Log("ta null 2");

            }

            else
            {
                Tile.GetHighestParent(randomEdge.tiles[1]).parent = randomEdge.tiles[0];
                randomEdge.DisableEdge();
            }
        }
        




    }

    public IEnumerator RemoveEdgeCoroutine()
    {
        int loopNum = edges.Count;

        for (int i = 0; i < loopNum; i++)
        { 
            if(edges.Count>0)RemoveEdges();
            yield return new WaitForSeconds(delayTime);
        }
    }

}




[System.Serializable]
public class Tile
{
   
    public Tile parent;

    public static Tile GetHighestParent(Tile tile)
    {

        

        if (tile.parent == null)
        {
            return tile;
        }

        else
        {
            return GetHighestParent(tile.parent);
        }

        
    }

}
