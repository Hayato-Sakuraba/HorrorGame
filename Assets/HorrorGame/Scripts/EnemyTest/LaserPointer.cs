using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
	public Transform target;
	public float maxDistance = 10f;

	public LayerMask hitMask; // ← Player + Wall 両方入れる！

	private LineRenderer lr;

	void Start()
	{
		lr = GetComponent<LineRenderer>();
		lr.positionCount = 2;
	}

	public void ShowLaser(bool active)
	{
		lr.enabled = active;
	}

	void Update()
	{
		if (!lr.enabled) return;

		Vector3 origin = transform.position + transform.forward * 0.5f;
		Vector3 direction = (target.position - origin).normalized;

		RaycastHit hit;

		// 👇 ここが重要（Layer指定）
		if (Physics.Raycast(origin, direction, out hit, maxDistance, hitMask))
		{
			lr.SetPosition(0, origin);
			lr.SetPosition(1, hit.point);
		}
		else
		{
			lr.SetPosition(0, origin);
			lr.SetPosition(1, origin + direction * maxDistance);
		}
	}
}