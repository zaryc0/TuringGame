using System.Collections.Generic;
using System.Threading;
using TGF_Controller.Model;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller
{
    class Controller
    {
        //Properties
        private List<IRoom> roomList;
        private List<Thread> threadList;

        //Constructor
        public Controller()
        {
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
