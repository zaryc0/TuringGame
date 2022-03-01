using System;
using System.Collections.Generic;
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
        private float numberOfConnectionsMade;
        private ISocketHandler hostSocket;
        public List<IRoom> roomList;
        private List<Thread> threadList;
        private int roomPortBase;

        //Constructor
        public Control()
        {
            numberOfConnectionsMade = 0;
            hostSocket = new SocketHandler(4839);
            roomList = new List<IRoom>();
            threadList = new List<Thread>();
            roomPortBase = 1000;
            Thread T = new(()=>ManageController());
            T.Start();
        }


        //Functions
        public bool CreateRoom(int roomID,int subPortNum, int intPortNum)
        {
            if (roomList.Count < Constants.Room_Limit)
            {
                roomList.Add(new Room(subPortNum, intPortNum, false, roomID));
                threadList.Add(new Thread(() => ManageRoom(roomID)));
                threadList[roomID].Start();
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
            while (numberOfConnectionsMade < 10)
            {
                hostSocket.WaitForConnection();
          
                numberOfConnectionsMade++;
                int requiredRooms = (int)((numberOfConnectionsMade / 2) + 0.5);
                int roomID = roomList.Count;
                if (requiredRooms > roomList.Count)
                {
                    roomPortBase += (100 * roomID);
                    int subport = roomPortBase + 50;
                    CreateRoom(roomID, subport, roomPortBase);
                    Bus.CreateNewRoomView(roomList[roomID]);
                    hostSocket.Broadcast( new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{roomPortBase},<MessageEnd/>"));
                }
                else
                {
                    int subport = roomPortBase + 50;
                    hostSocket.Broadcast(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{subport},<MessageEnd/>"));
                }
                
            }
        }
    }
}