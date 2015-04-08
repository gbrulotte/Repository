﻿using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ChatCommunication;
using Microsoft.Practices.Prism.Commands;
using MVVM.Container;
using System.Collections.ObjectModel;
using ChatClient.Utils;

namespace ChatClient.ViewModels
{
    class LobbyViewModel : BindableBase
    {
        private Lobby _lobby;
        //private Profile _userProfile;
        private readonly Lazy<ObservableCollection<RoomItemViewModel>> _roomItems;
        private RoomViewModel _roomViewModel;


        public LobbyViewModel()
        {
            DisconnectCommand = new DelegateCommand(Disconnect);
            EditProfileCommand = new DelegateCommand(EditProfile);
            ViewProfileCommand = new DelegateCommand(ViewProfile);
            CreateRoomCommand = new DelegateCommand(CreateRoom);
            
            _lobby = new Lobby();
            _roomViewModel = new RoomViewModel();

            Func<Room, RoomItemViewModel> roomItemsViewModelCreator = model => new RoomItemViewModel() { RoomItem = model };
            Func<ObservableCollection<RoomItemViewModel>> roomItemsCollectionCreator =
                () => new ObservableViewModelCollection<RoomItemViewModel, Room>(Lobby.AllRooms, roomItemsViewModelCreator);
            _roomItems = new Lazy<ObservableCollection<RoomItemViewModel>>(roomItemsCollectionCreator);
        }


        public Lobby Lobby
        {
            get { return _lobby; }
            set { SetProperty(ref _lobby, value); }
        }

        public RoomViewModel RoomViewModel
        {
            get { return _roomViewModel; }
        }
 
        public bool IsInRoom
        {
            //True si l'user a pas -1 et si on a recu la bonne room dans le updateRoom!
            get { return Lobby.ClientProfile.IDRoom != -1 && Lobby.ClientProfile.IDRoom == RoomViewModel.Room.IDRoom; }
        }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand EditProfileCommand { get; private set; }
        public ICommand ViewProfileCommand { get; private set; }
        public ICommand CreateRoomCommand { get; private set; }

        public void Disconnect()
        {
            Client.DisconnectClient();
            Container.GetA<MainViewModel>().NavigateToView(Container.GetA<LoginViewModel>());
        }

        public void EditProfile()
        {
            Container.GetA<EditProfileViewModel>().Profile = Lobby.ClientProfile;
            Container.GetA<MainViewModel>().NavigateToView(Container.GetA<EditProfileViewModel>());
        }

        public void ViewProfile()
        {
            Client.ViewProfile(Lobby.ClientProfile.Pseudo);
        }

        public void ViewProfileCallback()
        {
            Container.GetA<MainViewModel>().NavigateToView(Container.GetA<ViewProfileViewModel>());
        }

        public void CreateRoom()
        {
            Container.GetA<CreateRoomViewModel>().Room = new Room();
            Container.GetA<MainViewModel>().NavigateToView(Container.GetA<CreateRoomViewModel>());
        }
    }
}
