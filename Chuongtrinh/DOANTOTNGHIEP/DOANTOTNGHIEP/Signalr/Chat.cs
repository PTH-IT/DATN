using DOANTOTNGHIEP.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DOANTOTNGHIEP.Signalr
{
    [HubName("chat")]
    public class Chat : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
       
        public void Messages(string sender, string receiver, string malop, string message)
        {
            DB db = new DB();
            var tokenreceiver = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(receiver));
            var userreceiver = tokenreceiver.token;
            var tokensend = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(sender));
            var usersend = tokensend.token;
            Clients.Client(usersend).message(sender, receiver, malop, message);
            if (userreceiver != null)
            {
               
                Clients.Client(userreceiver).message(sender, receiver, malop, message);
            }
            


        }
        public void classnames(string sender, string receiver, string malop)
        {

            DB db = new DB();
            var token = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(receiver));
            var user = token.token;
            if (user != null)
            {
                Clients.Client(user).classname(Models.GetData.GetClass.getnameclass(malop));
            }
            
            


        }
        public void Thongbaos(string malop, string nguoidang)
        {

            Clients.All.thongbao(malop, nguoidang);
        }
        public void Baitaps(string malop)
        {

            Clients.All.baitap(malop);
        }




        public void removeTokenWhenTimeover()
        {
            var connections = _connections.GetTokenConnections();
            for(int i = 0; i < connections.Item2.Count; i++)
            {
                if (connections.Item2.ToArray()[i].time <= DateTimeOffset.Now.ToUnixTimeSeconds() )
                {
                    _connections.Remove(connections.Item1.ToArray()[i]);
                }
            }
            
        }

          public override Task OnConnected()
        {
            
            DB db = new DB();
            var  name = Context.RequestCookies.ToList().SingleOrDefault(x => x.Key.Equals("user")) ;
            var indexsub = name.Value.Value.ToString().IndexOf("&Matkhau");
            var username = Models.crypt.Encrypt.Decryptuser(name.Value.Value.ToString().Remove(indexsub, name.Value.Value.ToString().Length - indexsub).Replace("TenDangNhap=", ""));

            if (!_connections.GetConnections(username))
            {
                long seconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 150;
                _connections.Add(username, Context.ConnectionId, seconds);
            }
            var ConnectionId = Context.ConnectionId;
            var tk = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(username));
            tk.token = ConnectionId;
            db.SaveChanges();

            removeTokenWhenTimeover();
            var s = _connections.GetKeyConnections();
            Clients.All.clientonline(s);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = Context.RequestCookies.ToList().SingleOrDefault(x => x.Key.Equals("user"));
            var indexsub = name.Value.Value.ToString().IndexOf("&Matkhau");
            var username = Models.crypt.Encrypt.Decryptuser(name.Value.Value.ToString().Remove(indexsub, name.Value.Value.ToString().Length - indexsub).Replace("TenDangNhap=", ""));
            
            removeTokenWhenTimeover();
            var s = _connections.GetKeyConnections();
            Clients.All.clientonline(s);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            DB db = new DB();
            var name = Context.RequestCookies.ToList().SingleOrDefault(x => x.Key.Equals("user"));
            var indexsub = name.Value.Value.ToString().IndexOf("&Matkhau");
            var username = Models.crypt.Encrypt.Decryptuser(name.Value.Value.ToString().Remove(indexsub, name.Value.Value.ToString().Length - indexsub).Replace("TenDangNhap=", ""));

            if (!_connections.GetConnections(username))
            {
                long seconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 150;
                _connections.Add(username, Context.ConnectionId, seconds);
            }
            var ConnectionId = Context.ConnectionId;
            var tk = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(username));
            tk.token = ConnectionId;
            db.SaveChanges();

            removeTokenWhenTimeover();
            var s = _connections.GetKeyConnections();
            Clients.All.clientonline(s);
            return base.OnConnected();
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }

        public void SendMessageToGroup(string userName , string groupName, string malop , string message)
        {
            Clients.Group(groupName).ReceiveMessage(userName, malop , message);
        }
    }
}