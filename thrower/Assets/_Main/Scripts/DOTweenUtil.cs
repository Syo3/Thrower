using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Utilities.Extensions
{
    public static class DOTweenUtil
    {
        /// <summary>
        /// 新規シーケンス作成
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Sequence NewSequence(GameObject obj=null)
        {
            var sequence = DOTween.Sequence().SetLink(obj);
            return sequence;
        }

        /// <summary>
        /// シーケンスに中身がないかチェック
        /// </summary>
        /// <param name="seq"></param>
        public static void CheckSequence(ref Sequence seq, GameObject obj=null)
        {
            if(seq != null) KillSequence(ref seq);
            seq = NewSequence(obj);
        }

        /// <summary>
        /// シーケンス終了
        /// </summary>
        /// <param name="seq"></param>
        public static void KillSequence(ref Sequence seq)
        {
            if(seq == null) return;
            seq.Kill();
            seq = null;
        }

        public static void Delay(float time, System.Action action)
        {
            DOTween.Sequence().AppendInterval(time).AppendCallback(() => {action?.Invoke();});
        }
    }
}