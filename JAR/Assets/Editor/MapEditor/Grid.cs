using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public float tileWidth = 1f;
    public float tileHeight = 1f;

    public Transform tilePrefab;
    public TileSet tileSet;

    private float gizmoMaxWidth = 2400.0f;
    private float gizmoMaxHeight = 1600.0f;

    // grid color
    public Color color = Color.white;

    public void OnDrawGizmos()
    {
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = this.color;

        // Draw horizontal gizmo lines
        if (this.tileHeight > 0f)
        {
            for (float y = pos.y - this.gizmoMaxHeight / 2f; y < pos.y + this.gizmoMaxHeight / 2; y += this.tileHeight)
            {
                Gizmos.DrawLine(new Vector3(-this.gizmoMaxWidth, Mathf.Floor(y / this.tileHeight) * this.tileHeight, 0.0f),
                                new Vector3(this.gizmoMaxWidth, Mathf.Floor(y / this.tileHeight) * this.tileHeight, 0.0f));
            }
        }

        // Draw vertical gizmo lines
        if(this.tileWidth > 0f)
        {
            for(float x = pos.x - this.gizmoMaxWidth / 2f; x < pos.x + this.gizmoMaxWidth / 2; x += this.tileWidth)
            {
                Gizmos.DrawLine(new Vector3(Mathf.Floor(x / this.tileWidth) * this.tileWidth, -this.gizmoMaxHeight, 0.0f),
                                new Vector3(Mathf.Floor(x / this.tileWidth) * this.tileWidth, this.gizmoMaxHeight, 0.0f));
            }
        }


    }
}
