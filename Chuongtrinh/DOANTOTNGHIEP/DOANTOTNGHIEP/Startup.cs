using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(DOANTOTNGHIEP.Startup))]

namespace DOANTOTNGHIEP
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           /* GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);*/
            app.MapSignalR();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var x = DOANTOTNGHIEP.Models.database.daovan.comparetsentence("Sinh viên ngành Công học hệ chính quy nhưng chưa hết thời gian đào tạo tối đa.", "Sinh viên ngành Công nghệ thông tin các lớp đã kết thúc khóa học hệ ");

                var y = x;
        }
    }
}
