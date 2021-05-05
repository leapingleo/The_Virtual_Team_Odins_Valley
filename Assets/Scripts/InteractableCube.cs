using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class InteractableCube : MonoBehaviour
{
    public enum MovementType { X_AXIS, Y_AXIS, Z_AXIS };
    public MovementType[] types;
    public float movementPlus;
    public float movementMinus;
    ProBuilderMesh mesh;

    private MovementType type;
    private float[] minRanges;
    private float[] maxRanges;
    private bool withPlayer;

    void Start()
    {
        minRanges = new float[types.Length];
        maxRanges = new float[types.Length];

        if (types.Length == 1)
        {
            type = types[0];
        }

        withPlayer = false;

        float position = 0f;

        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] == MovementType.X_AXIS)
            {
                position = transform.position.x;
            }
            else if (types[i] == MovementType.Z_AXIS)
            {
                position = transform.position.z;
            }
            else if (types[i] == MovementType.Y_AXIS)
            {
                position = transform.position.y;
            }

            minRanges[i] = position - movementMinus;
            maxRanges[i] = position + movementPlus;

        }


        //if (type == MovementType.X_AXIS)
        //{
        //    position = transform.position.x;
        //}
        //else if (type == MovementType.Z_AXIS)
        //{
        //    position = transform.position.z;
        //}
        //else if (type == MovementType.Y_AXIS)
        //{
        //    position = transform.position.y;
        //}

        mesh = GetComponent<ProBuilderMesh>();
    }
    private void Update()
    {
        //Debug.Log(transform.right);
    }
    public void Scale(Camera camera, Vector3 direction, Vector3 contactedFaceNormal, Vector3 dir, float moveAmount, Vector3 handPos)
    {
        Vector3 heading = handPos - transform.position;
        var face = SelectionPicker.PickFace(camera, direction, mesh);

        if (face != null)
        {
            dir = dir.normalized;

            Vector3 absNormal = contactedFaceNormal;
            Vector3 combine = new Vector3(absNormal.x * dir.x,
               absNormal.y * dir.y, absNormal.z * dir.z);
            // Vector3 localPos = transform.InverseTransformDirection(combine);
            // Vector3 rot = rotatePointAroundAxis(new Vector3(0.7f, 0f, -0.7f), 45, Vector3.up);
            // Vector3 v = Vector3.forward;

            int[] indexs = new int[4];

            for (int i = 0; i < 4; i++)
            {
                indexs[i] = face.ToQuad()[i];
            }

            contactedFaceNormal = contactedFaceNormal.normalized;

            if (dir.z > 0)
                contactedFaceNormal *= -1;


            if (handPos.z > transform.position.z)
                contactedFaceNormal *= -1;

            // VertexPositioning.TranslateVertices(mesh, face.indexes, combine * moveAmount);
            VertexPositioning.TranslateVerticesInWorldSpace(mesh, indexs, contactedFaceNormal * moveAmount);
            mesh.ToMesh();
            mesh.Refresh();
            GetComponent<MeshCollider>().enabled = false;

            //  mesh.CenterPivot(mesh.faces);
            // PivotLocation.Center =;
            GetComponent<MeshCollider>().enabled = true;
            //mesh.CenterPivot(mesh.GetComponent<MeshCollider>().bounds.center);
            mesh.SetPivot(mesh.GetComponent<MeshCollider>().bounds.center);
        }
    }


    public void SetAtNewPos(Vector3 dir)
    {
        if (!withPlayer)
        {
            Vector3 moveDir = dir;
            Vector3 combine = Vector3.zero;

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == MovementType.Z_AXIS && (transform.position.z + moveDir.z >= minRanges[i] && transform.position.z + moveDir.z <= maxRanges[i]))
                {
                    combine += new Vector3(transform.forward.x * moveDir.z, transform.forward.y * moveDir.z, transform.forward.z * moveDir.z);
                }
                if (types[i] == MovementType.X_AXIS && (transform.position.x + moveDir.x >= minRanges[i] && transform.position.x + moveDir.x <= maxRanges[i]))
                    combine += new Vector3(transform.right.x * moveDir.x, transform.right.y * moveDir.x, transform.right.z * moveDir.x);
                if (types[i] == MovementType.Y_AXIS && (transform.position.y + moveDir.y >= minRanges[i] && transform.position.y + moveDir.y <= maxRanges[i]))
                    combine += new Vector3(transform.up.x * moveDir.y, transform.up.y * moveDir.y, transform.up.z * moveDir.y);
                //if (types[i] == MovementType.Z_AXIS && (transform.position.z + moveDir.z >= minRanges[i] && transform.position.z + moveDir.z <= maxRanges[i]))
                //{
                //    combine += new Vector3(0f, 0f, transform.forward.z * moveDir.z);
                //}
                //if (type == MovementType.X_AXIS && (transform.position.x + moveDir.x >= minRanges[i] && transform.position.x + moveDir.x <= maxRanges[i]))
                //    combine += new Vector3(transform.right.x * moveDir.x, 0f, 0f);
                //if (type == MovementType.Y_AXIS && (transform.position.y + moveDir.y >= minRanges[i] && transform.position.y + moveDir.y <= maxRanges[i]))
                //    combine += new Vector3(0f, transform.up.y * moveDir.y, 0f);
            }

            transform.position += combine;
            //Vector3 moveDir = dir;
            //Vector3 combine = Vector3.zero;
            //Debug.Log("z = " + transform.position.z + " min " + minRange + " max " + maxRange);
            //if (type == MovementType.Z_AXIS && (transform.position.z + moveDir.z >= minRange && transform.position.z + moveDir.z <= maxRange))
            //{
            //    combine = new Vector3(transform.forward.x * moveDir.z, transform.forward.y * moveDir.z, transform.forward.z * moveDir.z);
            //}
            //if (type == MovementType.X_AXIS && (transform.position.x + moveDir.x >= minRange && transform.position.x + moveDir.x <= maxRange))
            //    combine = new Vector3(transform.right.x * moveDir.x, transform.right.y * moveDir.x, transform.right.z * moveDir.x);
            //if (type == MovementType.Y_AXIS && (transform.position.y + moveDir.y >= minRange && transform.position.y + moveDir.y <= maxRange))
            //    combine = new Vector3(transform.up.x * moveDir.y, transform.up.y * moveDir.y, transform.up.z * moveDir.y);

            //transform.position += combine * 2f;
        }
    }
    /*
    public void Rotate(Quaternion rotVec, float rotSpeed)
    {
        if (!withPlayer)
        {
            float xRot = rotVec.x * Mathf.Deg2Rad * rotSpeed;
            float yRot = rotVec.y * Mathf.Deg2Rad * rotSpeed;
            float zRot = rotVec.z * Mathf.Deg2Rad * rotSpeed;

            //if (type == MovementType.Z_AXIS)
            //    transform.RotateAround(transform.forward, zRot);
            //if (type == MovementType.X_AXIS)
            //    transform.RotateAround(transform.right, xRot);
            //if (type == MovementType.Y_AXIS)
            //    transform.RotateAround(transform.up, yRot);

            if (type == MovementType.Y_AXIS)
            {
                transform.Rotate(new Vector3(0f, xRot, 0f));
            }
        }

        //  transform.RotateAround(Vector3.up, -xRot);
        //  transform.RotateAround(Vector3.right, yRot);
        // transform.RotateAround(transform.forward, zRot);
    }

    public void Rotate(Vector3 rotVec, float rotSpeed)
    {
        if (!withPlayer)
        {
            float xRot = rotVec.x * Mathf.Deg2Rad * rotSpeed;
            float yRot = rotVec.y * Mathf.Deg2Rad * rotSpeed;
            float zRot = rotVec.z * Mathf.Deg2Rad * rotSpeed;

            //if (type == MovementType.Z_AXIS)
            //    transform.RotateAround(transform.forward, zRot);
            //if (type == MovementType.X_AXIS)
            //    transform.RotateAround(transform.right, xRot);
            //if (type == MovementType.Y_AXIS)
            //    transform.RotateAround(transform.up, yRot);
            if (type == MovementType.X_AXIS)
            {
                transform.Rotate(new Vector3(xRot, 0f, 0f));
            }
            if (type == MovementType.Z_AXIS)
            {
                transform.Rotate(new Vector3(0f, 0f, xRot));
            }

            if (type == MovementType.Y_AXIS)
            {
                transform.Rotate(new Vector3(0f, xRot, 0f));
            }
        }

        //  transform.RotateAround(Vector3.up, -xRot);
        //  transform.RotateAround(Vector3.right, yRot);
        // transform.RotateAround(transform.forward, zRot);
    }
    */

    public void Rotate(Vector3 rotVec, float rotSpeed)
    {
        float xRot = rotVec.x * Mathf.Deg2Rad * rotSpeed;
        float yRot = rotVec.y * Mathf.Deg2Rad * rotSpeed;
        float zRot = rotVec.z * Mathf.Deg2Rad * rotSpeed;

        if (type == MovementType.Z_AXIS)
            transform.RotateAround(transform.forward, yRot);
        if (type == MovementType.X_AXIS)
            transform.RotateAround(transform.right, -yRot);
        if (type == MovementType.Y_AXIS)
            transform.RotateAround(transform.up, yRot);

        //  transform.RotateAround(Vector3.up, -xRot);
        //  transform.RotateAround(Vector3.right, yRot);
        // transform.RotateAround(transform.forward, zRot);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.tag == "GrabableNotWithPlayer" || gameObject.tag == "RotatableNotWithPlayer"))
        {
            withPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.tag == "GrabableNotWithPlayer" || gameObject.tag == "RotatableNotWithPlayer"))
        {
            withPlayer = false;
        }
    }
}
