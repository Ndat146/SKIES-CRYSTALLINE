using UnityEngine;

public class CubePart : MonoBehaviour
{
    [SerializeField] private CubeMove cubeMove;

    public bool IsSupported()
    {
        if (cubeMove == null) return false;

        Vector3 origin = cubeMove.IsUpright()
            ? cubeMove.transform.position
            : transform.position;

        origin += Vector3.down * 0.51f; 
        float checkRadius = 0.45f;

        Collider[] hits = Physics.OverlapSphere(origin, checkRadius, LayerMask.GetMask("Ground"));

        Debug.DrawRay(origin + Vector3.up * 0.5f, Vector3.down * 1f, hits.Length > 0 ? Color.green : Color.red, 1f);

        return hits.Length > 0;
    }
}
