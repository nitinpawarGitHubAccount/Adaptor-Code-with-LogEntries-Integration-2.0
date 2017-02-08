using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace Avalara.AvaTax.Adapter.MSMQ
{
    class MessageQueuing : IDisposable
    {
        MessageQueue msMq = null;
        
        public MessageQueuing()
        {

        }

        public MessageQueuing(string queueName)
        {
            lock(queueName)
            {
                if (!MessageQueue.Exists(queueName))
                {
                    msMq = MessageQueue.Create(queueName);
                }
                else
                {
                    msMq = new MessageQueue(queueName);
                }
            }
        }

        public void PushMessageInQueue(object message)
        {
            try
            {
                msMq.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }
            Dispose();
        }

        public void RetrieveMessagesFromQueue(string queueName)
        {
            try
            {
                msMq = new MessageQueue(queueName);
                msMq.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                var message = (String)msMq.Receive().Body;
            }
            catch (Exception)
            {

                throw;
            }
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            msMq.Close();
        }

        #endregion
    }
}
