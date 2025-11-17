了解！
このゲーム、**2週間で完成させるための「完全ロードマップ」**と、**実際に作るときのミニマム構成（MVP）**をまとめるね。
要するに **最短で「遊べる形」まで持っていくための戦略**です。

---

# 🎯 **2週間で完成させるための最短制作プラン（MVP設計）**

### **まず決めるべき方針**

* 「Unfinished Swan のように色を付けて可視化」 → **最小限の shader + decal + particle でOK**
* 透明ドア＆幽霊 → **コライダーは常にそのまま／可視化は特殊マテリアルでシルエットだけ**
* 遺跡は広すぎない → **1フロア or 小さめ迷路**
* 10分タイマー → **UI + State Management**
* 幽霊は倒せない → **Slow / Freeze のみ**

完璧な仕掛け・UI・アニメ・VFXは後回し。
まずは「ゲームとして最小限ループが成立するか」。

---

# 🧱 **Week 1（基盤づくり）**

---

## **Day 1 – Core Movement + Camera**

* FPS Controller（Unity Starter Assets or your own）
* シンプルな遺跡のブロックアウト（Cubeだけ）
* プレイヤーの体力バー（仮UI）

---

## **Day 2 – Paintball システム（可視化）**

### 必須要素：

* Paintball を投げる
* Raycast Hit → そこに **Color decal**（丸いテクスチャ）を貼る
* 「見えない物体」には透明マテリアル（Alpha=0.05）
  → ペイントが当たった場所だけ **Alpha が上がる or テクスチャが貼られる**

**最初はこれだけ！**
複雑なスプラッシュ表現は後に追加。

---

## **Day 3 – Invisible Door 実装**

* 大きめの Box Collider（ドアの形）
* 見た目は完全透明
* Paintball が当たるとその部分だけ白く / カラー化して見える
* ドア内に入ると「Goal area」にイベント発火

---

## **Day 4 – Ghost AI（パート1：巡回）**

まずは **透明の Capsule** でいい。

* NavMeshAgent でランダム巡回
* 透明マテリアル（Alpha=0）
* Paintball が当たると 1 秒間だけ可視化される
* その瞬間からプレイヤーを追跡開始

---

## **Day 5 – Ghost AI（パート2：攻撃 & プレイヤーダメージ）**

* プレイヤーに近づく → HP 減る
* 追跡中は透明のままでOK（MVP）
* Slow / Freeze の仕組み（Silver bullet / Axe）
  → GhostAgent.speed = speed * 0.3
  → Freeze 時は NavMesh 停止

ここでゲームとして「遊べる」状態になる。

---

# 🗝 **Week 2（最終週）**

---

## **Day 6 – Time System（10分間）**

* UI Timer
* 時間切れ → Game Over

---

## **Day 7 – Sound & Basic Atmosphere**

* 足音
* 遺跡の環境音
* Ghostの音（スー…というホラー的なやつ）

まだ仮でOK。

---

## **Day 8 – Goal Room & Traps（仕掛けの最小版）**

「宝物部屋に仕掛けを入れる」と言っていたので、MVPは：

* 足元に落とし穴（穴に落ちるとHP減る）
* 罠の槍がゆっくり出る（アニメ不要、位置移動だけ）

簡単でOK。

---

## **Day 9 – Visual Improvement**

* Paint splash の簡単なパーティクル
* 部分的 Decal Shader を強化（テクスチャ変更）
* Ghost に簡易モデル（白いシルエット）

---

## **Day 10 – Menu / Retry / Result**

* Start
* Retry
* Win / Lose UI

---

## **Day 11 – Optimization + Bug Fix**

* NavMeshの調整
* ペイントが大量になる処理負荷対応
  （古いDecalを消す／プールする）
* 当たり判定の隙間修正

---

## **Day 12 – Small Polish**

* Ghost の気配サウンド
* 雰囲気ライト追加（ライト1〜2個だけでOK）

---

## **Day 13 – Playtest & Balance**

* 幽霊の数調整
* タイマー調整
* ペイント弾の速度・大きさ調整

---

## **Day 14 – 完成 & 発表ビルド**

---

# 🎮 **ゲームループの完成形（MVP）**

1. 遺跡に入る（時間スタート）
2. 見えないドアと幽霊がある
   　→ ペイントで「うっすら見える」
3. 幽霊に色を付けると追ってくる
   　→ 逃げる or 耐える or Slow
4. 10分以内にドアを見つけて開ける
5. 仕掛けのある最後の部屋へ
6. 宝物を取ったらクリア

完全に成立する！

---

# 👍 **2週間で十分作れる理由**

* 高度な3Dモデル不要（全部透明でも成立）
* Unfinished Swan のようなペイントは 「Decal + Alpha コントロール」で簡単
* Ghost AI もシンプルな NavMesh でOK
* 必要なアニメーションもほぼ不要

**世界観の力 + ギミックの面白さ** で勝負できるタイプのゲーム！

---

# もし必要なら…

どれでもすぐ作ってあげます：

✅ ペイント当たり用の Unity 6 Shader
✅ Ghost の AI スクリプト（巡回・追跡・当たり判定・可視化）
✅ Invisible door の可視化スクリプト
✅ 全体のステート管理（時間・死亡・クリア）
✅ 最速プロトタイプ構築セット

---

必要な部分ある？
どこから作る？
