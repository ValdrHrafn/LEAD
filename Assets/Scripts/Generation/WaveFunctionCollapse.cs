using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [Header("References")]
    public GameObject BossRoom;
    public List<GameObject> AllTiles = new();
    public List<GameObject> AllEndTiles = new();

    [Header("Grid Setting")]
    public int xAs;
    public int yAs;
    public int zAs;

    public float floorDis;

    public float generationSpeed;

    //Private Vars
    private Dictionary<Vector3Int, List<GameObject>> ObjectsPerTile = new();
    private List<Vector3Int> EndCapPositions = new();

    private Vector3Int spawnPoint;

    void Start() {
        spawnPoint = new Vector3Int(Random.Range(1, xAs - 1), Random.Range(0, yAs), Random.Range(1, zAs - 1));

        AllocatePositions();

        StartCoroutine(SpawnTiles());

        //for (int i = EndCapPositions.Count - 1; i > -1; i--) {
        //    GetEndCap(EndCapPositions[i]);
        //}

        //StartCoroutine(RemoveUnusedCorridors());
    }

    private void AllocatePositions() {
        for (int x = 0; x < xAs; x++)
            for (int y = 0; y < yAs; y++)
                for (int z = 0; z < zAs; z++) {
                    List<GameObject> tmplist = new();
                    foreach (var item in AllTiles) {
                        tmplist.Add(item);
                    }

                    ObjectsPerTile.Add(new Vector3Int(x, y, z), tmplist);
                    CheckForEdge(new Vector3Int(x, y, z));
                }
    }

    private IEnumerator SpawnTiles() {
        List<Vector3Int> OpenSet = new();
        List<Vector3Int> ClosedSet = new();

        ObjectsPerTile[spawnPoint].Clear();
        ObjectsPerTile[spawnPoint].Add(BossRoom);

        OpenSet.Add(spawnPoint);

        while (OpenSet.Count > 0) {
            Vector3Int currentPos = OpenSet[0];

            if(ObjectsPerTile[currentPos].Count < 1) {
                EndCapPositions.Add(currentPos);
                OpenSet.RemoveAt(0);
                ClosedSet.Add(currentPos);
                continue;
            }

            var tmp = Instantiate(ObjectsPerTile[currentPos][Random.Range(0, ObjectsPerTile[currentPos].Count)], new Vector3(currentPos.x, currentPos.y * floorDis, currentPos.z), Quaternion.identity);
            tmp.name = tmp.name + " " + currentPos.ToString();
            ObjectsPerTile[currentPos].Clear();
            ObjectsPerTile[currentPos].Add(tmp);

            var neighbours = GetNeighbours(currentPos);
            RemoveTiles(neighbours, currentPos);

            foreach (var item in neighbours) {
                if (OpenSet.Contains(currentPos + item.Key) || ClosedSet.Contains(currentPos + item.Key))
                    continue;

                OpenSet.Add(currentPos + item.Key);
            }

            OpenSet.RemoveAt(0);
            ClosedSet.Add(currentPos);

            yield return new WaitForSeconds(generationSpeed);
        }


        for (int i = EndCapPositions.Count - 1; i > -1; i--) {
            GetEndCap(EndCapPositions[i]);
        }

        StartCoroutine(RemoveUnusedCorridors());
    }

    private Dictionary<Vector3Int, List<GameObject>> GetNeighbours(Vector3Int pos) {
        Dictionary<Vector3Int, List<GameObject>> output = new();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                for (int z = -1; z <= 1; z++) {
                    if (!ObjectsPerTile.ContainsKey(pos + new Vector3Int(x, y, z)))
                        continue;

                    if (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) > 1 || Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) < 1)
                        continue;

                    output.Add(new Vector3Int(x, y, z), ObjectsPerTile[pos + new Vector3Int(x, y, z)]);
                }
            }
        }

        return output;
    } 

    public void RemoveTiles(Dictionary<Vector3Int, List<GameObject>> listPerNeighbour, Vector3Int ownPos) {
        var ownTile = ObjectsPerTile[ownPos][0].GetComponent<TileComponent>();

        foreach (var item in listPerNeighbour) {
            var neighbourList = item.Value;

            for (int i = neighbourList.Count - 1; i > -1; i--) {
                if (item.Key.x > 0) {
                    if (ownTile.xAs.x != neighbourList[i].GetComponent<TileComponent>().xAs.y) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }
                else if (item.Key.x < 0) {
                    if (ownTile.xAs.y != neighbourList[i].GetComponent<TileComponent>().xAs.x) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }

                if (item.Key.y > 0) {
                    if (ownTile.yAs.x != neighbourList[i].GetComponent<TileComponent>().yAs.y) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }
                else if (item.Key.y < 0) {
                    if (ownTile.yAs.y != neighbourList[i].GetComponent<TileComponent>().yAs.x) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }

                if (item.Key.z > 0) {
                    if (ownTile.zAs.x != neighbourList[i].GetComponent<TileComponent>().zAs.y) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }
                else if (item.Key.z < 0) {
                    if (ownTile.zAs.y != neighbourList[i].GetComponent<TileComponent>().zAs.x) {
                        neighbourList.Remove(neighbourList[i]);
                    }
                }
            }
        }
    }

    public void CheckForEdge(Vector3Int pos) {
        for (int x = -1; x <= 1; x++) {
            if (x == 0)
                continue;

            var TileList = ObjectsPerTile[pos];

            if ((pos + new Vector3Int(x, 0, 0)).x < 0) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().xAs.y != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }

            if ((pos + new Vector3Int(x, 0, 0)).x >= xAs) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().xAs.x != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }
        }

        for (int y = -1; y <= 1; y++) {
            if (y == 0)
                continue;

            var TileList = ObjectsPerTile[pos];

            if((pos + new Vector3Int(0, y, 0)).y < 0) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().yAs.y != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }

            if ((pos + new Vector3Int(0, y, 0)).y >= yAs) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().yAs.x != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }
        }

        for (int z = -1; z <= 1; z++) {
            if (z == 0)
                continue;

            var TileList = ObjectsPerTile[pos];

            if ((pos + new Vector3Int(0, 0, z)).z < 0) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().zAs.y != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }

            if ((pos + new Vector3Int(0, 0, z)).z >= zAs) {
                for (int i = TileList.Count - 1; i > -1; i--) {
                    if (TileList[i].GetComponent<TileComponent>().zAs.x != 0) {
                        TileList.Remove(TileList[i]);
                    }
                }
            }
        }
    }

    public void GetEndCap(Vector3Int pos) {
        ObjectsPerTile[pos].Clear();

        List<GameObject> tmplist = new();
        foreach (var item in AllEndTiles) {
            tmplist.Add(item);
        }

        ObjectsPerTile[pos] = tmplist;

        CheckForEdge(pos);

        for (int x = -1; x <= 1; x++) {
            if ((pos + new Vector3Int(x, 0, 0)).x < 0 || (pos + new Vector3Int(x, 0, 0)).x > xAs || x == 0)
                continue;

            if (!ObjectsPerTile.ContainsKey(pos + new Vector3Int(x, 0, 0)))
                continue;

            var neighbourTile = ObjectsPerTile[pos + new Vector3Int(x, 0, 0)][0].GetComponent<TileComponent>();

            for (int i = ObjectsPerTile[pos].Count - 1; i > -1; i--) {
                if (x < 0) {
                    if (neighbourTile.xAs.x != ObjectsPerTile[pos][i].GetComponent<TileComponent>().xAs.y) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
                else {
                    if (neighbourTile.xAs.y != ObjectsPerTile[pos][i].GetComponent<TileComponent>().xAs.x) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
            }
        }
        if (tmplist.Count < 1) {
            Debug.LogError("Cap needed for " + pos.ToString() + " does not exist");
            return;
        }

        for (int y = -1; y <= 1; y++) {
            if ((pos + new Vector3Int(0, y, 0)).y < 0 || (pos + new Vector3Int(0, y, 0)).y > yAs || y == 0)
                continue;

            if (!ObjectsPerTile.ContainsKey(pos + new Vector3Int(0, y, 0)))
                continue;

            if (EndCapPositions.Contains(pos + new Vector3Int(0, y, 0))) {
                for (int i = ObjectsPerTile[pos].Count - 1; i > -1; i--) {
                    var TileComponent = ObjectsPerTile[pos][i].GetComponent<TileComponent>();

                    if (y > 0) {
                        if (TileComponent.yAs.x > 0) {
                            ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                        }
                    }
                    else {
                        if (TileComponent.yAs.y > 0) {
                            ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                        }
                    }
                }
                continue;
            }

            var neighbourTile = ObjectsPerTile[pos + new Vector3Int(0, y, 0)][0].GetComponent<TileComponent>();

            for (int i = ObjectsPerTile[pos].Count - 1; i > -1; i--) {
                if (y < 0) {
                    if (neighbourTile.yAs.x != ObjectsPerTile[pos][i].GetComponent<TileComponent>().yAs.y) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
                else {
                    if (neighbourTile.yAs.y != ObjectsPerTile[pos][i].GetComponent<TileComponent>().yAs.x) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
            }
        }
        if (tmplist.Count < 1) {
            Debug.LogError("Cap needed for " + pos.ToString() + " does not exist");
            return;
        }

        for (int z = -1; z <= 1; z++) {
            if ((pos + new Vector3Int(0, 0, z)).z < 0 || (pos + new Vector3Int(0, 0, z)).z > zAs || z == 0)
                continue;

            if (!ObjectsPerTile.ContainsKey(pos + new Vector3Int(0, 0, z)))
                continue;

            var neighbourTile = ObjectsPerTile[pos + new Vector3Int(0, 0, z)][0].GetComponent<TileComponent>();

            for (int i = ObjectsPerTile[pos].Count - 1; i > -1; i--) {
                if (z < 0) {
                    if (neighbourTile.zAs.x != ObjectsPerTile[pos][i].GetComponent<TileComponent>().zAs.y) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
                else {
                    if (neighbourTile.zAs.y != ObjectsPerTile[pos][i].GetComponent<TileComponent>().zAs.x) {
                        ObjectsPerTile[pos].Remove(ObjectsPerTile[pos][i]);
                    }
                }
            }
        }

        if (tmplist.Count < 1) {
            Debug.LogError("Cap needed for " + pos.ToString() + " does not exist");
            return;
        }

        var tmp = Instantiate(ObjectsPerTile[pos][Random.Range(0, ObjectsPerTile[pos].Count)], new Vector3(pos.x, pos.y * floorDis, pos.z), Quaternion.identity);
        tmp.name = tmp.name + " " + pos.ToString();
        ObjectsPerTile[pos].Clear();
        ObjectsPerTile[pos].Add(tmp);

        EndCapPositions.Remove(pos);
    }

    public IEnumerator RemoveUnusedCorridors() {
        List<Vector3Int> OpenSet = new();
        List<Vector3Int> ClosedSet = new();

        OpenSet.Add(spawnPoint);

        while (OpenSet.Count > 0) {
            var currentpos = OpenSet[0];

            var owntile = ObjectsPerTile[currentpos][0].GetComponent<TileComponent>();

            if (owntile.xAs.x > 0)
                if(!OpenSet.Contains(currentpos + new Vector3Int(1, 0, 0)) && !ClosedSet.Contains(currentpos + new Vector3Int(1, 0, 0)))
                    OpenSet.Add(currentpos + new Vector3Int(1, 0, 0));
            if (owntile.xAs.y > 0)
                if (!OpenSet.Contains(currentpos + new Vector3Int(-1, 0, 0)) && !ClosedSet.Contains(currentpos + new Vector3Int(-1, 0, 0)))
                    OpenSet.Add(currentpos + new Vector3Int(-1, 0, 0));
            if (owntile.yAs.x > 0)
                if (!OpenSet.Contains(currentpos + new Vector3Int(0, 1, 0)) && !ClosedSet.Contains(currentpos + new Vector3Int(0, 1, 0)))
                    OpenSet.Add(currentpos + new Vector3Int(0, 1, 0));
            if (owntile.yAs.y > 0)
                if (!OpenSet.Contains(currentpos + new Vector3Int(0, -1, 0)) && !ClosedSet.Contains(currentpos + new Vector3Int(0, -1, 0)))
                    OpenSet.Add(currentpos + new Vector3Int(0, -1, 0));
            if (owntile.zAs.x > 0)
                if (!OpenSet.Contains(currentpos + new Vector3Int(0, 0, 1)) && !ClosedSet.Contains(currentpos + new Vector3Int(0, 0, 1)))
                    OpenSet.Add(currentpos + new Vector3Int(0, 0, 1));
            if (owntile.zAs.y > 0)
                if (!OpenSet.Contains(currentpos + new Vector3Int(0, 0, -1)) && !ClosedSet.Contains(currentpos + new Vector3Int(0, 0, -1)))
                    OpenSet.Add(currentpos + new Vector3Int(0, 0, -1));

            var tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var pos = currentpos;
            tmp.transform.localScale = new Vector3(.1f, .1f, .1f);
            tmp.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);

            OpenSet.RemoveAt(0);
            ClosedSet.Add(currentpos);

            yield return new WaitForSeconds(generationSpeed);
        }

        foreach (var item in ObjectsPerTile) {
            if (!ClosedSet.Contains(item.Key)) {
                yield return new WaitForSeconds(generationSpeed);
                Destroy(item.Value[0]);
            }
        }
    }
}