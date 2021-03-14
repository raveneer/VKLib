using System.Collections.Generic;
using System.Linq;

namespace VKLib.Native
{
    /// <summary>
    /// 들어온 순서대로 누적합을 만들어, 길이별로 확률을 부여한다. 따라서 들어오는 순서에 상관없이 확률이 맞게 나옴.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Frequency<T>
    {
        private readonly List<(float, T)> _frequents = new List<(float, T)>();
        private float _maxPercent;

        public Frequency(params (float, T)[] elems)
        {
            var threshold = 0f;
            foreach (var elem in elems)
            {
                threshold += elem.Item1;
                _frequents.Add((threshold, elem.Item2));
                //TDebug.Log($"added freq = {threshold}, {elem.Item2}");
            }
            _maxPercent = threshold;
        }

        public T GetNextItem()
        {
            TDebug.Assert(_frequents.Any());
            float randomPercent = TRandom.Range(0f, _maxPercent);

            for (int i = 0; i < _frequents.Count; i++)
            {
                if (randomPercent <= _frequents[i].Item1)
                {
                    return _frequents[i].Item2;
                }
            }
            return default(T);
        }

        public override string ToString()
        {
            var res = "";
            foreach (var valueTuple in _frequents)
            {
                res += $"weight : {valueTuple.Item1}, {valueTuple.Item2.GetType()} ,";
            }
            return res;
        }
    }
}