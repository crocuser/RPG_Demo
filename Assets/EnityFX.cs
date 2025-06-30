using System.Collections;
using UnityEngine;

public class EnityFX : MonoBehaviour
{
    private SpriteRenderer sr; // ��Ⱦ��

    [Header("Flash FX")]
    [SerializeField] private float flashDuration; // ��˸����ʱ��
    [SerializeField] private Material hitMat; //Ч��������˸һ��
    private Material originalMat; // ԭʼ����

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // ��ȡ�������SpriteRenderer���
        originalMat = sr.material; // ��ȡ����ԭʼ����
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat; // ����Ϊ��˸����

        yield return new WaitForSeconds(flashDuration); // �ȴ�0.2��

        sr.material = originalMat; // �ָ�ԭʼ����
    }

    private void RedColorBlink()
    {
        // �����˸Ч��
        if (sr.color != Color.white)
            sr.color = Color.white; // ȷ����ɫ�ǰ�ɫ��
        else
            sr.color = Color.red; // �����ɫ�ǰ�ɫ�ģ�����Ϊ��ɫ
    }

    private void CancelRedBlink()
    {
        // ֹͣ�����˸
        CancelInvoke("RedColorBlink"); // ֹͣInvokeRepeating����
        sr.color = Color.white; // �ָ�Ϊ��ɫ
    }
}
