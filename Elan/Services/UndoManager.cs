using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Elan.Services
{
    internal class UndoManager
    {
        public UndoManager()
        {
            _list = new MemoryStream[10];
            _capacity = 10;
        }

        private readonly MemoryStream[] _list;

        private readonly int _capacity;

        private int _curPos = -1;

        private int _lastPos = -1;

        public bool CanUndo => _curPos != -1;

        public bool CanRedo => _curPos != _lastPos;

        public bool Enabled { get; set; } = true;

        public void AddUndo(object o)
        {
            if (!Enabled)
            {
                return;
            }

            _curPos++;
            if (_curPos >= _capacity)
            {
                _curPos--;
            }

            ClearList(_curPos);

            PushList();

            _list[_curPos] = SerializeObject(o);
            _lastPos = _curPos;
        }

        public object Undo()
        {
            if (!CanUndo)
            {
                throw new ApplicationException("Не возможно отменить");
            }

            var ret = DeserializeObject(_list[_curPos]);

            _curPos--;

            return ret;
        }

        public object Redo()
        {
            if (!CanRedo)
            {
                throw new ApplicationException("Не возможно возвратить");
            }

            _curPos++;

            return DeserializeObject(_list[_curPos]);
        }

        private static MemoryStream SerializeObject(object o)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, o);
            stream.Position = 0;
            return stream;
            
        }

        private static object DeserializeObject(Stream stream)
        {
            stream.Position = 0;
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        private void ClearList(int p)
        {
            if (_curPos >= _capacity - 1)
            {
                return;
            }

            for (var i = p; i < _capacity; i++)
            {
                if (_list[i] != null)
                {
                    _list[i].Close();
                }
                _list[i] = null;
            }
        }

        private void PushList()
        {
            if ((_curPos >= _capacity - 1) && (_list[_curPos] != null))
            {
                _list[0].Close();
                for (var i = 1; i <= _curPos; i++)
                {
                    _list[i - 1] = _list[i];
                }
            }
        }
    }
}