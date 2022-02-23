using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF_Controller.Controller
{
    internal class Constants
    {
        public const int Room_Limit = 5;

        public const int Initial_View_ID = 0;
        public const int Interviewer_View_ID = 1;
        public const int Subject_View_ID = 2;

        public const string Interviewer_Tag = "<INTERVIEWER/>";
        public const string Subject_Tag = "<SUBJECT/>";
        public const string Message_End_Tag = "<MESSAGEEND/>";

        public const string Message_Type_Terminate_Tag = "<TERMINATE/>";
        public const string Message_Type_Init_Tag = "<INITIALISE/>";
        public const string Message_Type_Question_Tag = "<QUESTION/>";
        public const string Message_Type_Answer_Tag = "<ANSWER/>";
        public const string Message_Type_Submission_Tag = "<SUBMISSION/>";
        public const string Submission_Robot_Tag = "<ROBOT/>";
        public const string Submission_Human_Tag = "<HUMAN/>";
    }

    internal enum Roles
    {
        Interviewer,
        Subject,
        None
    }
    internal enum SubjectEnum
    {
        Robot,
        Human,
        None
    }
}
