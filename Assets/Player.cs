using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 移動スピード
    public float speed = 5;
    public float timeOut;
    private float timeElapsed;
    public playerbeam leaserprefab; // 弾のプレハブ
    public float m_shotSpeed; // 弾の移動の速さ
    public float m_shotAngleRange; // 複数の弾を発射する時の角度
    public int m_shotCount; // 弾の発射数
    public int m_hpMax; // HP の最大値
    public int m_hp; // HP
    public Explosion m_explosionPrefab; // 爆発エフェクトのプレハブ
    public AudioClip m_damageClip; // ダメージを受けた時に再生する SE

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // ゲーム開始時に呼び出される関数
    private void Awake()
    {
        m_hp = m_hpMax; // HP
    }

    // Update is called once per frame
    void Update ()
    {
        timeElapsed += Time.deltaTime;

        // 右・左
        float x = Input.GetAxisRaw ("Horizontal");
    
        // 上・下
        float y = Input.GetAxisRaw ("Vertical");
    
        // 移動する向きを求める
        Vector2 direction = new Vector2 (x, y).normalized;
    
        // 移動する向きとスピードを代入する
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        
        // プレイヤーが画面外に出ないように位置を制限する
        transform.localPosition = Utils.ClampPosition( transform.localPosition );

        if (Input.GetKey (KeyCode.Space)) {
            if (timeElapsed >= timeOut ){
                ShootNWay( 0, m_shotAngleRange, m_shotSpeed, m_shotCount );
                timeElapsed = 0;
            }
        }
    }
    // 弾を発射する関数
    private void ShootNWay( 
    float angleBase, float angleRange, float speed, int count )
    {
        Vector2 pos = GameObject.Find("2DFighterMinigun2").transform.position;
        var rot = transform.localRotation; // プレイヤーの向き

        // 弾を複数発射する場合
        if ( 1 < count )
        {
            // 発射する回数分ループする
            for ( int i = 0; i < count; ++i )
            {
                // 弾の発射角度を計算する
                var angle = angleBase + 
                    angleRange * ( ( float )i / ( count - 1 ) - 0.5f );

                // 発射する弾を生成する
                var shot = Instantiate( leaserprefab, pos, rot );

                // 弾を発射する方向と速さを設定する
                shot.Init( angle, speed );
            }
        }
        // 弾を 1 つだけ発射する場合
        else if ( count == 1 )
        {
            // 発射する弾を生成する
            var shot = Instantiate( leaserprefab, pos, rot );

            // 弾を発射する方向と速さを設定する
            shot.Init( angleBase, speed );
        }
    }
    // ダメージを受ける関数
    // 敵とぶつかった時に呼び出される
    public void Damage( int damage )
    {
        // HP を減らす
        m_hp -= damage;

        // HP がまだある場合、ここで処理を終える
        if ( 0 < m_hp ) return;

        // ダメージを受けた時の SE を再生する
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot( m_damageClip );

        // プレイヤーがいた場所に爆発エフェクトを生成する
        Instantiate( 
            m_explosionPrefab, 
            transform.localPosition, 
            Quaternion.identity );
        
        // プレイヤーが死亡したので非表示にする
        // 本来であれば、ここでゲームオーバー演出を再生したりする
        gameObject.SetActive( false );
    }
}
