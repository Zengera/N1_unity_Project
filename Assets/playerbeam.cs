using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbeam : MonoBehaviour
{
    private Vector2 m_velocity; // 弾のスピード

    void OnBecameInvisible() 
    {
        Destroy (this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate (m_velocity.x, 0, 0);
    }

    // 弾を発射する時に初期化するための関数
    public void Init( float angle, float speed )
    {
        // 弾の発射角度をベクトルに変換する
        var direction = Utils.GetDirection( angle );

        // 発射角度と速さから速度を求める
        m_velocity = direction * speed;

        // 弾が進行方向を向くようにする
        var angles = transform.localEulerAngles;
        angles.z += angle;
        transform.localEulerAngles = angles;

    }

}
