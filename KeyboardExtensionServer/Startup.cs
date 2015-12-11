using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace KeyboardExtensionServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.ContentType = "text/html";
                var strReader = new StreamReader(new FileStream("index.html", FileMode.Open));
                var readToEnd = strReader.ReadToEnd();
                strReader.Close();
                return context.Response.WriteAsync(readToEnd);
            });
        }
    }
}
