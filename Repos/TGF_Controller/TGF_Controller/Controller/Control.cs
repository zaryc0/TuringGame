using System.Collections.Generic;
using System.Threading;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Controller.Network;

namespace TGF_Controller.Controller
{
    class Control
    {
        //Properties
        private SocketHandler hostSocket;
        private List<IRoom> roomList;
        private List<Thread> threadList;

        //Constructor
        public Control()
        {
            hostSocket = new SocketHandler(4839);
            roomList = new List<IRoom>();
            threadList = new List<Thread>();
        }


        //Functions
        public bool CreateRoom(int roomID,int subPortNum, int intPortNum)
        {
            if (roomList.Count < Constants.Room_Limit)
            {
                roomList.Add(new Room(subPortNum, intPortNum, false));
                threadList.Add(new Thread(() => ManageRoom(roomList[roomID])));
                threadList[roomID].Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ManageRoom(IRoom room)
        {
            room.Run();
        }
    }
}
