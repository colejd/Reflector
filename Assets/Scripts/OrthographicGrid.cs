using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Quick and dirty implementation of a GL grid drawer for orthographic cameras.
/// To use, put this script on a camera.
/// </summary>
public class OrthographicGrid : MonoBehaviour {
    Material mat;
    Camera cam;
    public float thickness = 0.02f;
    public Color lineColor = Color.red;

    void Start() {
        mat = new Material(Shader.Find("Sprites/Default"));
        cam = Camera.main;
    }

    void OnPostRender() {

        Vector2 origin = new Vector2(transform.position.x, transform.position.z);

        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
    
        GL.Color(lineColor);

        // Half the actual value
        float viewportHeightInWorldUnits = cam.orthographicSize;
        // Half the actual value
        float viewportWidthInWorldUnits = cam.aspect * viewportHeightInWorldUnits;

        // Draw all the vertical lines that will fit on the screen
        int widthRangeStart = (int)Mathf.Floor(origin.x - viewportWidthInWorldUnits);
        int widthRangeEnd = (int)Mathf.Ceil(origin.x + viewportWidthInWorldUnits);
        for(int x = widthRangeStart; x <= widthRangeEnd; x++) {
            QuadLine(new Vector2(x, origin.y + viewportHeightInWorldUnits),
                            new Vector2(x, origin.y - viewportHeightInWorldUnits), thickness, 0f);
        }

        // Draw all the horizontal lines that will fit on the screen
        int heightRangeStart = (int)Mathf.Floor(origin.y - viewportWidthInWorldUnits);
        int heightRangeEnd = (int)Mathf.Ceil(origin.y + viewportWidthInWorldUnits);
        for(int y = widthRangeStart; y <= widthRangeEnd; y++) {
            QuadLine(new Vector2(origin.x + viewportWidthInWorldUnits, y),
                            new Vector2(origin.x - viewportWidthInWorldUnits, y), 0f, thickness);
        }

        GL.End();
        GL.PopMatrix();


    }

    /// <summary>
    /// Draws a vertical or horizontal line in GL. Not good for any other kind of
    /// line, since this code really just draws a quad with some basic offsets for
    /// thickening the "line".
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="xThickness"> Added thickness along the X dimension </param>
    /// <param name="yThickness"> Added thickness along the Y dimension </param>
    void QuadLine(Vector2 p1, Vector2 p2, float xThickness, float yThickness) {
        Vector2 p1Viewport = cam.WorldToViewportPoint(new Vector3(p1.x, 0f, p1.y));
        Vector2 p2Viewport = cam.WorldToViewportPoint(new Vector3(p2.x, 0f, p2.y));

        float viewportHeight = cam.orthographicSize * 2.0f;
        float viewportWidth = cam.aspect * viewportHeight;

        float halfLineViewportX = (xThickness / viewportWidth) / 2.0f;
        float halfLineViewportY = (yThickness / viewportHeight) / 2.0f;

        GL.Vertex3(p1Viewport.x - halfLineViewportX, p1Viewport.y - halfLineViewportY, 0);
        GL.Vertex3(p2Viewport.x - halfLineViewportX, p2Viewport.y - halfLineViewportY, 0);
        GL.Vertex3(p2Viewport.x + halfLineViewportX, p2Viewport.y + halfLineViewportY, 0);
        GL.Vertex3(p1Viewport.x + halfLineViewportX, p1Viewport.y + halfLineViewportY, 0);

    }

    /// <summary>
    /// Draws a vertical line in GL. It's actually a quad, though (for thickness).
    /// </summary>
    void VerticalLine(Vector2 p1, Vector2 p2) {
        Vector2 p1Viewport = cam.WorldToViewportPoint(new Vector3(p1.x, 0f, p1.y));
        Vector2 p2Viewport = cam.WorldToViewportPoint(new Vector3(p2.x, 0f, p2.y));

        float viewportHeight = cam.orthographicSize * 2.0f;
        float viewportWidth = cam.aspect * viewportHeight;

        float halfLineViewport = thickness / viewportWidth;

        float halfLine = halfLineViewport / 2.0f;
        GL.Vertex3(p1Viewport.x - halfLine, p1Viewport.y, 0);
        GL.Vertex3(p2Viewport.x - halfLine, p2Viewport.y, 0);
        GL.Vertex3(p2Viewport.x + halfLine, p2Viewport.y, 0);
        GL.Vertex3(p1Viewport.x + halfLine, p1Viewport.y, 0);

    }

    /// <summary>
    /// Draws a horizontal line in GL. It's actually a quad, though (for thickness).
    /// </summary>
    void HorizontalLine(Vector2 p1, Vector2 p2) {
        Vector2 p1Viewport = cam.WorldToViewportPoint(new Vector3(p1.x, 0f, p1.y));
        Vector2 p2Viewport = cam.WorldToViewportPoint(new Vector3(p2.x, 0f, p2.y));

        float viewportHeight = cam.orthographicSize * 2.0f;

        float halfLineViewport = thickness / viewportHeight;

        float halfLine = halfLineViewport / 2.0f;
        GL.Vertex3(p1Viewport.x, p1Viewport.y  - halfLine, 0);
        GL.Vertex3(p2Viewport.x, p2Viewport.y  - halfLine, 0);
        GL.Vertex3(p2Viewport.x, p2Viewport.y  + halfLine, 0);
        GL.Vertex3(p1Viewport.x, p1Viewport.y  + halfLine, 0);

    }


}