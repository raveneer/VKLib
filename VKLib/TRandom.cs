using System;

/// <summary>
///     트라이브스에 사용하는 커스텀 랜덤.
///     todo : Random에 인자를 넘겨서 시드로 쓸 수 있도록
/// </summary>
public class TRandom
{
    public static Random Random = new Random();
    public static bool Bool => Random.Next(0, 10) % 2 == 0;

    /// <summary>
    ///     시드를 변경하면 랜덤이 새로 생성되는 것에 주의.
    /// </summary>
    public int Seed
    {
        get => _seed;
        set => Random = new Random(value);
    }

    private int _seed;

    /// <summary>
    ///     SampleGaussian을 지정된 범위 내에서 생성하게 하는 헬퍼 함수.
    ///     표준편차는 평균의 1/3을 쓴다. 정확하진 않으며 그럴듯한 값을 만들어냄.
    ///     최소 최대값은 '포함' 임.
    /// </summary>
    public static double RandomNumberInStandardGaussian(double min, double max)
    {
        if (min >= max)
        {
            throw new ArgumentException($"max {max} must bigger than min {min} ");
        }

        var mean = (min + max) / 2d;
        double next;
        do
        {
            next = SampleGaussian(mean, mean / 3);
        } while (next < min || next > max);

        return next;
    }

    /// <summary>
    ///     include min, exclude max
    /// </summary>
    public static int Range(int min, int max)
    {
        return Random.Next(min, max);
    }

    public static float Range(float min, float max)
    {
        double range = max - min;
        var sample = Random.NextDouble();
        var scaled = sample * range + min;
        var f = (float) scaled;
        return f;
    }

    /// <summary>
    ///     https://gist.github.com/tansey/1444070
    ///     표준편차 랜덤을 구해낸다. mean 50 seddev 15일 때, 5~95 사이에 99%의 값이 생성된다.
    /// </summary>
    /// <param name="mean">최빈값(평균)</param>
    /// <param name="stddev">표준편차 (1시그마)</param>
    /// <returns></returns>
    public static double SampleGaussian(double mean, double stddev)
    {
        // The method requires sampling from a uniform random of (0,1]
        // but Random.NextDouble() returns a sample of [0,1).
        var x1 = 1 - Random.NextDouble();
        var x2 = 1 - Random.NextDouble();

        var y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
        return y1 * stddev + mean;
    }
}