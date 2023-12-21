using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAtMouse : MonoBehaviour
{
    public bool isDrawing = false;
    [SerializeField] Color mainColor = new Color(0f, 1f, 0f, 1f);

    private void OnPostRender()
    {
        if (!isDrawing) return;

        Vector3 mousePosition = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped());

        CreateLineMaterial();
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(mainColor);

        float startX = mousePosition.x - 1.5f;
        float startY = mousePosition.y;
        float startZ = mousePosition.z - 1.5f;

        //X axis lines
        for (float i = 0; i <= 3; i += 1)
        {
            GL.Vertex3(startX, startY, startZ + i);
            GL.Vertex3(startX + 3, startY, startZ + i);
        }

        //Z axis lines
        for (float i = 0; i <= 3; i += 1)
        {
            GL.Vertex3(startX + i, startY, startZ);
            GL.Vertex3(startX + i, startY, startZ + 3);
        }
        GL.End();
    }

    private Material lineMaterial;

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
}
