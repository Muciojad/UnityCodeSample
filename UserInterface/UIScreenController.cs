namespace Muciojad.SpaceHorror.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Systems.Input;
    using Gameplay.PauseGame;
    using Interfaces;
    using Pointer;
    using UnityEngine;
    using Zenject;

    public class UIScreenController : IUIScreenRequestHandler, IUIScreenController, IInitializable, IDisposable
    {
        #region Public Methods
        
        void IUIScreenController.OpenScreen(ScreenType.ScreenType screenType)
        {
            PauseGame.Pause(true);
            _Input.SetUIMap();
            OpenScreenRequest?.Invoke(screenType);
            
            if (!_OpenScreenStack.Contains(screenType))
            {
                _OpenScreenStack.Push(screenType);
            }
            
            ScreenPointer.SetEnabled(false);
        }

        void IUIScreenController.CloseScreen(ScreenType.ScreenType screenType)
        {
            CloseScreenRequest?.Invoke(screenType);
            if (_RegisteredScreens.Count(x => x.Active) != 0) return;
            _Input.SetGameplayMap();
            ScreenPointer.SetEnabled(true);
            PauseGame.Unpause();
        }

        void IUIScreenController.CloseCurrent()
        {
            CloseCurrent();
        }

        void IUIScreenRequestHandler.AddOpenRequest(Action<ScreenType.ScreenType> openRequest, IUIScreen sender)
        {
            if (!_RegisteredScreens.Contains(sender))
            {
                _RegisteredScreens.Add(sender);
            }

            OpenScreenRequest += openRequest;
        }
        void IUIScreenRequestHandler.RemoveOpenRequest(Action<ScreenType.ScreenType> openRequest, IUIScreen sender)
        {
            if (_RegisteredScreens.Contains(sender))
            {
                _RegisteredScreens.Remove(sender);
            }
            OpenScreenRequest -= openRequest;
        }
        void IUIScreenRequestHandler.AddCloseRequest(Action<ScreenType.ScreenType> closeRequest)
        {
            CloseScreenRequest += closeRequest;
        }
        void IUIScreenRequestHandler.RemoveCloseRequest(Action<ScreenType.ScreenType> closeRequest)
        {
            CloseScreenRequest -= closeRequest;
        }
        
        
        #endregion

        #region Unity Methods
        void IInitializable.Initialize()
        {
            _Input.OnUICancelButtonDown += CloseCurrent;
        }

        void IDisposable.Dispose()
        {
            _Input.OnUICancelButtonDown -= CloseCurrent;
        }
        #endregion

        #region Private Variables
        private List<IUIScreen> _RegisteredScreens = new List<IUIScreen>();

        private event Action<ScreenType.ScreenType> OpenScreenRequest; 
        private event Action<ScreenType.ScreenType> CloseScreenRequest;
        
        private Stack<ScreenType.ScreenType> _OpenScreenStack = new Stack<ScreenType.ScreenType>();
        
        [Inject] private GameInput _Input;
        #endregion

        #region Private Methods
        private void CloseCurrent()
        {
           var lastOpenScreen = _OpenScreenStack.Pop();
           ((IUIScreenController)this).CloseScreen(lastOpenScreen);
        }
        #endregion

    }
}