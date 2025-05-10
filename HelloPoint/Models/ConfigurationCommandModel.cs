using HelloPoint.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace HelloPoint.Models
{
    public class ConfigurationCommandModel
    {
        int queueTimeoutSeconds = 10;

        public ResponseConfigurationMessage GetMonitorData(int mindex)
        {
            return Move("GETSETTINGS", mindex); ;
        }

        public ResponseConfigurationMessage MoveUp(int mindex)
        {
            return Move("MOVEUP", mindex); ;
        }

        public ResponseConfigurationMessage MoveDown(int mindex)
        {
            return Move("MOVEDOWN", mindex); ;
        }

        public ResponseConfigurationMessage MoveLeft(int mindex)
        {
            return Move("MOVELEFT", mindex); ;
        }

        public ResponseConfigurationMessage MoveRight(int mindex)
        {
            return Move("MOVERIGHT", mindex); ;
        }

        public ResponseConfigurationMessage ScaleUp(int mindex)
        {
            return Move("SCALEUP", mindex); ;
        }

        public ResponseConfigurationMessage ScaleDown(int mindex)
        {
            return Move("SCALEDOWN", mindex); ;
        }

        public ResponseConfigurationMessage Reset()
        {
            return Move("RESET", 0);
        }


        private ResponseConfigurationMessage Move(string commandtext, int mindex)
        {
            var responseAddress = string.Format(".\\private$\\{0}",
                                        Guid.NewGuid().ToString().Substring(0, 6));
            var responseMessage = new ResponseConfigurationMessage();
            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new ConfigurationMessage(commandtext, mindex);
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    string myip=null;
                    foreach (var ip in host.AddressList)
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        { myip = ip.ToString(); break; }
                    using (var queue = new MessageQueue("FormatName:Direct=TCP:"+myip+ "\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));
                        message.Label = "ConfigurationMessage";
                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseConfigurationMessage>(responseJsonBody);
                }
            }
            catch
            {
                if (MessageQueue.Exists(responseAddress))
                {
                    MessageQueue.Delete(responseAddress);
                }
            }
            finally
            {
                if (MessageQueue.Exists(responseAddress))
                {
                    MessageQueue.Delete(responseAddress);
                }
            }
            return responseMessage;
        }


        public ResponseMonitorState TurnOn()
        {
            return Turn("SWITCHON");
        }

        public ResponseMonitorState TurnOff()
        {
            return Turn("SWITCHOFF");
        }

        public ResponseMonitorState GetMonitorState()
        { 
            return Turn("GETMONITORSTATE");
        }

        private ResponseMonitorState Turn(string commandtext)
        {
            var responseAddress = string.Format(".\\private$\\{0}",
                                        Guid.NewGuid().ToString().Substring(0, 6));
            var responseMessage = new ResponseMonitorState();
            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new ConfigurationMessage(commandtext, 0);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));
                        message.Label = "ConfigurationMessage";
                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMonitorState>(responseJsonBody);
                }
            }
            catch
            {
                if (MessageQueue.Exists(responseAddress))
                {
                    MessageQueue.Delete(responseAddress);
                }
            }
            finally
            {
                if (MessageQueue.Exists(responseAddress))
                {
                    MessageQueue.Delete(responseAddress);
                }
            }
            return responseMessage;
        }
    }
}