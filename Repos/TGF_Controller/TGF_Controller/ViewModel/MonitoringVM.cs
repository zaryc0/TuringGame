using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TGF_Controller.Controller;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    internal class MonitoringVM : BaseViewModel, IMonitoringVm
    {
        //Properties
        private object _activeRoom;
        private string _header,
                       _subjectType,
                       _interviewerType,
                       _subjectImgUri,
                       _interviewerImgURI,
                       _buttonText;
        private int _hostPort;
        private int _index;

        //Constructor
        public MonitoringVM(IRoom room)
        {
            Tabs = new ObservableCollection<IRoomVM>();
            AddTab(room);
            _activeRoom = Tabs[0];
            Header = $"Observing {Tabs[0].GetRoomName()}";
            CloseRoomButtonText = "Close Selected Room";
            SubjectType = "Uknown";
            InterviewerType = "Uknown";
            SetOccupantDetails();
            _hostPort = Bus.hostPortNum;
            CloseActiveRoom = new RelayCommand(o => CloseRoom(_index));
            TabChangeCommand = new RelayCommand(o => ChangeTab(_index));
        }

        //Property Masks
        public ObservableCollection<IRoomVM> Tabs { get; set; }
        public string PortNumber
        {
            get => $"Session Code:{_hostPort}";
            set
            {
                if (value != $"{_hostPort}")
                {
                    _hostPort = int.Parse(value);
                    NotifyPropertyChanged();
                }
            }
        }
        public string CloseRoomButtonText
        {
            get => _buttonText;
            set
            {
                if (value != _buttonText)
                {
                    _buttonText = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Header
        {
            get => _header;
            set
            {
                if (value != _header)
                {
                    _header = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SubjectType
        {
            get => _subjectType;
            set
            {
                if (value != _subjectType)
                {
                    _subjectType = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SubjectImgURI
        {
            get => _subjectImgUri;
            set
            {
                if (value != _subjectImgUri)
                {
                    _subjectImgUri = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string InterviewerType
        {
            get => _interviewerType;
            set
            {
                if (value != _interviewerType)
                {
                    _interviewerType = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string InterviewerImgURI
        {
            get => _interviewerImgURI;
            set
            {
                if (value != _interviewerImgURI)
                {
                    _interviewerImgURI = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public object Content
        {
            get => _activeRoom;
            set
            {
                if (_activeRoom != value)
                {
                    _activeRoom = value;
                    SetOccupantDetails();
                    NotifyPropertyChanged();
                }
            }
        }
        public int TabIndex
        {
            get => _index;
            set
            {

                if (value >= 0)
                {
                    _index = value;
                }

            }
        }

        //Commands
        public ICommand CloseActiveRoom { get; set; }
        public ICommand TabChangeCommand { get; set; }

        //Functions
        public void AddTab(IRoom room)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Tabs.Add(new RoomVM(room));
                NotifyPropertyChanged();
            });
        }
        public void UpdateTab(int id,IMessage m)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Tabs[id].MessageVMs.Add(new MessageVM(m));
            });
        }
        public void ChangeTab(int index)
        {
            int roomID = Tabs[index].GetRoom().GetID();
            Tabs[index] = new RoomVM(Bus.GetRoom(roomID));
            _activeRoom = Tabs[index];
            IRoomVM temp = (IRoomVM)_activeRoom;
            Content = _activeRoom;
            Header = $"Observing {temp.GetRoomName()}";
            SetOccupantDetails();
        }
        public void UpdateMessages(IMessage message, int roomID)
        {
            if (_activeRoom.GetType().Equals(typeof(RoomVM)))
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    if(message.Source == Constants.Subject_Tag)
                    {
                        Tabs[roomID].MessageVMs.Add(new Message2VM(message));
                    }
                    else
                    {
                        Tabs[roomID].MessageVMs.Add(new MessageVM(message));
                    }
                
                    if (roomID == ((IRoomVM)_activeRoom).ID)
                    {
                        Content = Tabs[roomID];
                    }
                });
            }
            

        }
        private void SetOccupantDetails()
        {
            try
            {
                IRoomVM temp = (IRoomVM)_activeRoom;
                IRoom room = Bus.GetRoom(temp.GetRoom().GetID());

                if (room.HasInterviewer)
                {
                    InterviewerType = "Interviewer is Human";
                    InterviewerImgURI = Constants.Human128_Img_File_Path;
                }
                else
                {
                    InterviewerType = "Interviewer is Absent";
                    InterviewerImgURI = Constants.Unknown_img_File_Path;
                }
                if (room.HasSubject)
                {
                    if (room.HasRobot)
                    {
                        SubjectType = "Subject is Robot";
                        SubjectImgURI = Constants.Robot128_Img_File_Path;
                    }
                    else
                    {
                        SubjectType = "Subject is Human";
                        SubjectImgURI = Constants.Human128_Img_File_Path;
                    }
                }
                else
                {
                    SubjectType = "Subject is Absent";
                    SubjectImgURI = Constants.Unknown_img_File_Path;
                }
            }
            catch
            {

            }
        }
        public void CloseAllRooms()
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                CloseRoom(i);
            }
        }
        public void CloseRoom(int index)
        {
            try
            {
                IRoomVM temp = (IRoomVM)_activeRoom;
                TabIndex = 0;
                Bus.CloseRoom(Tabs[index].ID);
                if (temp.ID == Tabs[index].ID)
                {
                    Content = new RoomClosedVM();
                }
                Tabs.RemoveAt(index);
            }
            catch
            {
                return;
            }

        }
    }
}
