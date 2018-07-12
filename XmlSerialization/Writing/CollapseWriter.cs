using System;
using System.Collections.Generic;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public class CollapseWriter : IWriter
    {
        public CollapseWriter(IWriter writer, bool collapseOnlyOptionalElements = false)
        {
            this.writer = writer;
            this.collapseOnlyOptionalElements = collapseOnlyOptionalElements;
            actionsList = new List<XmlWriterAction> {new XmlWriterAction("?", w => { }, false)};
            cur = 0;
        }

        public void Dispose()
        {
            Flush();
            writer.Dispose();
        }

        public void WriteEndElement()
        {
            Check("[?", "WriteEndElement");
            if (actionsList[actionsList.Count - 1].ActionType == "(" && actionsList.Count > 2 && actionsList[actionsList.Count - 1].CanBeRemoved)
                actionsList.RemoveAt(actionsList.Count - 1);
            else
            {
                actionsList.Add(new XmlWriterAction(")", w => w.WriteEndElement(), false));
                Flush();
            }
        }

        public void WriteEndAttribute()
        {
            Check("()?]", "WriteEndAttribute");
            if (actionsList[actionsList.Count - 1].ActionType == "[" && actionsList[actionsList.Count - 1].CanBeRemoved)
                actionsList.RemoveAt(actionsList.Count - 1);
            else
            {
                actionsList.Add(new XmlWriterAction("]", w => w.WriteEndAttribute(), false));
                Flush();
            }
        }

        public void WriteStartAttribute(XmlElementInfo xmlElementInfo)
        {
            Check("[*)?", "WriteStartAttribute");
            actionsList.Add(new XmlWriterAction("[", w => w.WriteStartAttribute(xmlElementInfo), CanBeRemoved(xmlElementInfo)));
        }

        public void WriteStartArrayElement(XmlElementInfo xmlElementInfo, int index)
        {
            Check("[*", "WriteStartElement");
            actionsList.Add(new XmlWriterAction("(", w => w.WriteStartArrayElement(xmlElementInfo, index), CanBeRemoved(xmlElementInfo)));
        }

        public void WriteStartElement(XmlElementInfo xmlElementInfo)
        {
            Check("[*", "WriteStartElement");
            actionsList.Add(new XmlWriterAction("(", w => w.WriteStartElement(xmlElementInfo), CanBeRemoved(xmlElementInfo)));
        }

        public void WriteValue(object value)
        {
            if (value != null)
            {
                Check(")*?", "WriteValue");
                actionsList.Add(new XmlWriterAction("*", w => w.WriteValue(value), false));
            }
        }

        public void WriteRawData(string data)
        {
            Check(")*?", "WriteValue");
            actionsList.Add(new XmlWriterAction("*", w => w.WriteRawData(data), false));
        }

        private bool CanBeRemoved(XmlElementInfo xmlElementInfo)
        {
            return !collapseOnlyOptionalElements || xmlElementInfo.Optional;
        }

        private void Flush()
        {
            while (cur < actionsList.Count)
                actionsList[cur++].Action(writer);
        }

        private void Check(string badActions, string currentAction)
        {
            if (badActions.Contains(actionsList[actionsList.Count - 1].ActionType))
                throw new InvalidOperationException(string.Format("Попытка выполнить '{0}' в ситуации, когда это запрещено", currentAction));
        }

        private readonly List<XmlWriterAction> actionsList;
        private readonly IWriter writer;
        private readonly bool collapseOnlyOptionalElements;
        private int cur;

        private class XmlWriterAction
        {
            public XmlWriterAction(string actionType, Action<IWriter> action, bool canBeRemoved)
            {
                CanBeRemoved = canBeRemoved;
                ActionType = actionType;
                Action = action;
            }

            public string ActionType { get; private set; }
            public bool CanBeRemoved { get; private set; }
            public Action<IWriter> Action { get; private set; }
        }
    }
}