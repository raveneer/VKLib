using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;
using UnityEngine.SocialPlatforms;

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace VKLib
{


#if UNITY_ANDROID

    /// <summary>
    /// 싱글턴/매니저. 구글 플레이 게임즈 서비스를 관리한다. 
    /// </summary>
    /// 
    public class GPGSMng : MonoBehaviour
    {


        #region singletone
        public static GPGSMng instance = null;     //Allows other scripts to call functions from this singletone.             

        void Awake()
        {
            //Check if there is already an instance of instance
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of this.
                Destroy(gameObject);

            //Set to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);

        }
        #endregion

        public Text t_GPGSDebugtext;

        public string serverAuthCode = string.Empty;
        public string IDToken = string.Empty;

        //시작
        void Start()
        {
            Invoke("StartGPGS", 1);
        }

        public void StartGPGS()
        {
            //ID토큰을 받기위해 컨피그를 해준다.
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                    .Builder()
                    .RequestIdToken()
                    .RequestServerAuthCode(false)
                    .Build();

            //컨피그된 정보로 GPGS를 초기화 해 준다.
            PlayGamesPlatform.InitializeInstance(config);
            //Debug.Log("GPGS초기화 완료");

            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            //GPGS 시작.
            PlayGamesPlatform.Activate();
        }

        public void ShowLeaderBoard_Gold()
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {

                ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_pantyrank);

            }
        }

        /// <summary>
        /// 로그인 한다.
        /// </summary>
        public void Login()
        {
            if (Social.localUser.authenticated == true)
            {
                LogGPGS("You already loged in");
                GetTokens();
            }
            else
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        LogGPGS("You've successfully logged in");
                        GetTokens();
                    }
                    else
                    {
                        LogGPGS("Login failed for some reason");
                    }
                });
            }
        }

        /// <summary>
        /// 유저의 토큰을 획득한다.
        /// </summary>
        /// <returns></returns>
        public void GetTokens()
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                //유저 토큰 받기 첫번째 방법
                //string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
                //두번째 방법
                IDToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

                //인증코드 받기
                serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            }
            else
            {
                LogGPGS("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            }
        }


        /// <summary>
        /// 리더보드에 점수를 등록한다.
        /// </summary>
        public void SubmitToLeaderBorad(int score)
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                Social.ReportScore(score, GPGSIds.leaderboard_pantyrank, (bool success) =>
                {
                    if (success)
                    {
                    //Debug.Log("Update Score Success");
                    t_GPGSDebugtext.text = "Update Score Success";

                    }
                    else
                    {
                    //Debug.Log("Update Score Fail");
                    t_GPGSDebugtext.text = "Update Score Fail";
                    }
                });
            }
            else
            {
                //Debug.Log("Need Log in");
                t_GPGSDebugtext.text = "Need Log in";
            }
        }

        /// <summary>
        /// 도전과제 리스트를 연다. (구글플레이)
        /// </summary>
        public void ShowAchievements()
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ShowAchievementsUI();
                LoadAchievementData();
            }
            else
            {
                //Debug.Log("Cannot show Achievements, not logged in");
                t_GPGSDebugtext.text = "Cannot show Achievements, not logged in";
            }
        }

        /// <summary>
        /// 현재 도전과제 데이터를 구글 플레이에서 불러와 반환한다.
        /// </summary>
        public void LoadAchievementData()
        {
            Social.LoadAchievements(achievements =>
            {
                if (achievements.Length > 0)
                {
                    //Debug.Log("Got " + achievements.Length + " achievement instances");
                    string myAchievements = "My achievements:\n";
                    foreach (IAchievement achievement in achievements)
                    {
                        myAchievements += "\t" +
                            achievement.id + " " +
                            achievement.percentCompleted + " " +
                            achievement.completed + " " +
                            achievement.lastReportedDate + "\n";
                    }
                    //Debug.Log(myAchievements);
                    t_GPGSDebugtext.text = myAchievements;
                }
                else
                {
                    //Debug.Log("No achievements returned");
                    t_GPGSDebugtext.text = "No achievements returned";
                }
            });
        }

        public void ReportAchievement(Achievement _achive)
        {

            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.UnlockAchievement(_achive.googleAchiveID, (bool success) =>
                {
                    if (success)
                    {
                        t_GPGSDebugtext.text = "_googleAchievementID : " + _achive.googleAchiveID + " Report Success";
                        _achive.GetSaveData().bIsReportedGoogle = true;
                    }
                    else
                    {
                        t_GPGSDebugtext.text = "_googleAchievementID : " + _achive.googleAchiveID + " Report fail";
                    }
                });
            }
        }



        /// <summary>
        /// 로그아웃 한다.
        /// </summary
        public void SignOut()
        {
            PlayGamesPlatform.Instance.SignOut();
            LogGPGS("로그아웃 되었습니다");
        }

        //디버그용 테스트
        public void TestReportAchive()
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement100000Touch, (bool success) =>
                 {
                     t_GPGSDebugtext.text = "_googleAchievementID :  Report Success";
                 });
            }
        }

        /// <summary>
        /// 편리한 로그 출력을 위한 헬퍼함수
        /// </summary>
        /// <param name="_log"></param>
        public void LogGPGS(string _log)
        {
            //Debug.Log(_log);
            t_GPGSDebugtext.text = _log;
        }

        /// <summary>
        /// 토큰을 출력한다.
        /// </summary>
        public void Debug_ShowToken()
        {
            LogGPGS("authcode : " + serverAuthCode + " / " + "idtoken : " + IDToken);
        }

        /// <summary>
        /// 로그인 하여, 파이어베이스 서버까지 연결한다.
        /// </summary>
        public void Debug_LoginGPGSandFB()
        {
            throw new NotImplementedException("파이어베이스 쓸거야?");
            /*if (Social.localUser.authenticated == true)
            {
                LogGPGS("You already loged in");
                GetTokens();
                FireBaseManager.instance.AuthWithGoogleToken();
            }
            else
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        LogGPGS("You've successfully logged in");
                        GetTokens();
                        FireBaseManager.instance.AuthWithGoogleToken();
                    }
                    else
                    {
                        LogGPGS("Login failed for some reason");
                    }
                });
            }*/
        }




    }

    /*

    /// <summary>
    /// GPGS 에서 사용자 이름을 가져옵니다.
    /// </summary>
    /// <returns> 이름 </returns>
    public string GetNameGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.userName;
        else
            return null;
    }



    /// <summary>
    /// GPGS에서 자신의 프로필 이미지를 가져옵니다.
    /// </summary>
    /// <returns> Texture 2D 이미지 </returns>
    public Texture2D GetImageGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.image;
        else
            return null;
    }
    */
