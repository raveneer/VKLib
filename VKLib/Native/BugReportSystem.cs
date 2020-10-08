using System;
using System.Runtime.Serialization;
using Zenject;

namespace VKLib.Native
{
    public class BugReportSystem : IInitializable
    {
        [Inject] private EventManager _eventManager;

        public void Initialize()
        {
            TDebug.AssertNotNull(_eventManager, nameof(_eventManager));
            //_eventManager.ReportBug += OnReportBug;
        }

        private void OnReportBug(TException excetption)
        {
            ReportBugToAnalytics(excetption);
            //_eventManager.Raise_AlertMessageSpawn(excetption.Message, new List<AlertButtonInfo>());
        }

        //타이틀로 보내고 싶으나, 엔티타스가 깨지면 타이틀로 보내지지 않는다 (복구가 안됨)
        private void GoToTitle()
        {
            _eventManager.Raise_PrepareSceneLoad();
            //_eventManager.Raise_LoadScene(SceneNames.Title);
            throw new NotImplementedException("프로젝트별로 맞는 타이틀로 보낼 것. 타이틀씬의 이름이 항상 같다거나...?");
        }

        private void ReportBugToAnalytics(TException tExcetption)
        {
            throw tExcetption;
        }
    }

    [Serializable]
    public class TException : Exception
    {
        public TException()
        {
        }

        public TException(string message) : base(message)
        {
        }

        public TException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}