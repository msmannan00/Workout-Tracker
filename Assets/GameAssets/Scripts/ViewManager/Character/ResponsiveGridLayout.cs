using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGridLayout : MonoBehaviour
{
    public int columns = 2; // Fixed number of columns

    void Start()
    {
        AdjustGridLayout();
    }

    void AdjustGridLayout()
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();

        // Get the screen width
        RectTransform canvas = GameObject.Find("canvas").GetComponent<RectTransform>();
        float screenWidth = canvas.rect.width;

        // Get the cell width from the GridLayoutGroup
        float cellWidth = gridLayout.cellSize.x;

        // Calculate the total width occupied by cells
        float totalCellWidth = columns * cellWidth;

        // Calculate the remaining width
        float remainingWidth = screenWidth - totalCellWidth;

        // Calculate spacing (half of the remaining width)
        float spacing = remainingWidth / 2;

        // Set padding and spacing
        float halfSpacing = spacing / 2;
        gridLayout.padding = new RectOffset((int)halfSpacing, (int)halfSpacing, gridLayout.padding.top, gridLayout.padding.bottom);
        gridLayout.spacing = new Vector2(spacing / (columns - 1), gridLayout.spacing.y);
    }

}
