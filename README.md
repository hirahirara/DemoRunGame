# 床作る
1. Hierarchyを右クリック→3D Object→Cube を選択
2. Inspectorのtransformでscaleのxとzを100にする

![01](https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img01.png)

---
---

# Player作る
1. Hierarchyを右クリック→3D Object→Cube を選択
2. オブジェクト名とTagをPlayerに変更

![02](https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img02.png)

3. Inspectorを下にスクロールしてAdd Component選択

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img04.png">

4. Rigidbodyを選択．これでこのオブジェクトが物理的な挙動ができるようになる．これを使ってPlayerを動かす．

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img03.png">

5. Playerを動かすスクリプトを作る．一旦スクリプトを入れるフォルダとC#ファイルを作成する．ProjectのAssetsを右クリック→Create→Folderでフォルダ作成．
フォルダ名をScriptにする．

6. Scriptフォルダを右クリック→Create→C#ScriptでC#ファイル作成

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img05.png">

7. C#ファイルをダブルクリックとかで開いて編集する．
以下のように編集してPlayerのRigidbodyを取得する．
- Awake
    - このObjectが生成されるタイミングで1度だけ実行される関数
- GetComponent
    - その名の通り自分に付いてるcomponentを取得する関数
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}

```

8. 以下のように編集してPlayerを動かす部分を作る．
- Update
    - 毎フレーム呼び出される関数
- FixedUpdate
    - 一定時間毎に呼び出される関数
    - 物理的挙動を制御する場合は大体この関数で実行させる
- Input.GetAxis
    - キー入力によって-1~1の値を返す関数
    - 例えばInput.GetAxis("Horizontal")は「D」か「→」を押していると0より大きい値，「A」か「←」を押していると0未満の値，それらが押されてないと0を返す．
    - この設定はFile→BuildSetting→PlayerSetting→InputManagerで変更できる
    - 入力を受け取る方法はこれ以外にも色々あったりする
- rigidbody.velocity
    - オブジェクトの速度を表すプロパティ
    - 今回はx軸，z軸の速度を変更することで移動させている
- speed
    - Playerの速度倍率
    - privateでなくpublic修飾子になっている理由は後述

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float moveX; # 追加
    private float moveZ; # 追加
    public float speed; # 追加

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    # 以下追加
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            // 右
            moveX = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // 左
            moveX = -1;
        }
        else
        {
            moveX = 0;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            // 前
            moveZ = 1;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            // 後
            moveZ = -1;
        }
        else
        {
            moveZ = 0;
        }
    }

    private void FixedUpdate()
    {
        Vector3 move = new (moveX, 0, moveZ);
        rigidbody.velocity = move * speed;
    }
}
```

9. Unityの画面に戻り，作ったC#ScriptをHierarchyのPlayerオブジェクトにドラッグ&ドロップする．(PlayerのInspectorにドラッグ&ドロップでもいい)

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img06.png">

10. PlayerのInspectorにPlayer(Script)が追加されている．speedはpublic修飾子にしたのでInspectorで値を変更することができる．speedの値とPlayerの位置を適当に変えてテスト実行したら動くはず．

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img07.png">

テスト実行は上の矢印ボタンから

<img width="300" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img08.png">

11. 今のままだと転がっていくがRigidbodyのConstraintsのFreezeRotationのチェックボックスをオンにすると転がらなくなる

<img width="300" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img09.png">

---
---

# Playerを動的に生成する
今Playerがゲームシーンの中に存在しているが，実際のゲームではPlayerの破壊と生成が行われることがよくある(死亡からのリスポーン等)．今のままではPlayerを破壊すると消滅して二度と生成できなくなってしまうので，Playerの生成をできるようにする．そのためにPrefabを使う．

1. AssetsにPrefabフォルダを作り，Playerオブジェクトをドラッグ&ドロップでPrefabフォルダの中に入れる．

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img10.png">

2. こうするとPrefabフォルダの中に青っぽいPlayerができる(これがPrefab)．こいつはPlayerオブジェクトの情報を持っていて，オブジェクトとして生成したりできる．HierarchyのPlayerオブジェクトはいらないので消してよい(右クリック→delete)．

