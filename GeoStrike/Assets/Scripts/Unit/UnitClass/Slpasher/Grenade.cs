using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Grenade : MonoBehaviourPun
{
    private AudioSource theAudio;

    [SerializeField] private AudioClip clip;
    [SerializeField] private Slider effect;

    // Bullet Pool
    public Queue<Grenade> myPool;

    public Transform parent;
    public Collider targetCollider;
    public Vector3 targetPos;

    public GameObject shootEffect;          // ������ ����Ʈ
    public GameObject explosionEffect;      // ���� ����Ʈ

    public float damage;
    public float ExplosionRadius = 3f;    // ���� ȿ�� ����

    float speed = 3f;   // ����ü �ӵ�
    bool onExplosion = false;

    private void Awake()
    {
        theAudio = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            onExplosion = false;
            myPool.Enqueue(this);
        }
    }

    private void Update()
    {
        theAudio.volume = effect.value;

        if (!photonView.IsMine)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // ����ź�� Ÿ����ġ �������� ����
        if (transform.position == targetPos && !onExplosion)
        {
            Explosion();
        }
    }

    [PunRPC]
    public void SetGrenadeActive(bool isTrue)
    {
        this.gameObject.SetActive(isTrue);
        if (photonView.IsMine)
        {
            photonView.RPC("SetGrenadeActive", RpcTarget.Others, isTrue);
        }
    }

    [PunRPC]
    public void SetShootActive(bool isTrue)
    {
        shootEffect.SetActive(isTrue);

        if (photonView.IsMine)
        {
            photonView.RPC("SetShootActive", RpcTarget.Others, isTrue);
        }
    }

    [PunRPC]
    public void SetExplosionActive(bool isTrue)
    {
        explosionEffect.SetActive(isTrue);

        if (photonView.IsMine)
        {
            photonView.RPC("SetExplosionActive", RpcTarget.Others, isTrue);

            theAudio.clip = clip;
            theAudio.Play();
        }
    }

    private void Explosion()
    {
        onExplosion = true;
        SetShootActive(false);
        SetExplosionActive(true);
        StartCoroutine(CActiveDelay());

        // ���߹��� �� �� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, ExplosionRadius, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        // ������ ���鿡�� ������
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
    }

    IEnumerator CActiveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(parent);
        SetGrenadeActive(false);
    }
}
