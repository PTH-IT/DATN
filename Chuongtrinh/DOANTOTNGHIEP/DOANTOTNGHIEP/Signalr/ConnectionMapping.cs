using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Signalr
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<DOANTOTNGHIEP.Models.Modelcreate.chat>> _connections =
            new Dictionary<T, HashSet<DOANTOTNGHIEP.Models.Modelcreate.chat>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId , long time)
        {
            lock (_connections)
            {
                HashSet<DOANTOTNGHIEP.Models.Modelcreate.chat> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<DOANTOTNGHIEP.Models.Modelcreate.chat>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    DOANTOTNGHIEP.Models.Modelcreate.chat connec = new Models.Modelcreate.chat();
                    connec.time = time;
                    connec.token = connectionId;
                    connections.Add(connec);
                }
            }
        }

        public bool GetConnections(T key)
        {
           
            foreach (var s in _connections)
            {
                if (key.Equals(s.Key))
                {
                    return true;
                }
               

            }
            return false;

           
        }
        public (List<string> ,List<DOANTOTNGHIEP.Models.Modelcreate.chat>) GetTokenConnections()
        {
            List<DOANTOTNGHIEP.Models.Modelcreate.chat> listtoken = new List<DOANTOTNGHIEP.Models.Modelcreate.chat>();
            List<string> key = new List<string>();
            foreach (var s in _connections)
            {
                key.Add(s.Key.ToString());
                listtoken.AddRange(s.Value.ToList());

            }
            return (key , listtoken);


        }
        public List<string> GetKeyConnections()
        {
            List<string> key = new List<string>();
            foreach (var s in _connections)
            {
                key.Add(s.Key.ToString());

            }
            return key;


        }

        public void Remove(T key)
        {
            lock (_connections)
            {
                _connections.Remove(key);
            }
        }
    }
}