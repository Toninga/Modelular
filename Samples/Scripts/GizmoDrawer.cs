using System.Linq;
using UnityEditor;
using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    [Range(0f, 2f)]
    public float Size = 1f;
    public enum DisplayMode { Always, SelectedOnly, NotSelectedOnly }
    public DisplayMode ShowWhen;
    public bool DrawMoveGizmo = true;
    public bool DrawRotationGizmo = false;
    public bool DrawScaleGizmo = false;
    
    private void OnDrawGizmos()
    {
        if (ShowWhen == DisplayMode.Always)
            Draw();

        if (ShowWhen == DisplayMode.NotSelectedOnly)
            if (!Selection.gameObjects.Contains(gameObject))
                Draw();

    }
    private void OnDrawGizmosSelected()
    {
        if (ShowWhen != DisplayMode.SelectedOnly)
            return;

        Draw();
    }

    void Draw()
    {
        if (DrawMoveGizmo)
            MoveGizmo();
        if (DrawRotationGizmo)
            RotationGizmo();
        if (DrawScaleGizmo)
            ScaleGizmo();
    }
    void MoveGizmo()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(Vector3.right, Vector3.up), Size, EventType.Repaint);
        Handles.color = Color.green;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.back), Size, EventType.Repaint);
        Handles.color = Color.blue;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.identity, Size, EventType.Repaint);
    }
    void RotationGizmo()
    {
        Handles.color = Color.red;
        Handles.CircleHandleCap(0, transform.position, Quaternion.LookRotation(Vector3.right, Vector3.up), Size, EventType.Repaint);
        Handles.color = Color.green;
        Handles.CircleHandleCap(0, transform.position, Quaternion.LookRotation(Vector3.up, Vector3.back), Size, EventType.Repaint);
        Handles.color = Color.blue;
        Handles.CircleHandleCap(0, transform.position, Quaternion.identity, Size, EventType.Repaint);
    }
    void ScaleGizmo()
    {
        Handles.color = Color.red;
        Handles.ScaleHandle(Vector3.one * Size, transform.position, Quaternion.identity);
        
    }
}
