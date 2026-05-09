using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ViewMash2D : MonoBehaviour
{
	[Header("視界設定")]
	public float viewAngle = 60f;      // 全角度
	public float viewDistance = 7f;
	public int resolution = 30;

	public LayerMask obstacleMask;

	private Mesh mesh;

	void Start()
	{
		mesh = new Mesh();
		mesh.name = "ViewMesh";

		GetComponent<MeshFilter>().mesh = mesh;
	}

	void LateUpdate()
	{
		DrawViewMesh();
	}

	void DrawViewMesh()
	{
		mesh.Clear();

		int stepCount = resolution;

		float stepAngle = viewAngle / stepCount;

		Vector3[] vertices = new Vector3[stepCount + 2];
		int[] triangles = new int[stepCount * 3];

		// 中心点
		vertices[0] = Vector3.zero;

		for (int i = 0; i <= stepCount; i++)
		{
			// 左右30°ずつ（viewAngle=60なら）
			float angle = -viewAngle / 2f + stepAngle * i;

			// 敵の正面(transform.up)基準
			Vector3 dir =
				Quaternion.Euler(0, 0, angle) * transform.up;

			Vector3 worldPoint;

			RaycastHit hit;

			if (Physics.Raycast(
				transform.position,
				dir,
				out hit,
				viewDistance,
				obstacleMask))
			{
				worldPoint = hit.point;
			}
			else
			{
				worldPoint =
					transform.position + dir * viewDistance;
			}

			// 超重要
			// ワールド→ローカル変換
			vertices[i + 1] =
				transform.InverseTransformPoint(worldPoint);
		}

		// 三角形生成
		int triIndex = 0;

		for (int i = 0; i < stepCount; i++)
		{
			triangles[triIndex] = 0;
			triangles[triIndex + 1] = i + 1;
			triangles[triIndex + 2] = i + 2;

			triIndex += 3;
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
	}
}