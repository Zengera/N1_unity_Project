using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵を制御するコンポーネント
public class Enemy : MonoBehaviour
{
    public Vector2 m_respawnPosInside; // 敵の出現位置（内側）
    public Vector2 m_respawnPosOutside; // 敵の出現位置（外側）
    public float m_speed; // 移動する速さ
    public int m_hpMax; // HP の最大値
    public int m_exp; // この敵を倒した時に獲得できる経験値
    public int m_damage; // この敵がプレイヤーに与えるダメージ
    public Explosion m_explosionPrefab; // 爆発エフェクトのプレハブ
    public AudioClip m_damageClip; // ダメージを受けた時に再生する SE

    private int m_hp; // HP
    private Vector3 m_direction; // 進行方向
    //弾のプレハブオブジェクト
    public Enemybeam leaserprefab;
    public float m_shotSpeed; // 弾の移動の速さ
    public int m_shotCount; // 弾の発射数ss

    //一定時間ごとに弾を発射するためのもの
    private float targetTime = 2.5f;
    private float currentTime = 0;


    // 敵が生成された時に呼び出される関数
    private void Start()
    {
        // HP を初期化する
        m_hp = m_hpMax;

        var pos = Vector3.zero;

        // 出現位置と進行方向を決定する
        pos.x = Random.Range( m_respawnPosOutside.x, m_respawnPosInside.x );
        pos.y = Random.Range( m_respawnPosOutside.y, m_respawnPosInside.y );
        m_direction = Vector2.left;

        // 位置を反映する
        transform.localPosition = pos;
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // まっすぐ移動する
        transform.localPosition += m_direction * m_speed;


        //一秒経つごとに弾を発射する
        currentTime += Time.deltaTime;
        if (targetTime<currentTime) {
            currentTime = 0;
            //敵からプレイヤーに向かうベクトルをつくる
            GameObject Player = GameObject.Find("Player");
            if (Player != null){
                //プレイヤーの位置から敵の位置（弾の位置）を引く
                Vector3 PlayerPos = Player.transform.position;
                Vector3 pos = this.gameObject.transform.position;
                Vector3 vec = PlayerPos- pos;
                float angle = Mathf.Atan2(vec.y,vec.x) * Mathf.Rad2Deg;
                //弾のRigidBody2Dコンポネントのvelocityに先程求めたベクトルを入れて力を加える
                ShootNWay( angle, 30f, m_shotSpeed, m_shotCount );
            }
        }
        
        if(transform.localPosition.x <= -12){
            Destroy (this.gameObject);
        }
    }

    // 他のオブジェクトと衝突した時に呼び出される関数
    private void OnTriggerEnter2D( Collider2D collision )
    {

        // プレイヤーと衝突した場合
        if ( collision.name.Contains( "Player" ) )
        {
            // プレイヤーにダメージを与える
            var player = collision.GetComponent<Player>();
            player.Damage( m_damage );
            return;
        }

        // 弾と衝突した場合
        if ( collision.name.Contains( "playerbeam" ) )
        {

            // ダメージを受けた時の SE を再生する
            var audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot( m_damageClip );

            // 弾を削除する
            Destroy( collision.gameObject );

            // 敵の HP を減らす
            m_hp--;

            // 敵の HP がまだ残っている場合はここで処理を終える
            if ( 0 < m_hp ) return;

            // 弾が当たった場所に爆発エフェクトを生成する
            Instantiate( 
                m_explosionPrefab, 
                collision.transform.localPosition, 
                Quaternion.identity );
            
                GameObject EnemyManager = GameObject.Find("EnemyManager");
                if (EnemyManager != null){
                    EnemyManager.SendMessage("ScoreDisplay");
                }

            // 敵を削除する
            Destroy( gameObject );
        }
    }
    
    // 弾を発射する関数
    private void ShootNWay( 
    float angleBase, float angleRange, float speed, int count )
    {
        Vector2 pos = this.gameObject.transform.position;
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
}
