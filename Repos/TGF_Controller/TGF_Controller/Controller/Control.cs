﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Controller.Network;
using TGF_Controller.Controller.Network.interfaces;
using TGF_Controller.Model;

namespace TGF_Controller.Controller
{
    class Control
    {
        //Properties
        private float _numberOfConnectionsMade;
        private ISocketHandler _hostSocket;
        private List<Thread> _threadList;
        private int _roomPortBase;
        private int _lastRoomID;
        private bool _active;
        public Dictionary<int, IRoom> roomList;
        
        //Constructor
        public Control()
        {
            _active = true;
            _numberOfConnectionsMade = 0;
            _hostSocket = new SocketHandler(4839);
            roomList = new Dictionary<int, IRoom>();
            _threadList = new List<Thread>();
            _roomPortBase = 1000;
            Thread T = new(() => ManageController());
            T.Start();
        }


        //Functions
        public bool CreateRoom(int roomID,int subPortNum, int intPortNum)
        {
            if (roomList.Count < Constants.Room_Limit)
            {
                IRoom temp = new Room(subPortNum, intPortNum, false, roomID);
                roomList.Add(temp.GetID(),temp);
                _threadList.Add(new Thread(() => ManageRoom(roomID)));
                _threadList[roomID].Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ManageRoom(int room)
        {
            roomList[room].Run();
        }

        public IRoom GetRoom(int room_ID)
        {
            return roomList[room_ID];
        }

        public void ManageController()
        {
            while (_active)
            {
                if (_numberOfConnectionsMade < Constants.User_limit)
                {
                    _ = _hostSocket.WaitForPrimaryConnection();

                    _numberOfConnectionsMade++;
                    int requiredRooms = (int)((_numberOfConnectionsMade / 2) + 0.5);
                    if (requiredRooms > roomList.Count)
                    {
                        int roomID = GetFreeRoomID();
                        _lastRoomID = roomID;
                        int temp = _roomPortBase + (100 * roomID);
                        int subport = temp + 50;
                        _ = CreateRoom(roomID, subport, temp);
                        Bus.CreateNewRoomView(roomList[roomID]);
                        _hostSocket.BroadcastOnPrimary(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{temp},<MessageEnd/>"));
                    }
                    else
                    {
                        int subport = _roomPortBase + (100 * _lastRoomID) + 50;
                        _hostSocket.BroadcastOnPrimary(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{subport},<MessageEnd/>"));
                    }
                }
            }
        }

        public int GetFreeRoomID()
        {
            IEnumerable<int> freeroomIDs;
            List<int> allRoomIDs = new();
            List<int> roomIDs = new();

            if (roomList.Count > 0)
            {
                for (int i = 0; i < Constants.Room_Limit; i++)
                {
                    allRoomIDs.Add(i);
                }
                foreach (IRoom room in roomList.Values)
                {
                    roomIDs.Add(room.GetID());
                }
                freeroomIDs = allRoomIDs.Except(roomIDs);
                return freeroomIDs.Min();
            }
            else
            {
                return 0;
            }

        }

        internal void CloseRoom(int index)
        {
            roomList[index].Kill();
            _ = roomList.Remove(index);
            _numberOfConnectionsMade -= 2;
        }

        internal void Kill()
        {
            _active = false;
            bool temp = true;
            while (temp)
            {
                temp = CheckThreads();
            }
        }
        private bool CheckThreads()
        {
            foreach (Thread t in _threadList)
            {
                if (t.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }
    }
}