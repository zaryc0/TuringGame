using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotWrapper.Network.Filters
{
    class TagFilter : IFilter
    {
        private string _tag;
        public TagFilter(string tag)
        {
            _tag = tag;
        }
        public IMessage Run(IMessage message)
        {
            message.TypeTag = _tag;
            return message;
        }
    }
}
