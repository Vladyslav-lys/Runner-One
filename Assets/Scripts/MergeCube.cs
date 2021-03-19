using UnityEngine;

public class MergeCube : NumberCube
{
    public Transform targetTransform;
    public MergeCube createdMergeCube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            if (createdMergeCube && !createdMergeCube.gameObject.activeSelf)
            {
                createdMergeCube.value = value * 2;
                createdMergeCube.pow = pow + 1;
                createdMergeCube.colorIndex = (createdMergeCube.pow - 1) % gm.colors.Count;
                createdMergeCube.valueText.text = createdMergeCube.value.ToString();
                createdMergeCube.meshRenderer.material.color = gm.colors[createdMergeCube.colorIndex];
                createdMergeCube.gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(targetTransform)
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, 5f * Time.deltaTime);
    }
}
