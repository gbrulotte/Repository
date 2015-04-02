﻿using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatCommunication;
using System.Windows.Controls;
using MVVM.Container;
using System.Diagnostics;

namespace ChatClient.ViewModels
{
    class LoginViewModel : BindableBase 
    {
        private User _user;

        public LoginViewModel()
        {
            this.LoginCommand = new DelegateCommand<object>(Login);
            this.SubscribeCommand = new DelegateCommand<object>(Subscribe);
            this._user = new User();
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand SubscribeCommand { get; private set; }
        public User User
        {
            get { return this._user; }
            set { SetProperty(ref this._user, value); }
        }

        private void Login(object password)
        {
            if (!Client.IsConnected())
            {
                Client.EstablishConnection();
            }

            //user.Password = Encode_Pass(((PasswordBox)password).Password);
            User.Password = ((PasswordBox)password).Password;

            if (Client.LogClient(User))
            {
                //reponse positive, on a pas recu Error! => On est logger
                Container.GetA<MainViewModel>().NavigateToView(Container.GetA<LobbyViewModel>());
            }

        }

        private void Subscribe(object password)
        {
            if (!Client.IsConnected())
            {
                Client.EstablishConnection();
            }

            //user.Password = Encode_Pass(((PasswordBox)password).Password);
            User.Password = ((PasswordBox)password).Password;

            if (Client.SubClient(User))
            {
                //move to profil creation and set the profile from lobby information
                Container.GetA<EditProfileViewModel>().Profile = Container.GetA<LobbyViewModel>().Lobby.ClientProfile;
                Container.GetA<MainViewModel>().NavigateToView(Container.GetA<EditProfileViewModel>());
            }

        }
        private string Encode_Pass(string pass)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes("trYT0" + pass + "H4cKme");
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.UTF8.GetString(data);
        }

    }
}
