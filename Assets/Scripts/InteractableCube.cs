using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class InteractableCube : MonoBehaviour
{
    public enum MovementType { X_AXIS, Y_AXIS, Z_AXIS };
    public MovementType type;
    ProBuilderMesh mesh;

    void Start()
    {
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
        Vector3 moveDir = dir;
        Vector3 combine = Vector3.zero;

        if (type == MovementType.Z_AXIS)
            combine = new Vector3(transform.forward.x * moveDir.z, transform.forward.y * moveDir.z, transform.forward.z * moveDir.z);
        if (type == MovementType.X_AXIS)
            combine = new Vector3(transform.right.x * moveDir.x, transform.right.y * moveDir.x, transform.right.z * moveDir.x);
        if (type == MovementType.Y_AXIS)
            combine = new Vector3(transform.up.x * moveDir.y, transform.up.y * moveDir.y, transform.up.z * moveDir.y);

        transform.position += combine * 2f;
    }

    public void Rotate(Vector3 rotVec, float rotSpeed)
    {
        float xRot = rotVec.x * Mathf.Deg2Rad * rotSpeed;
        float yRot = rotVec.y * Mathf.Deg2Rad * rotSpeed;
        float zRot = rotVec.z * Mathf.Deg2Rad * rotSpeed;

        if (type == MovementType.Z_AXIS)
            transform.RotateAround(transform.forward, xRot);
        if (type == MovementType.X_AXIS)
            transform.RotateAround(transform.right, -xRot);
        if (type == MovementType.Y_AXIS)
            transform.RotateAround(transform.up, xRot);

        //  transform.RotateAround(Vector3.up, -xRot);
        //  transform.RotateAround(Vector3.right, yRot);
        // transform.RotateAround(transform.forward, zRot);
    }
}