#endif
}


/// <summary>
/// 게임마다 다를 것이다. 별도로 빼 줄 것.
/// </summary>
public static class GPGSIds
{
    public static string leaderboard_pantyrank = "CgkIu-jqz9MaEAIQBg";
    public static string achievementFirstTouch = "CgkIu-jqz9MaEAIQAQ";
    public static string achievement1000Touch = "CgkIu-jqz9MaEAIQAw";
    public static string achievement10000Touch = "CgkIu-jqz9MaEAIQBA";
    public static string achievement100000Touch = "CgkIu-jqz9MaEAIQBQ";
}

/// <summary>
/// https://mentum.tistory.com/382 에서 가져온 참조용 코드.
/// </summary>
public class GooglePlayManager : MonoBehaviour
{
    #region 싱글톤
    private static GooglePlayManager _instance = null;

    public static GooglePlayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GooglePlayManager)FindObjectOfType(typeof(GooglePlayManager));
                if (_instance == null)
                {
                    Debug.Log("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    #region 구글플레이 초기화 및 로그인
    private void Start() //동적도 할당을 해주어야 한다.
    {
        CheckSocialLogin();
    }

    public bool isProcessing
    {
        get;
        private set;
    }
    public string loadedData
    {
        get;
        private set;
    }

    public bool IsLogined => Social.localUser.authenticated;

    private void CheckSocialLogin()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //.EnableSavedGames() // 저장된 게임 활성화 : 안쓰시니까 주석처리
        .Build();

        PlayGamesPlatform.InitializeInstance(config);

        // 구글 플레이 로그를 확인할려면 활성화
        PlayGamesPlatform.DebugLogEnabled = false;

        // 구글 플레이 활성화
        PlayGamesPlatform.Activate();

        // 로그인 안돼있을 경우 로그인 호출
        if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
            SocialSignIn();

#elif UNITY_IOS
        SocialSignIn();
#endif
    }

    // 로그인
    public void SocialSignIn()
    {
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
                Debug.Log("로그인 성공");
#if UNITY_IOS
                GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
            }
            else
            {
                Debug.Log("로그인 실패");
            }

        });
    }

    #endregion

    #region 리더보드
    public void ShowMainLeaderBoard()
    {
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                }
                else
                {
                    // Sign In 실패 
                    return;
                }
            });
        }

#if UNITY_ANDROID
        //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_sum_of_all_stage_score);
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI(GPGSIds.leaderboard_sum_of_all_stage_score, UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
    }
    #endregion

    #region 업적
    public void ShowAchievement()
    {
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    return;
                }
                else
                {
                    // Sign In 실패 처리
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    #endregion
}