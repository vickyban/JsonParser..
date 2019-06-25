using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTree
{
    class JsonParser
    {
        private Reader reader;
        public JsonParser(string content)
        {
            reader = new Reader(content);
        }

        public Dictionary<string,object> Parse()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "root";
            object value = null;
            StringBuilder builder = new StringBuilder();
            while (reader.HasNext())
            {
                char c = reader.Next();
                if (c == '{')
                {
                    value = Parse();
                }
                else if (c == '}')
                {
                    if (builder.Length != 0) value = builder.ToString().Trim();
                    dic.Add(key, value);
                    return dic;
                }
                else if(c == '[')
                {
                    value = ParseArray();
                }
                else if(c == ':')
                {
                    key = builder.ToString().Trim();
                    builder.Clear();
                }else if(c == ',')
                {
                    if (builder.Length != 0) value = builder.ToString().Trim();
                    dic.Add(key, value);
                    builder.Clear();
                }
                else
                {
                    builder.Append(c);
                }
            }
            dic.Add(key, value);
            return dic;
        }

        public List<object> ParseArray()
        {
            List<object> list = new List<object>();
            object item = null;
            StringBuilder builder = new StringBuilder();
            while (reader.HasNext())
            {
                char c = reader.Next();
                if (c == ']')
                {
                    if (item != null) list.Add(item);
                    break;
                }
                if(c == '{')
                {
                    item = Parse();
                }
                else if(c == ',')
                {
                    if (builder.Length != 0) item = builder.ToString().Trim();
                    list.Add(item);
                    builder.Clear();
                }
                else
                {
                    builder.Append(c);
                }
               
            }
            return list;
        }
    }

    class Reader
    {
        private string _content;
        private int _length;
        private int _curIndex;
        public Reader(string content)
        {
            this._content = content;
            this._length = content.Length;
            _curIndex = 0;
        }

        public bool HasNext()
        {
            return _curIndex < _length;
        }

        public char Next()
        {
            return _content[_curIndex++];
        }

    }

}
