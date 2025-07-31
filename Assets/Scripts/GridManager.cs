using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public GameObject cellPrefab;
    public GameObject[,] grid = new GameObject[20, 20];

    private void Awake()
    {
        Instance = this;

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                grid[x, y] = cell;

                var img = cell.GetComponent<Image>();
                img.enabled = false;

                cell.transform.GetChild(0).gameObject.SetActive(false); // sanp ka fun
                cell.transform.GetChild(1).gameObject.SetActive(false); // sanp ki punchh
                cell.transform.GetChild(2).gameObject.SetActive(false); // sanp ka khana
            }
        }
    }
}
