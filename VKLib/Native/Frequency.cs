using System.Collections.Generic;
using System.Linq;

namespace VKLib.Native
{
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
            }
            _maxPercent = threshold;
        }

        public T GetNextItem()
        {
            TDebug.Assert(_frequents.Any());
            var randomPercent = TRandom.Range(0f, _maxPercent);

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