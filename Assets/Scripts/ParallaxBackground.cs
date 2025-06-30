using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam; // �����������
    [SerializeField] private float parallaxEffect; // �Ӳ�ϵ���������ƶ���������0-1

    private float xPosition; // ��������ĳ�ʼX����
    private float length;

    private void Start()
    {
        cam = GameObject.Find("Main Camera"); // ���ҳ����е��������
        
        xPosition = transform.position.x; // ��¼��������ĳ�ʼXλ��
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // ���㱳��Ӧ���ƶ��ľ��룺�������ǰXλ�� �� �Ӳ�ϵ��
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // �������ĵ��������λ��
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        // ���±���λ�ã���ʼXλ�� + �������ƫ������Y�ᱣ�ֲ���
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        // �޾��ܵ��ĸо���
        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}