3. Playerを生成するスクリプトを書く．Scriptフォルダの中に新しいC#Scriptを作る．名前はとりあえずGameControllerにしておく．

4. 以下のように編集する．GameStart関数はUpdateやAwakeとかの決められたタイミングで実行されると違い普通に自分で作った関数．
- Start
    - ほぼAwakeと一緒で最初に1度だけ実行される関数
    - 今回Startを使ったがこれはAwakeでも問題ない．
    (※Startは他のオブジェクトの初期化が終わった後に実行されるが，Awakeは自分の初期化が終わればすぐ実行する．そのため，Awakeで他のオブジェクトを取得する等をしようとすると初期化前のオブジェクトを参照しようとしてエラーが出たりする．)
- Instantiate
    - prefabからオブジェクトを生成する(インスタンス化する)関数．
    - 第一引数がprefab
    - 第二引数が生成する場所(今回は座標(0,1,0)に設定)
    - 第三引数は回転(Quaternion.identityは回転していない状態を表す)
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject prefabPlayer;

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        Instantiate(prefabPlayer, new Vector3(0, 1, 0), Quaternion.identity);
    }
}

```

5. 作ったスクリプトを動かすために適当なオブジェクトにこのスクリプトを付ける．UnityのHierarchyを右クリック→CreateEmptyで空のオブジェクトを作る．名前はなんでもいいがとりあえずGameControllerにしておく．これにさっきのスクリプトをドラッグ&ドロップする．

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img11.png">

6. GameControllerのInspectorのGameController(Script)にPrefabPlayerとある(さっきのScriptでpublicにしていたのでInspectorで設定できるようになっている)．ここにPlayerのprefabをドラッグ&ドロップする．これでテスト実行するとPlayerが生成されて操作できるはず．

<img width="400" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img12.png">

---
---

# ボタンを押すと始まるようにする

今ゲート開始時にPlayerが生成されるようになっているが，ボタンを押してPlayerが生成されるようにする．そのために，ボタンを押したときにGameControllerスクリプトのGameStart関数が実行されるようにする．

1. Hierarchyを右クリック→UI→Button-TextMeshProを選択(TMP(UIのテキスト処理に使う奴)のインポートを促すウィンドウが出てくる．してもしなくてもいい)．

<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img13.png">

2. GameControllerを以下のように変更する．Startの中のGameStart実行は邪魔なので消し，GameStart関数をpublicにする(理由は後述)．

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject prefabPlayer;

    private void Start()
    {
        // GameStart(); // コメントアウト(消す)
    }

    // privete -> public
    public void GameStart()
    {
        Instantiate(prefabPlayer, new Vector3(0, 1, 0), Quaternion.identity);
    }
}
```

3. ButtonのInspectorのOnClick()の+を押す．
<img width="200" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img14.png">

4. 出てきたNone(Object)にHierarchyのGameControllerをドラッグ&ドロップし，NoFunction→GameController→GameStartを選択．
これでボタンを押せば関数GameStartが実行される(関数がpublicである必要がある)．

<img width="400" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img15.png">

5. これで実行すると多分左下にボタンがでてきて押す度にPlayerが生成されるようになる．
1回ボタンを押せばボタンが消えるようにしたい場合，以下のようにGameObject型のbuttonを格納する変数を用意し，GameStartの中でbutton.SetActive(false)を行うことでボタンが非アクティブ化し見えなくなる．後はGameControllerのInspectorのbutton変数にボタンオブジェクトをドラッグ&ドロップすれば完成．

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject prefabPlayer;
    public GameObject button; # 追加

    private void Start()
    {
        // GameStart();
    }

    public void GameStart()
    {
        Instantiate(prefabPlayer, new Vector3(0, 1, 0), Quaternion.identity);
        button.SetActive(false); # 追加
    }
}

```

<img width="400" src="https://raw.githubusercontent.com/hirahirara/DemoRunGame/images/img15.png">