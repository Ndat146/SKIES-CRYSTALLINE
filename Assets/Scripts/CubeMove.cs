using UnityEngine;
using System.Collections;

public class CubeMove : MonoBehaviour
{
    public int speed = 300;
    private bool isMoving = false;

    enum Orientation
    {
        Upright,
        LayingX,
        LayingZ
    }

    Orientation currentState = Orientation.Upright;

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Roll(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Roll(Vector3.back));
        }
    }

    IEnumerator Roll(Vector3 dir)
    {
        isMoving = true;

        // Nếu đang đứng, dịch nửa ô để chuẩn bị nằm
        if (currentState == Orientation.Upright)
        {
            transform.position += dir * 0.5f;
        }

        // Nếu đang nằm và sẽ đứng dậy → dịch thêm 0.5 để đảm bảo đứng tại ô tiếp theo
        if ((currentState == Orientation.LayingX && (dir == Vector3.right || dir == Vector3.left)) ||
            (currentState == Orientation.LayingZ && (dir == Vector3.forward || dir == Vector3.back)))
        {
            transform.position += dir * 0.5f;
        }

        Vector3 anchor = GetRotationAnchor(dir);
        Vector3 axis = Vector3.Cross(Vector3.up, dir);

        float remainingAngle = 90f;

        while (remainingAngle > 0)
        {
            float angle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(anchor, axis, angle);
            remainingAngle -= angle;
            yield return null;
        }


        UpdateState(dir);
        SnapToGrid();
        isMoving = false;
    }

    Vector3 GetRotationAnchor(Vector3 dir)
    {
        Vector3 pos = transform.position;

        switch (currentState)
        {
            case Orientation.Upright:
                return pos + (dir + Vector3.down) * 0.5f;

            case Orientation.LayingX:
                if (dir == Vector3.forward || dir == Vector3.back)
                {
                    return pos + (dir + Vector3.down) * 0.5f;
                }
                else if (dir == Vector3.right)
                {
                    // Xoay lên từ nằm sang đứng, dùng mép bên phải làm tâm (phía trước box)
                    return pos + new Vector3(0.5f, -0.5f, 0f); 
                }
                else if (dir == Vector3.left)
                {                   
                    return pos + new Vector3(-0.5f, -0.5f, 0f);
                }
                break;

            case Orientation.LayingZ:
                if (dir == Vector3.left || dir == Vector3.right)
                {
                    return pos + (dir + Vector3.down) * 0.5f;
                }
                else if (dir == Vector3.forward)
                {
                    return pos + new Vector3(0f, -0.5f, 0.5f);
                }
                else if (dir == Vector3.back)
                {
                    return pos + new Vector3(0f, -0.5f, -0.5f);
                }
                break;
        }

        return pos;
    }

    void UpdateState(Vector3 dir)
    {
        if (currentState == Orientation.Upright)
        {
            if (dir == Vector3.right || dir == Vector3.left)
                currentState = Orientation.LayingX;
            else if (dir == Vector3.forward || dir == Vector3.back)
                currentState = Orientation.LayingZ;
        }
        else if (currentState == Orientation.LayingX)
        {
            if (dir == Vector3.right || dir == Vector3.left)
                currentState = Orientation.Upright;
        }
        else if (currentState == Orientation.LayingZ)
        {
            if (dir == Vector3.forward || dir == Vector3.back)
                currentState = Orientation.Upright;
        }
        Debug.Log("Updated orientation: " + currentState);
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;

        float groundY = -0.5f;

        if (currentState == Orientation.Upright)
        {
            pos.x = Mathf.Round(pos.x);
            pos.z = Mathf.Round(pos.z);
            pos.y = groundY + 1f;
        }
        else if (currentState == Orientation.LayingX)
        {
            pos.x = Mathf.Floor(pos.x) + 0.5f; // tâm giữa 2 ô X
            pos.z = Mathf.Round(pos.z);
            pos.y = groundY + 0.5f;
        }
        else if (currentState == Orientation.LayingZ)
        {
            pos.x = Mathf.Round(pos.x);
            pos.z = Mathf.Floor(pos.z) + 0.5f; // tâm giữa 2 ô Z
            pos.y = groundY + 0.5f;
        }

        transform.position = pos;

        // Snap rotation về bội số 90 độ
        transform.rotation = Quaternion.Euler(
            Mathf.Round(transform.eulerAngles.x / 90f) * 90f,
            Mathf.Round(transform.eulerAngles.y / 90f) * 90f,
            Mathf.Round(transform.eulerAngles.z / 90f) * 90f
        );
        //if (currentState == Orientation.Upright)
        //{
        //    Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        //    foreach (var col in colliders)
        //    {
        //        if (col.CompareTag("Goal"))
        //        {
        //            GameManager.Instance.WinGame(transform);
        //        }
        //    }
        //}
        if (currentState == Orientation.Upright)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f);
            foreach (var col in colliders)
            {
                Debug.Log("Found collider: " + col.name + " | Tag: " + col.tag);
                if (col.CompareTag("Goal"))
                {
                    Debug.Log("YOU WIN!");
                    GameManager.Instance.WinGame(transform);
                }
            }
        }

    }
    public bool IsUpright()
    {
        return currentState == Orientation.Upright;
    }
    public string GetCurrentOrientation()
    {
        return currentState.ToString();
    }

}
