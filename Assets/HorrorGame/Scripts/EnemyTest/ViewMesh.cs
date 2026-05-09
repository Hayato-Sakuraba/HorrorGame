using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ViewMesh : MonoBehaviour
{
	public float viewAngle = 60f;
	public float viewDistance = 7f;
	public int resolution = 30;

	private Mesh mesh;

	void Start()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	void LateUpdate()
	{
		DrawFieldOfView();
	}

	public LayerMask obstacleMask; // 壁レイヤー

	void DrawFieldOfView()
	{
		int stepCount = resolution;
		float stepAngleSize = (viewAngle * 2) / stepCount;

		Vector3[] vertices = new Vector3[stepCount + 2];
		int[] triangles = new int[stepCount * 3];

		Vector3 origin = transform.position + Vector3.up * 0.5f;

		// ★これ重要
		vertices[0] = Vector3.zero;

		for (int i = 0; i <= stepCount; i++)
		{
			float angle = -viewAngle + stepAngleSize * i;
			Vector3 dir = DirFromAngle(angle);

			RaycastHit hit;

			if (Physics.Raycast(origin, dir, out hit, viewDistance, obstacleMask))
			{
				vertices[i + 1] = transform.InverseTransformPoint(hit.point);
			}
			else
			{
				vertices[i + 1] = transform.InverseTransformPoint(origin + dir * viewDistance);
			}

			Debug.DrawRay(origin, dir * viewDistance, Color.red);
		}

		int triIndex = 0;

		for (int i = 0; i < stepCount; i++)
		{
			triangles[triIndex] = 0;
			triangles[triIndex + 1] = i + 1;
			triangles[triIndex + 2] = i + 2;
			triIndex += 3;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
	Vector3 DirFromAngle(float angle)
	{
		float rad = angle * Mathf.Deg2Rad;
		Vector3 dir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));

		// ★ここが超重要
		return transform.TransformDirection(dir);
	}
}