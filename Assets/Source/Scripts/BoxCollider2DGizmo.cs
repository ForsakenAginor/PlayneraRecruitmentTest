using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxCollider2DGizmo : MonoBehaviour
{
    private BoxCollider2D _boxCollider;

    private void OnDrawGizmos()
    {
        if (_boxCollider == null)
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        if (_boxCollider != null)
        {
            Gizmos.color = Color.red;

            Vector2 position = _boxCollider.bounds.center;
            Vector2 size = _boxCollider.size;

            Gizmos.DrawWireCube(position, size);
        }
    }
}
