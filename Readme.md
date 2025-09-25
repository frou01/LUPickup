# RigidBody Udon Railway
## 概要
LateUpdatePickupは専ら車両内での使用を前提にLateUpdateと相対位置を用いて位置更新・同期を行うピックアップオブジェクトを追加するための物です。
原案はSmartPickupSharp(SmartPickupSharp_Readme.txt参照)ですが、実装は大きく異なっています。

# 目次
- [ビルドプロセス](#ビルドプロセス)
- [コンポーネント](#コンポーネント)
	- [LUPickUpBase_LateUpdatePickUpBase](#lupickupbase_lateupdatepickupbase)
	- [LUPickUpRC_RootChangeable](#lupickuprc_rootchangeable)
	- [LUP_RC_CatcherCollider](#lup_rc_catchercollider)
# ビルドプロセス
参照を同期する必要からビルドプロセスにて全レール、列車を配列登録しています。

他に連結器参照の自動設定や、走行用コンポーネントの設定値変更が今後増える可能性があります。

## SPS_BuildProcess
- 参照同期のためのリストや参照を作ります
- LUP_RC_ColliderManagerを探します。（なければ作ります）
- LUPickUpRC_RootChangeableを探し、LUP_RC_ColliderManagerの参照を渡します。
- LUP_RC_CatcherColliderを探し、リストを作ります。LUP_RC_CatcherColliderにはindexを渡しておきます。

# コンポーネント
## LUPickUpBase_LateUpdatePickUpBase

LateUpdate、Local座標で動作するPickup用のUdonです。

### 仕様
元のPickupの位置更新はほぼ殺しています。

外部から位置を更新する事は想定していません。

|設定値|概要|
|---:|:---|
isLocal|trueにするとOwner変更を無視します。同期を切っていてもPickupはOwnerを持つため、その挙動を回避するためのプロパティです。

|関数|概要|同期|
|---:|:---|:---|
ResetPosition|初期位置にリセットします。|内部実行
SetPositionAndRotation_OwnerOnly|Global値で位置・角度を設定します。Owner以外では動作しません。|なし

## LUPickUpRC_RootChangeable

LUPickUpBaseから、親座標系を移れるようにしたUdonです。
LUP_RC_CatcherColliderと合わせて使用します。

### 仕様
VRCPickupの位置更新が特殊な仕様を持つため、orientation=GunかつExactGun=Noneでないと判定がズレるようです。

Catcherから出た判定になった後、コライダーを一瞬切って入れ直すことで入れ子コライダーにも対応しています。

|設定値|概要|
|---:|:---|
isLocal|trueにするとOwner変更を無視します。同期を切っていてもPickupはOwnerを持つため、その挙動を回避するためのプロパティです。
Tags_ExcludeExceptMode|falseではExceptCatcherTagsに含まれるタグを持つCathcerを除外します。<br>trueとするとExceptCatcherTagsに含まれるタグを持つCathcerとのみ接触するようになります。
ExceptCatcherTags|除外/包含するタグ文字列です。
PickupTags|自身のタグです。

|関数|概要|同期|
|---:|:---|:---|
ResetPosition|初期位置にリセットします。親も初期の物に戻ります。|内部実行
SetPositionAndRotation_OwnerOnly|Global値で位置・角度を設定します。Owner以外では動作しません。|なし
SetParentToNull|親をNullにします（自身をRoot直下のオブジェクトにします）|なし
SetParentToCollider|任意のCatcherを親に設定します。|なし

## LUP_RC_CatcherCollider

LUPickUpRC_RootChangeableと合わせて使用します。

### 仕様
Scaleは必ず1,1,1で用いてください。


|設定値|概要|
|---:|:---|
isHook|trueの場合、接触したPickupを手からもぎ取れるようになります。また、持たれていない状態のPickupに接触した際にも動作するようになります。<br>falseでは持たれていないPickupと接触しても何も起こりません。
isSyncOwner|trueの場合、接触したPickupのOwnerを自身のOwnerで上書きします。
dropTarget|設定しておくと、Catcher内でDropされたPickupが定位置に移動するようになります。dropTargetの子がある場合、子の内最も距離×角度が小さい物に移動します。
Tags_ExcludeExceptMode|falseではExceptCatcherTagsに含まれるタグを持つPickupを除外します。trueとするとExceptCatcherTagsに含まれるタグを持つPickupとのみ接触するようになります。
ExceptPickupTags|除外/包含するタグ文字列です。
CatcherTags|自身のタグです。
