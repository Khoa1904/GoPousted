using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Events
{
    public struct EventCode
    {
        public const string Print = "PRINT"; 
        public const string Calendar = "CALENDAR";
        public const string Insert = "INSERT";
        public const string Delete = "DELETE";
        public const string Search = "SEARCH";
    }
    public class GoPOSEventHandler
    {
        public delegate void GeneralEventHandler<T>(object sender, EventArgs<T> e);
    }

    public class EventArgs<T> : EventArgs
    {
        public T Data { get; private set; }

        public EventArgs(T data)
        {
            Data = data;
        }
    }
    public class CloseEventArgs : EventArgs
    {
        public object Data { get; private set; }
        public CloseEventArgs(object data)
        {
            Data = data;
        }
    }
    public class MenuEventArgs : EventArgs
    {
        public object Data { get; private set; } = new object();
        public string MenuCode { get; private set; } = string.Empty;
        public string SubMenu { get; private set; } = string.Empty;
        public MenuEventArgs(string menuCode, string subMenu, object data)
        {
            Data = data;
            MenuCode = menuCode;
            SubMenu = subMenu;
        }

        public MenuEventArgs(string menuCode, object data)
        {
            Data = data;
            MenuCode = menuCode;
        }

        public MenuEventArgs(string menuCode)
        {
            MenuCode = menuCode;
        }
    }

    public enum PageItemEventTypes
    {
        ActiveItem,
        DeactiveItem,
        ClosePage,
        DataEvent
    }

    public class PageItemEventArgs : EventArgs
    {
        public PageItemEventArgs(string parentType, string itemName, PageItemEventTypes eventType, params object[] eventData)
        {
            ParentType = parentType;
            ItemName = itemName;
            EventType = eventType;
            EventData = eventData;
        }

        public PageItemEventArgs()
        {

        }

        public string ParentType { get; set; }
        public string ItemName { get; set; }
        public PageItemEventTypes EventType { get; set; }
        public object[] EventData { get; set; }
    }

    public class BasePageClosedEventArgs : EventArgs
    {

    }

    public class StringCollectionEventArgs : EventArgs
    {
        public ICollection<String> Collection { get; private set; }

        public StringCollectionEventArgs(ICollection<String> collection)
        {
            Collection = collection;
        }
    }
    public delegate void StringCollectionEventHandler(object sender, StringCollectionEventArgs e);
    public enum L2LMessageType
    {
        Information,
        Warning,
        Error,
        Critical,
    }

    public class L2LMessageEventArgs : EventArgs
    {
        /// <summary>
        /// This type of message
        /// </summary>
        public L2LMessageType Type { get; private set; }
        /// <summary>
        /// String that contain a message
        /// </summary>
        public String Message { get; private set; }
        /// <summary>
        /// This code defined by each library. 0 is not used
        /// </summary>
        public int Code { get; private set; }

        public L2LMessageEventArgs(L2LMessageType type, String message)
        {
            Type = type;
            Message = message;
            Code = 0;
        }

        public L2LMessageEventArgs(L2LMessageType type, String message, int code)
        {
            Type = type;
            Message = message;
            Code = code;
        }
    }
    public delegate void L2LMessageEventHandler(object sender, L2LMessageEventArgs e);
    //public delegate void GeneralEventHandler<T>(object sender, EventArgs<T> e);
    public class MessageEventArgs : EventArgs
    {
        public String Message { get; private set; }

        public MessageEventArgs(String message)
        {
            Message = message;
        }
    }
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public class DataEventArgs : EventArgs
    {
        public Byte[] Data { get; private set; }

        public DataEventArgs(Byte[] data)
        {
            Data = data;
        }
    }
    public delegate void DataEventHandler(object sender, DataEventArgs e);

    public class BooleanEventArgs : EventArgs
    {
        public bool Value { get; private set; }

        public BooleanEventArgs(bool val)
        {
            Value = val;
        }
    }
    public delegate void BooleanEventHandler(object sender, BooleanEventArgs e);
    public delegate bool GeneralEventStringHandler<T>(object sender, EventArgs<T> e);
    public delegate void GeneraEventChangeConfigHandler<T>(object sender, EventArgs<T> e);
}
