using System;
using HelloPoint.Communication;
using System.Messaging;
using Newtonsoft.Json;
using System.IO;
using System.Text;



namespace HelloPoint.Models
{
    public class PlayListModel
    {
        private const int queueTimeoutSeconds = 10;
        public ResponseMessage GetPlayList()
        {
            var responseAddress = string.Format(".\\private$\\{0}",
                                        Guid.NewGuid().ToString().Substring(0, 6));
            var responseMessage = new ResponseMessage();
            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage("GETPLAYLIST", null);

                    using (var queue = new MessageQueue("FormatName:DIRECT=OS:"+Environment.MachineName+"\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    //the TimeSpan makes the queue timeout and send an exception if it didn't
                    //receive a response
                    var response = responseQueue.Receive(new TimeSpan(0,0,queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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

        public ResponseMessage AddElement(string guid, string type, string description)
        {
            string command = "ADD";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(description, guid, type, false, false,0,1);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));
            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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


        public ResponseMessage Play(int id)
        {
            string command = "PLAY";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(id);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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


        public ResponseMessage Pause()
        {
            string command = "PAUSE";
            var responseMessage = new ResponseMessage();
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, null);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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

        public ResponseMessage Stop()
        {
            string command = "STOP";
            var responseMessage = new ResponseMessage();
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, null);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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

        public ResponseMessage ClearAll()
        {
            string command = "CLEAR";
            var responseMessage = new ResponseMessage();
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, null);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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


        public ResponseMessage MoveUp(int id)
        {
            string command = "MOVEUP";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(id);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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


        public ResponseMessage MoveDown(int id)
        {
            string command = "MOVEDOWN";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(id);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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

        public ResponseMessage Remove(int id)
        {
            string command = "REMOVE";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(id);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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


        public ResponseMessage ModifyRepeatNumber(int id, int repetnumber)
        {
            string command = "SETREPETITIONS";
            var responseMessage = new ResponseMessage();
            PlaylistElement element = new PlaylistElement(id, repetnumber);
            var responseAddress = string.Format(".\\private$\\{0}",
                                    Guid.NewGuid().ToString().Substring(0, 6));

            try
            {
                using (var responseQueue = MessageQueue.Create(responseAddress))
                {

                    var commandMsg = new CommandMessage(command, element);

                    using (var queue = new MessageQueue(".\\private$\\commandReceive"))
                    {
                        var message = new Message();
                        var jsonBody = JsonConvert.SerializeObject(commandMsg);
                        message.BodyStream = new MemoryStream(Encoding.Default.GetBytes(jsonBody));

                        message.ResponseQueue = responseQueue;
                        queue.Send(message);
                    }
                    var response = responseQueue.Receive(new TimeSpan(0, 0, queueTimeoutSeconds));
                    var responseBody = new StreamReader(response.BodyStream);
                    var responseJsonBody = responseBody.ReadToEnd();
                    responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseJsonBody);
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