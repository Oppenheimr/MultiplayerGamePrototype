using System.Threading.Tasks;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Network;
using Network.PlayFab;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityUtils.Extensions;

namespace UI.Lobby.Panels
{
    public class LoginPanel : BasePanel
    {
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _changePanel;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private Color _messageColor;
        [SerializeField] private Color _alertColor;
        
        [Header("Login")]
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private GameObject _loginButtonRoot;
        [SerializeField] private GameObject _registerButtonRoot;
        
        [Header("Register")]
        [SerializeField] private TMP_InputField _rePassword;
        [SerializeField] private TMP_InputField _playerNameInput;
        
        private CanvasGroup _messageCanvasGroup;
        private CanvasGroup MessageCanvasGroup => _messageCanvasGroup ? _messageCanvasGroup : (_messageCanvasGroup = _message.GetComponent<CanvasGroup>());


        private bool _isLoginPanel;

        protected override void Start()
        {
            base.Start();
            
            if (Authentication.isLogged)
            {
                PanelManager.Instance.ShowPanel(typeof(CharacterSelectionPanel), true);
                return;
            }
            
            ShowLoginOrRegister();

            if (!LocalDatabase.email.HasValue)
                return;
            
            _email.text = LocalDatabase.email.Value;
            _password.text = LocalDatabase.password.Value;
        }

        public override TweenerCore<float, float, FloatOptions> ShowPanel()
        {
            if (!Authentication.isLogged)
                return base.ShowPanel();
            
            PanelManager.Instance.ShowNextPanel();
            return null;
        }

        public async void OnLoginButtonClicked()
        {
            LoadingBar.Instance.ShowLoadingBar("Logging in...");
            string email = _email.text;
            
            if (!email.Equals(""))
            {
                var loginResult = await Authentication.Login(_email.text, _password.text);
                
                if (!loginResult.success)
                {
                    ShowMessage(loginResult.error, true);
                    LoadingBar.Instance.HideLoadingBar();
                    return;
                }
                
                LocalDatabase.email.Value = _email.text;
                LocalDatabase.password.Value = _password.text;
                
                if (!PhotonManager.Login(loginResult.item.AccountInfo.Username))
                {
                    LoadingBar.Instance.HideLoadingBar();
                    return;
                }

                await PhotonManager.WaitConnectToMaster();
                await PhotonManager.JoinLobby();
                
                PanelManager.Instance.ShowPanel(typeof(CharacterSelectionPanel));
            }
            else
            {
                LoadingBar.Instance.HideLoadingBar();
                ShowMessage("Email is invalid.", true);
            }
            LoadingBar.Instance.HideLoadingBar();
        }
        
        public async void OnRegisterButtonClicked()
        {
            LoadingBar.Instance.ShowLoadingBar("Registering...");
            
            if (_rePassword.text != _password.text)
            {
                Debug.LogError("Passwords do not match.");
                LoadingBar.Instance.HideLoadingBar();
                return;
            }
            
            var registerResult = await Authentication.Register(_email.text, _password.text, _playerNameInput.text);
                
            if (!registerResult.success)
            {
                ShowMessage(registerResult.error, true);
                LoadingBar.Instance.HideLoadingBar();
                return;
            }
            
            ShowMessage("Successfully registered!");
            LoadingBar.Instance.HideLoadingBar();
            ShowLoginOrRegister();
        }
        
        public void ShowLoginOrRegister()
        {
            _isLoginPanel = !_isLoginPanel;
            _header.text = _isLoginPanel ? "Login" : "Register";
            _changePanel.text = _isLoginPanel ? "New member ?   <color=purple>Register Now</color>" : "Already a member?   <color=purple>Log In</color>";
            
            _rePassword.transform.parent.SetActivate(!_isLoginPanel);
            _playerNameInput.transform.parent.SetActivate(!_isLoginPanel);
            
            _loginButtonRoot.SetActive(_isLoginPanel);
            _registerButtonRoot.SetActive(!_isLoginPanel);
        }
        
        private void ShowMessage(string message, bool isAlert = false)
        {
            _message.text = message;
            _message.color = isAlert ? _alertColor : _messageColor;
            MessageCanvasGroup.alpha = 1;
            MessageCanvasGroup.DOFade(1, 2).OnComplete(() => {
                MessageCanvasGroup.DOFade(0, .5f).SetDelay(3);
            });
        }
    }
}