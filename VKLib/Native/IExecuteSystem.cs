/// <summary>
/// 엔티타스를 빼내며 독자적으로 구축함. 별 차이는 없다.
/// </summary>

namespace VKLib.Native
{
    public interface IExecuteSystem : ISystem
    {
        void Execute();
    }
}